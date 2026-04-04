using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers;

/// <summary>
/// Whenever you Burst, Collect. All enemies (including newly appearing ones) gain 99 Intangible.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Rare.Power.AutoClickerOn"/>.
/// </summary>
public class AutoClickerOnPower : LinkuraPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Single;

  private Subscription _sub;

  public override async Task AfterApplied(Creature applier, CardModel cardSource) {
    _sub?.Dispose();
    _sub = Events.Burst.SubscribeLate(OnBurstLate);

    if (Owner.CombatState != null) {
      foreach (var enemy in Owner.CombatState.GetOpponentsOf(Owner).Where(e => e.IsAlive).ToList()) {
        await PowerCmd.Apply<IntangiblePower>(enemy, 99, Owner, cardSource);
      }
    }

    await base.AfterApplied(applier, cardSource);
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

  public override async Task AfterCreatureAddedToCombat(Creature creature) {
    if (creature.Side == Owner.Side) return;
    await PowerCmd.Apply<IntangiblePower>(creature, 99, Owner, null);
  }

  private async Task OnBurstLate(Events.BurstEvent ev) {
    if (ev.Player.Creature != Owner) return;
    await LinkuraCmd.CollectHearts(ev.Player, ev.Context);
  }
}
