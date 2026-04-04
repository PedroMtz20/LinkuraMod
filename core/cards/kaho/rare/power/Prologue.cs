using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RuriMegu.Core.Powers;

namespace RuriMegu.Core.Cards.Kaho.Rare.Power;

/// <summary>
/// Prologue (序章) — Cost 2, Power, Rare, (Innate.)
/// The first time Max ❤️ changes each turn, your next card costs {Energy:energyIcons()} less.
/// </summary>
public class Prologue() : LinkuraCard(2, CardType.Power, CardRarity.Rare, TargetType.None) {
  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new EnergyVar(1),
  ];

  protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play) {
    await PowerCmd.Apply<ProloguePower>(Owner.Creature, 1, Owner.Creature, this);
  }

  protected override void OnUpgrade() {
    AddKeyword(CardKeyword.Innate);
  }
}
