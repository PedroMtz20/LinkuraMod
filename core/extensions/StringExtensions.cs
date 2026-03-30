using System.IO;

namespace RuriMegu.Core.Extensions;

/// <summary>
/// Utility extension methods for resolving asset paths within the mod.
/// </summary>
public static class StringExtensions {
  public static string ImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", path);
  }

  public static string CardImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "card_portraits", path);
  }

  public static string BigCardImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "card_portraits", "big", path);
  }

  public static string PowerImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "powers", path);
  }

  public static string BigPowerImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "powers", "big", path);
  }

  public static string RelicImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "relics", path);
  }

  public static string BigRelicImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "relics", "big", path);
  }

  public static string CharacterUiPath(this string path, string characterId = "") {
    return Path.Join(LinkuraMod.ModId, "images", "charui", characterId, path);
  }
}
