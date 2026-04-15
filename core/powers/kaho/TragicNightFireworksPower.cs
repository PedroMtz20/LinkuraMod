using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Powers.Kaho;

/// <summary>
/// When you increase Max ❤️, raise your Max HP by stacks instead.
/// Applied by <see cref="RuriMegu.Core.Cards.Kaho.Ancient.TragicNightFireworks"/>.
/// </summary>
public class TragicNightFireworksPower : KahoPower {
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterApplied(Creature applier, CardModel cardSource) {
    DisposeTrackedSubscriptions();
    TrackSubscription(Events.IncreaseMaxHearts.SubscribeEarly(OnIncreaseMaxHeartsEarly));
    await base.AfterApplied(applier, cardSource);
  }

  private async Task OnIncreaseMaxHeartsEarly(Events.IncreaseMaxHeartsEvent ev) {
    if (ev.Player != Owner.Player) return;
    ev.Cancel();
    Flash();
    await CreatureCmd.GainMaxHp(Owner, Amount);
  }
}
