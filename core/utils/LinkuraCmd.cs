using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace RuriMegu.Core.Utils;

public static class LinkuraCmd {
  public static async Task IncreaseMaxHearts(Player player, int amount, CardModel source = null) {
    var ev = new Events.IncreaseMaxHeartsEvent(player, amount, source);
    if (!Events.IncreaseMaxHearts.InvokeAllEarly(ev)) return;
    var childEv = await HeartsState.AddMaxHearts(player, amount, source);
    if (childEv.IsNullOrCancelled()) return;
    ev.ActualAmount = childEv.NewMaxHearts - childEv.OldMaxHearts;
    Events.IncreaseMaxHearts.InvokeAllLate(ev);
  }

  public static async Task BurstHearts(Player player, int amount, CardModel source = null) {
    if (amount <= 0) return;
    var ev = new Events.BurstHeartsEvent(player, amount, source);
    if (!Events.BurstHearts.InvokeAllEarly(ev)) return;
    var childEv = await HeartsState.AddHearts(player, amount, source);
    if (childEv.IsNullOrCancelled()) return;
    ev.ActualAmount = childEv.NewHearts - childEv.OldHearts;
    Events.BurstHearts.InvokeAllLate(ev);
  }

  public static async Task CollectHearts(Player player, PlayerChoiceContext context, CardModel source = null, Creature target = null) {
    var ev = new Events.CollectHeartsEvent(player, source);
    if (!Events.CollectHearts.InvokeAllEarly(ev)) return;
    int hearts = HeartsState.GetHearts(player);
    IReadOnlyList<Creature> targets = await ApplyHeartDamage(hearts, target, player, context);
    var childEv = await HeartsState.SetHearts(player, 0, source);
    if (childEv.IsNullOrCancelled()) return;
    ev.Amount = hearts;
    ev.Targets = targets;
    Events.CollectHearts.InvokeAllLate(ev);
  }

  private static async Task<IReadOnlyList<Creature>> ApplyHeartDamage(int value, Creature target, Player player, PlayerChoiceContext choiceContext) {
    List<Creature> list = [.. from e in player.Creature.CombatState.GetOpponentsOf(player.Creature)
                           where e.IsHittable
                           select e];
    if (list.Count == 0) {
      return [];
    }
    IReadOnlyList<Creature> targets = (target == null) ? [player.RunState.Rng.CombatTargets.NextItem(list)] : [target];
    await CreatureCmd.Damage(choiceContext, targets, value, ValueProp.Unpowered, player.Creature);
    return targets;
  }
}
