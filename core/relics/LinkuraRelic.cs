using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using RuriMegu.Core.Characters;

namespace RuriMegu.Core.Relics;

/// <summary>
/// Base class for all Linkura-pool relics.
/// Inheriting from this automatically places relics in the Linkura relic pool.
/// </summary>
[Pool(typeof(LinkuraRelicPool))]
public abstract class LinkuraRelic : CustomRelicModel {
  public override RelicRarity Rarity => RelicRarity.Common;


}
