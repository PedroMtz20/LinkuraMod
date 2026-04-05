using System;
using System.Collections.Generic;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using RuriMegu.Core.Cards;

namespace RuriMegu.Core.Patches;

/// <summary>
/// When an <see cref="InHandTriggerCard"/> fires its backstage effect, shake the
/// card visually within its hand holder and play the relic-activate SFX.
///
/// Implementation: we hook <see cref="NHandCardHolder"/>'s private
/// SubscribeToEvents / UnsubscribeFromEvents to attach/detach a handler on the
/// card model's <see cref="InHandTriggerCard.Triggered"/> event.  The handler
/// tweens the inner <c>NCard</c>'s position (offset within the holder) for a
/// quick shake, which never conflicts with the holder's own position lerp.
/// </summary>
[HarmonyPatch(typeof(NHandCardHolder), "SubscribeToEvents")]
public static class BackstageCardShakeSubscribePatch {
  internal static readonly Dictionary<InHandTriggerCard, Action> Handlers = new();

  [HarmonyPostfix]
  public static void Postfix(NHandCardHolder __instance, CardModel card) {
    if (card is not InHandTriggerCard trigger || !__instance.IsInsideTree()) return;

    void OnTriggered() {
      if (!GodotObject.IsInstanceValid(__instance)) return;
      var cardNode = __instance.CardNode;
      if (cardNode == null || !GodotObject.IsInstanceValid(cardNode)) return;

      SfxCmd.Play(FmodSfx.relicFlashGeneral);
      __instance.Flash();

      var tween = __instance.CreateTween();
      tween.TweenProperty(cardNode, "position", new Vector2(24, 0), 0.05);
      tween.TweenProperty(cardNode, "position", new Vector2(-18, 0), 0.07);
      tween.TweenProperty(cardNode, "position", new Vector2(10, 0), 0.06);
      tween.TweenProperty(cardNode, "position", Vector2.Zero, 0.09);
    }

    Handlers[trigger] = OnTriggered;
    trigger.Triggered += OnTriggered;
  }
}

[HarmonyPatch(typeof(NHandCardHolder), "UnsubscribeFromEvents")]
public static class BackstageCardShakeUnsubscribePatch {
  [HarmonyPostfix]
  public static void Postfix(CardModel card) {
    if (card is not InHandTriggerCard trigger) return;
    if (BackstageCardShakeSubscribePatch.Handlers.TryGetValue(trigger, out var handler)) {
      trigger.Triggered -= handler;
      BackstageCardShakeSubscribePatch.Handlers.Remove(trigger);
    }
  }
}
