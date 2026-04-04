using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// Base class for May Dreams Bloom powers.
/// </summary>
public abstract class MayDreamsBloomPowerBase : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Counter;

  protected abstract int Threshold { get; }

  private int _accumulatedOverflow;

  public override Task AfterApplied(Creature applier, CardModel cardSource) {
    DisposeTrackedSubscriptions();
    TrackSubscription(Events.Burst.SubscribeLate(OnBurstLate));
    _accumulatedOverflow = 0;
    return base.AfterApplied(applier, cardSource);
  }

  private async Task OnBurstLate(Events.BurstEvent ev) {
    if (ev.Player.Creature != Owner) return;
    int overflow = ev.RequestedAmount - ev.ActualAmount;
    if (overflow <= 0) return;

    _accumulatedOverflow += overflow;
    while (_accumulatedOverflow >= Threshold) {
      _accumulatedOverflow -= Threshold;
      await LinkuraCmd.GainAutoBurst(Owner, Amount, Owner, null);
    }
  }
}

/// <summary>
/// For every 20 ❤️ overflowed, gain 1 stack of Auto Burst.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Rare.Power.MayDreamsBloom"/> (base version).
/// </summary>
public class MayDreamsBloomPower : MayDreamsBloomPowerBase {
  protected override int Threshold => 20;
}

/// <summary>
/// For every 15 ❤️ overflowed, gain 1 stack of Auto Burst.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Rare.Power.MayDreamsBloom"/> (upgraded version).
/// </summary>
public class MayDreamsBloomUpgradedPower : MayDreamsBloomPowerBase {
  protected override int Threshold => 15;
}
