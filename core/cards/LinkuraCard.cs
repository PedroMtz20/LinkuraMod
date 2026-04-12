using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using RuriMegu.Core.Characters.Kaho;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Cards;

/// <summary>
/// Base class for all Linkura-pool cards.
/// Provides subscription tracking; lifecycle (init + cleanup) is managed by LinkuraSystem.
/// </summary>
public abstract class LinkuraCard(int cost, CardType type, CardRarity rarity, TargetType target)
  : CustomCardModel(cost, type, rarity, target) {
  public virtual string CharacterId => "";
  public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath(CharacterId);
  public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath(CharacterId);
  public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath(CharacterId);

  private readonly List<Subscription> _subs = [];

  /// <summary>
  /// Override to set up subscriptions via TrackSubscription().
  /// Called by LinkuraSystem at BeforeCombatStartLate for startup cards,
  /// and via AfterCardEnteredCombat for mid-combat additions.
  /// </summary>
  protected virtual Task InitializeSubscriptions() => Task.CompletedTask;
  internal Task RunInitializeSubscriptions() => InitializeSubscriptions();

  /// <summary>Track a subscription for automatic cleanup.</summary>
  protected void TrackSubscription(Subscription sub) => _subs.Add(sub);

  /// <summary>Dispose all tracked subscriptions. Called by LinkuraSystem.</summary>
  internal void DisposeTrackedSubscriptions() {
    foreach (var sub in _subs) sub.Dispose();
    _subs.Clear();
  }
}
