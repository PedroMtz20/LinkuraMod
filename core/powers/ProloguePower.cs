using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// The first time Max ❤️ changes each turn, the next card costs {Amount:energyIcons()} less.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Rare.Power.Prologue"/>.
/// </summary>
public class ProloguePower : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Counter;

  private bool _triggeredThisTurn;
  private bool _costReductionPending;

  public override Task AfterApplied(Creature applier, CardModel cardSource) {
    DisposeTrackedSubscriptions();
    TrackSubscription(Events.MaxHeartsChanged.SubscribeLate(OnMaxHeartsChangedLate));
    return base.AfterApplied(applier, cardSource);
  }

  public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState) {
    await base.BeforeSideTurnStart(choiceContext, side, combatState);
    if (side == Owner.Side) {
      _triggeredThisTurn = false;
    }
  }

  public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost) {
    modifiedCost = originalCost;
    if (card.Owner.Creature != Owner || !_costReductionPending) return false;
    if (originalCost <= 0m) return false;
    modifiedCost = System.Math.Max(0m, originalCost - Amount);
    return true;
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay) {
    if (cardPlay.Card.Owner.Creature == Owner && _costReductionPending) {
      _costReductionPending = false;
    }
    return base.BeforeCardPlayed(cardPlay);
  }

  private Task OnMaxHeartsChangedLate(Events.MaxHeartsChangedEvent ev) {
    if (ev.Player.Creature != Owner || _triggeredThisTurn) return Task.CompletedTask;
    _triggeredThisTurn = true;
    _costReductionPending = true;
    Flash();
    return Task.CompletedTask;
  }
}
