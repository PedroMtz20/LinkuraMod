using BaseLib.Abstracts;
using Godot;
using RuriMegu.Core.Extensions;

namespace RuriMegu.Core.Characters;

/// <summary>
/// Potion pool for Linkura potions.
/// </summary>
public class LinkuraPotionPool : CustomPotionPoolModel {
  public override Color LabOutlineColor => HinoshitaKaho.Color;

  public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
  public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
