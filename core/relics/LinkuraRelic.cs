using System.Collections.Generic;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using RuriMegu.Core.Cards;
using RuriMegu.Core.Characters;
using RuriMegu.Core.Powers;

namespace RuriMegu.Core.Relics;

/// <summary>
/// Base class for all Linkura-pool relics.
/// Inheriting from this automatically places relics in the Linkura relic pool.
/// </summary>
[Pool(typeof(LinkuraRelicPool))]
public abstract class LinkuraRelic : CustomRelicModel {
  public override RelicRarity Rarity => RelicRarity.Common;

  protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<AutoBurstPower>(),
    HoverTipFactory.FromKeyword(LinkuraKeywords.Collect)
  ];
}
