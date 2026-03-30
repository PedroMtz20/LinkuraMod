using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;

namespace RuriMegu.Core.Utils;

public class PlayerCombatData {
  public static readonly ConditionalWeakTable<Player, PlayerCombatData> Instance = [];
  public static PlayerCombatData Get(Player player) => Instance.GetOrCreateValue(player);

  public const int DEFAULT_MAX_HEARTS = 9;
  public const int MAX_MAX_HEARTS = 9999;

  private int _hearts = 0;
  public int Hearts {
    get => _hearts;
    set => _hearts = Math.Clamp(value, 0, MaxHearts);
  }

  private int _maxHearts = DEFAULT_MAX_HEARTS;
  public int MaxHearts {
    get => _maxHearts;
    set {
      _maxHearts = Math.Clamp(value, 0, MAX_MAX_HEARTS);
      Hearts = Math.Clamp(Hearts, 0, _maxHearts);
    }
  }
}
