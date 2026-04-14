using BaseLib.Abstracts;
using BaseLib.Extensions;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Potions;

/// <summary>
/// Base class for all Linkura mod potions.
/// Provides image path resolution based on character ID.
/// </summary>
public abstract class LinkuraPotion : CustomPotionModel {
  public abstract string CharacterId { get; }

  public override string CustomPackedImagePath =>
    $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath(CharacterId);

  public override string CustomPackedOutlinePath =>
    $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath(CharacterId);
}
