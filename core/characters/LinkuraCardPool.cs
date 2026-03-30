using BaseLib.Abstracts;
using Godot;
using RuriMegu.Core.Extensions;

namespace RuriMegu.Core.Characters;

/// <summary>
/// Card pool for Linkura-colored cards.
/// </summary>
public class LinkuraCardPool : CustomCardPoolModel {
  public override string Title => HinoshitaKaho.CharacterName;

  public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
  public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

  public override float H => 0.017f;
  public override float S => 1.0f;
  public override float V => 0.745f;

  public override Color DeckEntryCardColor => new("be1400");

  public override bool IsColorless => false;
}
