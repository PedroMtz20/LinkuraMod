using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards;

/// <summary>
/// Base class for cards whose effect triggers when a certain condition is met
/// while the card is in the player's hand.
///
/// A guard of <see cref="MAX_TRIGGERS_PER_PLAY"/> prevents runaway loops.
/// </summary>
public abstract class InHandTriggerCard(int cost, CardType type, CardRarity rarity, TargetType target)
  : LinkuraCard(cost, type, rarity, target) {

  /// <summary>Maximum times this card's in-hand effect may fire per player card play.</summary>
  public const int MAX_TRIGGERS_PER_PLAY = 999;

  public override IEnumerable<CardKeyword> CanonicalKeywords => [LinkuraKeywords.Backstage];

  private int _triggerCount;

  protected abstract Task OnBackstageTrigger(PlayerChoiceContext context, CardPlay cardPlay);

  protected async Task<bool> TryTrigger(PlayerChoiceContext context, CardPlay cardPlay) {
    if (!CanTrigger(cardPlay)) return false;
    var ev = new Events.TriggerBackstageEvent(Owner, this);
    if (!Events.TriggerBackstage.InvokeAllEarly(ev)) return false;
    _triggerCount++;
    await OnBackstageTrigger(context, cardPlay);
    Events.TriggerBackstage.InvokeAllLate(ev);
    return true;
  }

  protected virtual bool CanTrigger(CardPlay cardPlay) {
    if (_triggerCount >= MAX_TRIGGERS_PER_PLAY) return false;
    if (cardPlay.Card == this || cardPlay.Card.Owner != Owner) return false;
    if (!this.IsInHand()) return false;
    return true;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) {
    await base.AfterCardPlayed(context, cardPlay);
    // Reset counter only when this card is manually played by the player.
    if (cardPlay.Card == this && !cardPlay.IsAutoPlay) {
      _triggerCount = 0;
    }
  }
}
