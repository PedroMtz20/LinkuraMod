using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards.Kaho.Rare.Attack;

/// <summary>
/// The Grand Finale — Cost 1, Attack, Rare.
/// Deal damage equal to your current max ❤️ to one enemy (ALL enemies). Reset max ❤️ to 9. Exhaust.
/// </summary>
public class TheGrandFinale() : LinkuraCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) {
  public override TargetType TargetType => IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;

  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(0),
    new ExtraDamageVar(1),
    new CalculatedDamageVar(ValueProp.Move).WithMultiplier((_, creature) => HeartsState.GetMaxHearts(creature.Player)),
  ];

  public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    if (TargetType == TargetType.AllEnemies) {
      await CommonActions.CardAttack(this, play).Execute(ctx);
    } else {
      await CommonActions.CardAttack(this, play.Target).Execute(ctx);
    }
    await HeartsState.SetMaxHearts(Owner, HeartsState.DEFAULT_MAX_HEARTS, this);
  }

  protected override void OnUpgrade() {
    // TargetType is overridden, nothing to do here.
  }
}
