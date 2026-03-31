using System.Reflection;
using BaseLib.Patches.Content;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.GameInfo.Objects;

namespace RuriMegu;

[ModInitializer(nameof(Initialize))]
public static class LinkuraMod {
  public const string ModId = "linkuramod";

  public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
  new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

  public static void Initialize() {
    Logger.Info("Link! Like! LoveLive! - LinkuraMod Initializing...");
    Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
    Harmony harmony = new(ModId);
    harmony.PatchAll();
  }
}
