using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// Base class for Career Survey powers.
/// </summary>
public abstract class CareerSurveyPowerBase : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Counter;

  protected abstract int Threshold { get; }

  public override Task AfterApplied(Creature applier, CardModel cardSource) {
    DisposeTrackedSubscriptions();
    TrackSubscription(Events.Burst.SubscribeLate(OnBurstLate));
    return base.AfterApplied(applier, cardSource);
  }

  private async Task OnBurstLate(Events.BurstEvent ev) {
    if (ev.Player.Creature != Owner || ev.ActualAmount < Threshold) return;

    if (ev.HeartsChangedEvent.NewHearts < ev.HeartsChangedEvent.MaxHearts) {
      await CardPileCmd.Draw(ev.Context, (int)Amount, Owner.Player);
    }
  }
}

/// <summary>
/// Whenever you burst at least 8 hearts in a single instance and don't reach max hearts, draw X cards.
/// </summary>
public class CareerSurveyPower : CareerSurveyPowerBase {
  protected override int Threshold => 8;
}

/// <summary>
/// Whenever you burst at least 6 hearts in a single instance and don't reach max hearts, draw X cards.
/// </summary>
public class CareerSurveyUpgradedPower : CareerSurveyPowerBase {
  protected override int Threshold => 6;
}
