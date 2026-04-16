using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using RuriMegu.Core.Utils;
using System.Collections.Immutable;

namespace RuriMegu.Core.Cards.Kaho.Uncommon.Skill;

/// <summary>
/// Happy Supremacy! (快乐至上主义！) — Cost 0, Skill, Uncommon.
/// Convert all your ❤️ to Strength or Dexterity, randomly distributed to
/// all characters (base) or all players only (upgraded). Exhaust.
/// </summary>
public class HappySupremacy() : KahoCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.None) {
  public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
  public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    int hearts = HeartsState.GetHearts(Owner);
    if (hearts <= 0) return;

    var targets = (IsUpgraded
        ? Owner.Creature.CombatState.PlayerCreatures
        : Owner.Creature.CombatState.Creatures)
      .Where(c => c.IsAlive)
      .ToList();

    if (targets.Count == 0) return;

    var rng = Owner.RunState.Rng.CombatTargets;
    var strengths = new int[targets.Count];
    var dexterities = new int[targets.Count];
    for (int i = 0; i < hearts; i++) {
      var target = rng.NextInt(targets.Count);
      if (rng.NextBool()) {
        strengths[target]++;
      } else {
        dexterities[target]++;
      }
    }

    // Build targets list for visual effect distribution
    var collectTargets = Enumerable.Range(0, targets.Count)
      .Where(i => strengths[i] + dexterities[i] > 0)
      .Select(i => targets[i])
      .ToImmutableList();

    await Owner.PlayCollectAnim();
    var visualEv = new Events.CollectVisualEvent(Owner, collectTargets);
    await Events.CollectVisual.InvokeAll(visualEv);

    var tasks = new List<Task>();
    for (int i = 0; i < targets.Count; i++) {
      if (strengths[i] > 0) {
        tasks.Add(PowerCmd.Apply<StrengthPower>(targets[i], strengths[i], Owner.Creature, this));
      }
      if (dexterities[i] > 0) {
        tasks.Add(PowerCmd.Apply<DexterityPower>(targets[i], dexterities[i], Owner.Creature, this));
      }
    }
    await Task.WhenAll(tasks);

    await HeartsState.SetHearts(Owner, ctx, 0, this);
  }
}
