using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;
using RuriMegu.Core.Characters;
using RuriMegu.Core.Nodes.Combat;

namespace RuriMegu.Core.Patches;

/// <summary>
/// After <see cref="NCombatUi.Activate"/> runs and adds the energy counter to the
/// EnergyCounterContainer, we look for the <see cref="NHeartCounter"/> node that lives
/// inside the Kaho energy counter scene and call <c>Initialize(player)</c> on it.
///
/// This is safe for non-Kaho characters — if the energy counter has no HeartCounter
/// child the lookup returns null and we bail out.
/// </summary>
[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
public static class KahoHeartCounterPatch {
  [HarmonyPostfix]
  public static void Postfix(NCombatUi __instance, CombatState state) {
    Player me = LocalContext.GetMe(state);

    // Only run for Kaho
    if (me.Character is not HinoshitaKaho) return;

    // The energy counter was added to EnergyCounterContainer; find the embedded HeartCounter.
    Control energyCounterContainer = __instance.EnergyCounterContainer;
    NHeartCounter heartCounter = null;

    foreach (Node child in energyCounterContainer.GetChildren()) {
      heartCounter = child.GetNodeOrNull<NHeartCounter>("%HeartCounter");
      if (heartCounter is not null) break;
    }

    if (heartCounter is null) {
      LinkuraMod.Logger.Warn(
        "KahoHeartCounterPatch: HeartCounter not found inside energy counter — " +
        "check that kaho_energy_counter.tscn contains a HeartCounter child.");
      return;
    }

    heartCounter.Initialize(me);
  }
}
