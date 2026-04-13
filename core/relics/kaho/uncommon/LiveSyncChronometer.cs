using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using RuriMegu.Core.Cards;
using RuriMegu.Core.Powers;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Relics.Kaho.Uncommon;

/// <summary>
/// Live-Sync Chronometer — Uncommon relic for Hinoshita Kaho.
/// During your turn, trigger Auto Burst once per real-time minute.
/// Note: only triggered during player's turn to avoid bugs during enemy turn.
/// </summary>
public class LiveSyncChronometer : KahoRelic {
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  private CancellationTokenSource _cts;
  private int _secondCount;

  protected override IEnumerable<IHoverTip> ExtraHoverTips => [
    HoverTipFactory.FromPower<AutoBurstPower>(),
    BurstHeartsVar.HoverTip(),
  ];

  public override bool ShowCounter => true;
  public override int DisplayAmount => _secondCount;

  public override Task BeforeCombatStart() {
    _secondCount = 0;
    return Task.CompletedTask;
  }

  public override Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player) {
    if (player != Owner) return Task.CompletedTask;
    CancelLoop();
    _cts = new CancellationTokenSource();
    _ = RunMinuteLoop(ctx, _cts.Token);
    return Task.CompletedTask;
  }

  private async Task RunMinuteLoop(PlayerChoiceContext ctx, CancellationToken ct) {
    try {
      while (!ct.IsCancellationRequested) {
        await Cmd.Wait(1f, ct);
        if (ct.IsCancellationRequested) break;
        _secondCount++;
        if (_secondCount >= 60) {
          _secondCount = 0;
          if (Owner.Creature.CombatState?.CurrentSide == CombatSide.Player) {
            Flash();
            await LinkuraCmd.TriggerAutoBurst(Owner, ctx);
          }
        }
      }
    } catch (OperationCanceledException) {
      // Expected when turn ends or combat ends.
    }
  }

  public override Task BeforeTurnEnd(PlayerChoiceContext ctx, CombatSide side) {
    if (side == CombatSide.Player) CancelLoop();
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom room) {
    CancelLoop();
    return base.AfterCombatEnd(room);
  }

  public override Task AfterRemoved() {
    CancelLoop();
    return base.AfterRemoved();
  }

  private void CancelLoop() {
    _cts?.Cancel();
    _cts?.Dispose();
    _cts = null;
  }
}
