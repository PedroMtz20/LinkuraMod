using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RuriMegu.Core.Cards;

namespace RuriMegu.Core.Utils;

public static class CardExtensions {
  public static DynamicVar ExpandHearts(this DynamicVarSet vars) {
    if (!vars.TryGetValue(ExpandHeartsVar.Key, out var value)) {
      LinkuraMod.Logger.Error($"ExpandHeartsVar not found for card!");
      return null;
    }
    return value;
  }

  public static DynamicVar BurstHearts(this DynamicVarSet vars) {
    if (!vars.TryGetValue(BurstHeartsVar.Key, out var value)) {
      LinkuraMod.Logger.Error($"BurstHeartsVar not found for card!");
      return null;
    }
    return value;
  }
}
