using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace RuriMegu.Core.Relics.Kaho.Shop;

/// <summary>
/// Cap Liberator — Shop relic for Hinoshita Kaho.
/// On pickup: for every pair of identical unupgraded cards in your deck,
/// remove both and add one upgraded copy.
/// </summary>
public class CapLiberator : KahoRelic {
  public override RelicRarity Rarity => RelicRarity.Shop;

  public override bool HasUponPickupEffect => true;

  public override async Task AfterObtained() {
    var deckCards = PileType.Deck.GetPile(Owner).Cards
      .Where(c => !c.IsUpgraded && c.IsUpgradable)
      .ToList();

    var groups = deckCards
      .GroupBy(c => c.Id)
      .Where(g => g.Count() >= 2)
      .ToList();

    foreach (var group in groups) {
      int pairs = group.Count() / 2;
      var toRemove = group.Take(pairs * 2).ToList();
      await CardPileCmd.RemoveFromDeck(toRemove);
      for (int i = 0; i < pairs; i++) {
        CardModel upgraded = ModelDb.GetById<CardModel>(group.Key).ToMutable();
        CardCmd.Upgrade(upgraded, CardPreviewStyle.None);
        await CardPileCmd.Add(upgraded, PileType.Deck, CardPilePosition.Bottom, this);
      }
    }
  }
}
