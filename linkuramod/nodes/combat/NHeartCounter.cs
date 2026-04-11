using System;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using RuriMegu.Core.Utils;

namespace RuriMegu.Nodes.Combat;

/// <summary>
/// Displays Hinoshita Kaho's "Hearts" counter — a secondary resource
/// that mirrors the Regent's star counter in layout, but with Kaho's
/// golden-pink Love Live theme.
/// </summary>
public partial class NHeartCounter : Control {
  private Player _player;
  private RichTextLabel _label = null!;
  private TextureRect _layer1 = null!;
  private Control _fillClip = null!;
  private TextureRect _layer2 = null!;
  private IDisposable _heartsChangedSubscription;
  private IDisposable _maxHeartsChangedSubscription;

  // Smooth-damp state for the animated label
  private int _targetHearts;
  private int _targetMaxHearts;
  private float _lerpedHearts;
  private float _lerpedMaxHearts;
  private float _heartsVelocity;
  private float _maxHeartsVelocity;

  // ──────────────────────────────────────────────────────────────
  // Godot lifecycle
  // ──────────────────────────────────────────────────────────────

  public override void _Ready() {
    _label = GetNode<RichTextLabel>("%HeartLabel");
    _layer1 = GetNode<TextureRect>("Icon/Layer1");
    _fillClip = GetNode<Control>("Icon/Layer1/FillClip");
    _layer2 = GetNode<TextureRect>("Icon/Layer1/FillClip/Layer2");
    Visible = false;
  }

  public override void _ExitTree() {
    _heartsChangedSubscription?.Dispose();
    _heartsChangedSubscription = null;

    _maxHeartsChangedSubscription?.Dispose();
    _maxHeartsChangedSubscription = null;
  }

  public override void _Process(double delta) {
    if (_player is null) return;

    _lerpedHearts = MathHelper.SmoothDamp(
      _lerpedHearts, _targetHearts, ref _heartsVelocity, 0.1f, (float)delta);
    _lerpedMaxHearts = MathHelper.SmoothDamp(
      _lerpedMaxHearts, _targetMaxHearts, ref _maxHeartsVelocity, 0.1f, (float)delta);
    UpdateFill(_lerpedHearts, _lerpedMaxHearts);
    OnHeartsChanged(Mathf.RoundToInt(_lerpedHearts), Mathf.RoundToInt(_lerpedMaxHearts));
  }

  // ──────────────────────────────────────────────────────────────
  // Public API
  // ──────────────────────────────────────────────────────────────

  public void Initialize(Player player) {
    _heartsChangedSubscription?.Dispose();
    _maxHeartsChangedSubscription?.Dispose();

    _player = player;
    _targetHearts = HeartsState.GetHearts(player);
    _targetMaxHearts = HeartsState.GetMaxHearts(player);
    _lerpedHearts = _targetHearts;
    _lerpedMaxHearts = _targetMaxHearts;
    _heartsVelocity = 0f;
    _maxHeartsVelocity = 0f;

    _heartsChangedSubscription = Events.HeartsChanged.SubscribeLate(OnHeartsStateChanged);
    _maxHeartsChangedSubscription = Events.MaxHeartsChanged.SubscribeLate(OnMaxHeartsStateChanged);

    UpdateFill(_lerpedHearts, _lerpedMaxHearts);
    OnHeartsChanged(_targetHearts, _targetMaxHearts);
    RefreshVisibility();
  }

  // ──────────────────────────────────────────────────────────────
  // Helpers
  // ──────────────────────────────────────────────────────────────

  private Task OnHeartsStateChanged(Events.HeartsChangedEvent evt) {
    if (_player is null || evt.Player != _player) {
      return Task.CompletedTask;
    }

    _targetHearts = evt.NewHearts;
    _targetMaxHearts = evt.MaxHearts;
    return Task.CompletedTask;
  }

  private Task OnMaxHeartsStateChanged(Events.MaxHeartsChangedEvent evt) {
    if (_player is null || evt.Player != _player) {
      return Task.CompletedTask;
    }

    _targetMaxHearts = evt.NewMaxHearts;
    _targetHearts = evt.Hearts;
    return Task.CompletedTask;
  }

  private void OnHeartsChanged(int newHearts, int newMaxHearts) {
    SetLabelText($"{newHearts}/{newMaxHearts}");
    RefreshVisibility();
  }

  private void UpdateFill(float hearts, float maxHearts) {
    var size = _layer1.Size;
    if (size.Y <= 0f) return;
    float fraction = maxHearts > 0f ? Mathf.Clamp(hearts / maxHearts, 0f, 1f) : 0f;
    float fillHeight = size.Y * fraction;
    float emptyHeight = size.Y - fillHeight;
    _fillClip.Position = new Vector2(0f, emptyHeight);
    _fillClip.Size = new Vector2(size.X, fillHeight);
    _layer2.Position = new Vector2(0f, -emptyHeight);
    _layer2.Size = size;
  }

  private void SetLabelText(string text) {
    if (_label.Text == text) return;
    _label.Text = text;
  }

  private void RefreshVisibility() {
    if (_player is null) { Visible = false; return; }
    Visible = true;
  }
}
