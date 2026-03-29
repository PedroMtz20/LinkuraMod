using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using RuriMegu.Core.Characters;
using RuriMegu.Core.Extensions;

namespace RuriMegu.Core.Cards;

/// <summary>
/// Base class for all Linkura-pool cards.
/// Inheriting from this automatically places cards in the Linkura card pool.
/// </summary>
[Pool(typeof(LinkuraCardPool))]
public abstract class LinkuraCard(int cost, CardType type, CardRarity rarity, TargetType target)
  : CustomCardModel(cost, type, rarity, target) {
  public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
  public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
  public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}
