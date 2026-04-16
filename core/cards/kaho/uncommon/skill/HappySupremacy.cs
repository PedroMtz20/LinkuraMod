using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards.Kaho.Uncommon.Skill;

/// <summary>
/// Happy Supremacy! (快乐至上主义！) — Cost 0, Skill, Uncommon.
/// Convert all your ❤️ to Strength or Dexterity, randomly distributed to
/// all characters (base) or all players only (upgraded). Exhaust.
/// </summary>
public class HappySupremacy() : KahoCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.None) {
  public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    int hearts = HeartsState.GetHearts(Owner);
    if (hearts <= 0) return;

    await Owner.PlayCollectAnim();
    await HeartsState.SetHearts(Owner, ctx, 0, this);

    var targets = (IsUpgraded
        ? Owner.CombatState.PlayerCreatures
        : Owner.CombatState.Creatures)
      .Where(c => c.IsAlive)
      .ToList();

    if (targets.Count == 0) return;

    var rng = Owner.RunState.Rng.CombatTargets;
    for (int i = 0; i < hearts; i++) {
      var target = rng.NextItem(targets);
      if (rng.NextBool()) {
        await PowerCmd.Apply<StrengthPower>(target, 1, Owner.Creature, this);
      } else {
        await PowerCmd.Apply<DexterityPower>(target, 1, Owner.Creature, this);
      }
    }
  }
}
