using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards;

public static class LinkuraCardActions {
  public static Task IncreaseMaxHearts(CardModel card) {
    var data = PlayerCombatData.Get(card.Owner);
    data.MaxHearts += card.DynamicVars.ExpandHearts().IntValue;
    return Task.CompletedTask;
  }

  public static Task BurstHearts(CardModel card) {
    var data = PlayerCombatData.Get(card.Owner);
    data.Hearts += card.DynamicVars.BurstHearts().IntValue;
    return Task.CompletedTask;
  }

  public static async Task CollectHearts(CardModel card, PlayerChoiceContext context, Creature target = null) {
    var data = PlayerCombatData.Get(card.Owner);
    await ApplyHeartDamage(data.Hearts, target, card.Owner, context);
    data.Hearts = 0;
  }

  private static async Task<IEnumerable<Creature>> ApplyHeartDamage(decimal value, Creature target, Player player, PlayerChoiceContext choiceContext) {
    List<Creature> list = [.. (from e in player.Creature.CombatState.GetOpponentsOf(player.Creature)
                           where e.IsHittable
                           select e)];
    if (list.Count == 0) {
      return [];
    }
    IReadOnlyList<Creature> targets = ((target == null) ? [player.RunState.Rng.CombatTargets.NextItem(list)] : [target]);
    await CreatureCmd.Damage(choiceContext, targets, value, ValueProp.Unpowered, player.Creature);
    return targets;
  }
}
