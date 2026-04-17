using MegaCrit.Sts2.Core.Entities.Powers;

namespace RuriMegu.Core.Powers.Kaho;

/// <summary>
/// Whenever you Collect Hearts, deal damage to all enemies.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Uncommon.Power.SpecialAppeal"/>.
/// </summary>
public class SpecialAppealPower : KahoPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Single;
}
