using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// This turn, your next Burst Heart card deals damage equal to Burst count to ALL enemies.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Uncommon.Skill.TrueFacePixel"/>.
/// </summary>
public class TrueFacePixelPower : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Single;

  private Subscription _sub;
  private CardModel _triggeringCard;

  public override Task AfterApplied(Creature applier, CardModel cardSource) {
    _sub?.Dispose();
    _sub = Events.Burst.SubscribeLate(OnBurst);
    return base.AfterApplied(applier, cardSource);
  }

  public override Task AfterRemoved(Creature oldOwner) {
    _sub?.Dispose();
    _sub = null;
    return base.AfterRemoved(oldOwner);
  }

  public override Task AfterCombatEnd(CombatRoom room) {
    _sub?.Dispose();
    _sub = null;
    return base.AfterCombatEnd(room);
  }

  private async Task OnBurst(Events.BurstEvent ev) {
    if (ev.Player.Creature != Owner || ev.ActualAmount <= 0 || ev.Source == null)
      return;

    if (_triggeringCard != null && ev.Source != _triggeringCard)
      return;

    // Deal damage equal to the amount actually burst to all opponents
    await CreatureCmd.Damage(
      new BlockingPlayerChoiceContext(),
      Owner.CombatState.GetOpponentsOf(Owner),
      ev.ActualAmount,
      ValueProp.Move,
      Owner,
      ev.Source
    );

    // Track which card triggered this so we can remove the power after the card finishes playing.
    _triggeringCard = ev.Source;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) {
    if (_triggeringCard != null && cardPlay.Card == _triggeringCard) {
      await PowerCmd.Remove(this);
    }
  }

  public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side) {
    await base.AfterTurnEnd(choiceContext, side);
    if (side == Owner.Side) {
      await PowerCmd.Remove(this);
    }
  }
}
