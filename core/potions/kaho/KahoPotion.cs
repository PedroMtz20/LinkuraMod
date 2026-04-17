using BaseLib.Utils;
using RuriMegu.Core.Characters.Kaho;

namespace RuriMegu.Core.Potions.Kaho;

/// <summary>
/// Base class for all Kaho potions.
/// Inheriting from this automatically places potions in the Kaho potion pool.
/// </summary>
[Pool(typeof(KahoPotionPool))]
public abstract class KahoPotion : LinkuraPotion {
  public override string CharacterId => HinoshitaKaho.CHARACTER_ID;
}
