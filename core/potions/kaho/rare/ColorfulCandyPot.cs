using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using RuriMegu.Core.Cards;
using RuriMegu.Core.Powers.Kaho;

namespace RuriMegu.Core.Potions.Kaho.Rare;

/// <summary>
/// Colorful Candy Pot — Rare potion for Hinoshita Kaho.
/// This turn, whenever you Burst, Collect.
/// </summary>
public class ColorfulCandyPot : KahoPotion {
  public override PotionRarity Rarity => PotionRarity.Rare;
  public override PotionUsage Usage => PotionUsage.CombatOnly;
  public override TargetType TargetType => TargetType.None;

  public override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
    BurstHeartsVar.HoverTip(),
    HoverTipFactory.FromKeyword(LinkuraKeywords.Collect)
  ]);

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target) {
    await PowerCmd.Apply<ColorfulCandyPotPower>(Owner.Creature, 1, Owner.Creature, null);
  }
}
