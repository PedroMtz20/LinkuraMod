using System.IO;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace RuriMegu.Core.Utils;

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

  public static string RelicImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "relics", path);
  }

  public static string BigRelicImagePath(this string path) {
    return Path.Join(LinkuraMod.ModId, "images", "relics", "big", path);
  }

  public static string CharacterUiPath(this string path, string characterId = "") {
    return Path.Join(LinkuraMod.ModId, "images", "charui", characterId, path);
  }

  public static string CharacterScenePath(this string path, string characterId = "") {
    return Path.Join(LinkuraMod.ModId, "scenes", characterId, path);
  }

  private static LocString L10NStatic(string entry) {
    return new LocString("static_hover_tips", entry);
  }

  public static HoverTip HoverTip(this string locKey, params DynamicVar[] vars) {
    string text = locKey;
    LocString locString = L10NStatic(text + ".title");
    LocString locString2 = L10NStatic(text + ".description");
    foreach (DynamicVar dynamicVar in vars) {
      locString.Add(dynamicVar);
      locString2.Add(dynamicVar);
    }

    return new HoverTip(locString, locString2);
  }
}
