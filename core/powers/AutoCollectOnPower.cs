using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// Whenever your ❤️ hits maximum, Collect.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Uncommon.Power.AutoCollectOn"/>.
/// </summary>
public class AutoCollectOnPower : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Single;

  public override Task AfterApplied(Creature applier, CardModel cardSource) {
    DisposeTrackedSubscriptions();
    TrackSubscription(Events.HeartsChanged.SubscribeLate(OnHeartsChangedLate));
    return base.AfterApplied(applier, cardSource);
  }

  private async Task OnHeartsChangedLate(Events.HeartsChangedEvent ev) {
    if (ev.Player.Creature != Owner) return;

    if (ev.NewHearts >= ev.MaxHearts) {
      await LinkuraCmd.CollectHearts(ev.Player, ev.Context);
    }
  }
}
