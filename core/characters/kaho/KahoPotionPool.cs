using BaseLib.Abstracts;
using Godot;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Characters.Kaho;

/// <summary>
/// Potion pool for Linkura potions.
/// </summary>
public class KahoPotionPool : CustomPotionPoolModel {
  public override Color LabOutlineColor => HinoshitaKaho.Color;

  public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
  public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
