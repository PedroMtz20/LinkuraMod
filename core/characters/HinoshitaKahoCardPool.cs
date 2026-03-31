using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Cards.Kaho;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Characters;

/// <summary>
/// Card pool for Hinoshita Kaho-colored cards.
/// </summary>
public class HinoshitaKahoCardPool : CustomCardPoolModel {
  public override string Title => HinoshitaKaho.CharacterName;

  public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
  public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

  public override float H => 0.017f;
  public override float S => 1.0f;
  public override float V => 0.745f;

  public override Color DeckEntryCardColor => new("be1400");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards() {
    return [
      ModelDb.Card<KahoStrike>(),
      ModelDb.Card<KahoDefend>(),
      ModelDb.Card<WideHeart>(),
      ModelDb.Card<SpecialThanks>(),
    ];
  }
}
