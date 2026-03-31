

using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using RuriMegu.Core.Cards;
using RuriMegu.Core.Powers;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Relics;

/// <summary>
/// Linkura Charm - Starter relic for Hinoshita Kaho.
/// </summary>
public class LinkuraSystem : LinkuraRelic {
  public override RelicRarity Rarity => RelicRarity.Starter;
  public override bool ShouldReceiveCombatHooks => true;

  public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
  protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
  protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();

  protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<AutoBurstPower>(),
    BurstHeartsVar.LocKey.HoverTip(new BurstHeartsVar(2)),
    HoverTipFactory.FromKeyword(LinkuraKeywords.Collect)
  ];

  public override async Task BeforeCombatStart() {
    await HeartsState.Reset(Owner);
    await PowerCmd.Apply<AutoBurstPower>(Owner.Creature, 2, Owner.Creature, null);
  }
}
