using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using RuriMegu.Core.Utils;

namespace RuriMegu.Core.Nodes.Combat;

/// <summary>
/// Displays Hinoshita Kaho's "Hearts" counter — a secondary resource
/// that mirrors the Regent's star counter in layout, but with Kaho's
/// golden-pink Love Live theme. Backed by <see cref="PlayerCombatState.Stars"/>.
/// </summary>
public partial class NHeartCounter : Control {
  private PlayerCombatData _playerData;
  private RichTextLabel _label = null!;

  // Smooth-damp state for the animated label
  private float _lerpedHearts;
  private float _lerpedMaxHearts;
  private float _velocity;

  // ──────────────────────────────────────────────────────────────
  // Godot lifecycle
  // ──────────────────────────────────────────────────────────────

  public override void _Ready() {
    _label = GetNode<RichTextLabel>("%HeartLabel");
    Visible = false;
  }

  public override void _Process(double delta) {
    if (_playerData is null) return;

    _lerpedHearts = MathHelper.SmoothDamp(
      _lerpedHearts, _playerData.Hearts, ref _velocity, 0.1f, (float)delta);
    _lerpedMaxHearts = MathHelper.SmoothDamp(
      _lerpedMaxHearts, _playerData.MaxHearts, ref _velocity, 0.1f, (float)delta);
    OnHeartsChanged(Mathf.RoundToInt(_lerpedHearts), Mathf.RoundToInt(_lerpedMaxHearts));
  }

  // ──────────────────────────────────────────────────────────────
  // Public API
  // ──────────────────────────────────────────────────────────────

  public void Initialize(Player player) {
    _playerData = PlayerCombatData.Get(player);
    RefreshVisibility();
  }

  // ──────────────────────────────────────────────────────────────
  // Helpers
  // ──────────────────────────────────────────────────────────────

  private void OnHeartsChanged(int newHearts, int newMaxHearts) {
    SetLabelText($"{newHearts}/{newMaxHearts}");
    RefreshVisibility();
  }

  private void SetLabelText(string text) {
    if (_label.Text == text) return;
    _label.Text = text;
  }

  private void RefreshVisibility() {
    if (_playerData is null) { Visible = false; return; }
    Visible = true;
  }
}
