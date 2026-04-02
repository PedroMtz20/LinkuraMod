using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using RuriMegu.Core.Powers;

namespace RuriMegu.Core.Cards.Kaho.Common.Attack;

/// <summary>
/// 37.5°C's Fantasy — Cost 3, Attack, Common.
/// Deal 37 damage. All Skill cards cost 2 (3) less this turn, reset after any Skill card is played.
/// Uses <see cref="SkillCostReductionPower"/> so the discount applies to cards drawn or generated
/// after this card is played.
/// </summary>
public class Fantasy375() : LinkuraCard(3, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {
  private const int BASE_DAMAGE = 37;
  private const int BASE_REDUCTION = 2;

  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new DamageVar(BASE_DAMAGE, ValueProp.Move),
    new EnergyVar(BASE_REDUCTION)
  ];

  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    await CommonActions.CardAttack(this, play.Target).Execute(ctx);
    // Apply a power that globally reduces all Skill card costs by the reduction amount.
    // The power also handles drawn/generated cards and removes itself on first Skill card play.
    int reduction = DynamicVars.Energy.IntValue;
    await PowerCmd.Apply<SkillCostReductionPower>(Owner.Creature, reduction, Owner.Creature, this);
  }

  protected override void OnUpgrade() {
    EnergyCost.UpgradeBy(-1);
  }
}
