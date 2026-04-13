using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Relics;

/// <summary>
/// Base class for all Linkura relics.
/// Provides subscription tracking: call TrackSubscription() to register a subscription;
/// all tracked subscriptions are disposed automatically at combat end and on removal.
/// </summary>
public abstract class LinkuraRelic : CustomRelicModel {
  public abstract string CharacterId { get; }
  public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath(CharacterId);
  protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath(CharacterId);
  protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath(CharacterId);

  private readonly List<Subscription> _subs = [];

  /// <summary>Track a subscription for automatic cleanup at combat end or relic removal.</summary>
  protected void TrackSubscription(Subscription sub) => _subs.Add(sub);

  private void DisposeAllSubscriptions() {
    foreach (var sub in _subs) sub.Dispose();
    _subs.Clear();
  }

  public override Task AfterCombatEnd(CombatRoom room) {
    DisposeAllSubscriptions();
    return base.AfterCombatEnd(room);
  }

  public override Task AfterRemoved() {
    DisposeAllSubscriptions();
    return base.AfterRemoved();
  }
}
