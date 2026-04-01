using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards.Kaho;

/// <summary>
/// Fan Service (饭撒) — Cost 1 (0), Skill, Common.
/// Collect. The damage from Collect also triggers on 1 additional random enemy.
/// </summary>
public class FanService() : LinkuraCard(1, CardType.Skill, CardRarity.Common, TargetType.None) {
  public override IEnumerable<CardKeyword> CanonicalKeywords => [LinkuraKeywords.Collect];

  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    // Standard Collect: deals hearts as damage to 1 random enemy, then resets ♥ to 0.
    // Fan Service hits 1 additional random enemy, so we trigger the damage 2 times.
    await LinkuraCardActions.CollectHearts(this, ctx, null, triggers: 2);
  }

  protected override void OnUpgrade() {
    EnergyCost.UpgradeBy(-1);
  }
}
