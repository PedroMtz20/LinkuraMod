using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards.Kaho;

/// <summary>
/// Special Thanks — Cost 1, Skill (Common).
/// On play: Draw 1 card.
/// Backstage: whenever the player plays an Attack, Burst Hearts 4.
/// </summary>
public class SpecialThanks() : InHandTriggerCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) {
  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CardsVar(1),
    new BurstHeartsVar(4),
  ];

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play) {
    await CommonActions.Draw(this, choiceContext);
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) {
    await base.AfterCardPlayed(context, cardPlay);
    if (cardPlay.Card.Type != CardType.Attack) return;
    if (!TryTrigger(cardPlay)) return;
    await LinkuraCardActions.BurstHearts(this);
  }

  protected override void OnUpgrade() {
    DynamicVars.BurstHearts().UpgradeValueBy(2m);
  }
}
