using System;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace RuriMegu.Core.Utils;

public static class Events {
  public record Event {
    public bool IsCancelled { get; private set; } = false;
    public void Cancel() => IsCancelled = true;
  }

  public record HeartsChangedEvent(
    Player Player,
    int OldHearts,
    int NewHearts,
    int MaxHearts,
    int Delta,
    CardModel Source
  ) : Event;

  public record MaxHeartsChangedEvent(
    Player Player,
    int OldMaxHearts,
    int NewMaxHearts,
    int Hearts,
    int Delta,
    CardModel Source
  ) : Event;

  public record BurstHeartsEvent(
    Player Player,
    int RequestedAmount,
    CardModel Source
  ) : Event {
    public int ActualAmount { get; set; } = 0;
  }

  public record CollectHeartsEvent(
    Player Player,
    CardModel Source
  ) : Event {
    public int Amount { get; set; } = 0;
    public IReadOnlyList<Creature> Targets { get; set; } = null;
  }

  public record IncreaseMaxHeartsEvent(
    Player Player,
    int RequestedAmount,
    CardModel Source
  ) : Event {
    public int ActualAmount { get; set; } = 0;
  }

  public record TriggerBackstageEvent(
    Player Player,
    CardModel Source
  ) : Event;

  public class PhasedEvent<TEvent> where TEvent : Event {
    public event Action<TEvent> VeryEarly;
    public event Action<TEvent> Early;
    public event Action<TEvent> Late;
    public event Action<TEvent> VeryLate;

    public Subscription SubscribeVeryEarly(Action<TEvent> handler) {
      VeryEarly += handler;
      return new Subscription(() => VeryEarly -= handler);
    }

    public Subscription SubscribeEarly(Action<TEvent> handler) {
      Early += handler;
      return new Subscription(() => Early -= handler);
    }

    public Subscription SubscribeLate(Action<TEvent> handler) {
      Late += handler;
      return new Subscription(() => Late -= handler);
    }

    public Subscription SubscribeVeryLate(Action<TEvent> handler) {
      VeryLate += handler;
      return new Subscription(() => VeryLate -= handler);
    }

    /// <summary>
    /// Returns true if the event was not cancelled.
    /// </summary>
    public bool InvokeVeryEarly(TEvent e) {
      VeryEarly?.Invoke(e);
      return !e.IsCancelled;
    }

    /// <summary>
    /// Returns true if the event was not cancelled.
    /// </summary>
    public bool InvokeEarly(TEvent e) {
      Early?.Invoke(e);
      return !e.IsCancelled;
    }

    /// <summary>
    /// Returns true if the event was not cancelled.
    /// </summary>
    public bool InvokeAllEarly(TEvent e) {
      if (!InvokeVeryEarly(e)) return false;
      if (!InvokeEarly(e)) return false;
      return true;
    }

    public void InvokeLate(TEvent e) => Late?.Invoke(e);

    public void InvokeVeryLate(TEvent e) => VeryLate?.Invoke(e);

    public void InvokeAllLate(TEvent e) {
      InvokeLate(e);
      InvokeVeryLate(e);
    }
  }

  public static readonly PhasedEvent<HeartsChangedEvent> HeartsChanged = new();
  public static readonly PhasedEvent<MaxHeartsChangedEvent> MaxHeartsChanged = new();
  public static readonly PhasedEvent<BurstHeartsEvent> BurstHearts = new();
  public static readonly PhasedEvent<CollectHeartsEvent> CollectHearts = new();
  public static readonly PhasedEvent<IncreaseMaxHeartsEvent> IncreaseMaxHearts = new();
  public static readonly PhasedEvent<TriggerBackstageEvent> TriggerBackstage = new();
}

public static class EventExtensions {
  public static bool IsNullOrCancelled<T>(this T ev) where T : Events.Event {
    return ev == null || ev.IsCancelled;
  }
}
