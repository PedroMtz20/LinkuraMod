using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace RuriMegu.Core.Cards;

/// <summary>
/// Base class for cards whose effect triggers when a certain condition is met
/// while the card is in the player's hand.
///
/// The framework calls <see cref="AfterCardPlayedLate"/> for cards that are
/// currently in the player's hand after another card finishes being played.
/// A guard of <see cref="MAX_TRIGGERS_PER_PLAY"/> prevents runaway loops.
/// </summary>
public abstract class InHandTriggerCard(int cost, CardType type, CardRarity rarity, TargetType target)
  : LinkuraCard(cost, type, rarity, target) {

  /// <summary>Maximum times this card's in-hand effect may fire per player card play.</summary>
  public const int MAX_TRIGGERS_PER_PLAY = 999;

  public override IEnumerable<CardKeyword> CanonicalKeywords => [LinkuraKeywords.Backstage];

  private int _triggerCount;

  protected bool TryTrigger() {
    if (!CanTrigger) return false;
    _triggerCount++;
    return true;
  }

  protected bool CanTrigger => _triggerCount < MAX_TRIGGERS_PER_PLAY;

  public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) {
    await base.AfterCardPlayed(context, cardPlay);
    // Reset counter only when this card is manually played by the player.
    if (cardPlay.Card == this && !cardPlay.IsAutoPlay) {
      _triggerCount = 0;
    }
  }
}
