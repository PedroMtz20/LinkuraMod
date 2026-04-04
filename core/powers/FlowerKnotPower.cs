using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// Whenever you trigger a Backstage effect, trigger Auto Burst once.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Rare.Power.FlowerKnot"/>.
/// </summary>
public class FlowerKnotPower : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Counter;

  private Subscription _sub;

  public override Task AfterApplied(Creature applier, CardModel cardSource) {
    _sub?.Dispose();
    _sub = Events.TriggerBackstage.SubscribeLate(OnTriggerBackstageLate);
    return base.AfterApplied(applier, cardSource);
  }

  public override Task AfterRemoved(Creature oldOwner) {
    _sub?.Dispose();
    _sub = null;
    return base.AfterRemoved(oldOwner);
  }

  public override Task AfterCombatEnd(MegaCrit.Sts2.Core.Rooms.CombatRoom room) {
    _sub?.Dispose();
    _sub = null;
    return base.AfterCombatEnd(room);
  }

  private async Task OnTriggerBackstageLate(Events.TriggerBackstageEvent ev) {
    if (ev.Player.Creature != Owner) return;
    for (int i = 0; i < Amount; i++) {
      await LinkuraCmd.TriggerAutoBurst(ev.Player, ev.Context, ev.Source);
    }
  }
}
