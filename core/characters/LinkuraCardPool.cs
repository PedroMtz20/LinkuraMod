using BaseLib.Abstracts;
using Godot;
using RuriMegu.Core.Extensions;

namespace RuriMegu.Core.Characters;

/// <summary>
/// Card pool for Linkura-colored cards.
/// </summary>
public class LinkuraCardPool : CustomCardPoolModel {
  public override string Title => HinoshitaKaho.CharacterId;

  public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
  public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

  public override float H => 0.94f;
  public override float S => 0.65f;
  public override float V => 0.91f;

  public override Color DeckEntryCardColor => new("e8789a");

  public override bool IsColorless => false;
}
