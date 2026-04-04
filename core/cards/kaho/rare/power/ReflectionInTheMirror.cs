using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Powers;

namespace RuriMegu.Core.Cards.Kaho.Rare.Power;

/// <summary>
/// Reflection in the Mirror (镜中倒影) — Cost 1 (0), Power, Rare.
/// Effects that increase Max ❤️ are doubled.
/// </summary>
public class ReflectionInTheMirror() : LinkuraCard(1, CardType.Power, CardRarity.Rare, TargetType.None) {
  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    await PowerCmd.Apply<ReflectionInTheMirrorPower>(Owner.Creature, 1, Owner.Creature, this);
  }

  protected override void OnUpgrade() {
    EnergyCost.UpgradeBy(-1);
  }
}
