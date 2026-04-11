using System.Threading.Tasks;
using Godot;

namespace RuriMegu.Nodes.Combat;

/// <summary>
/// A single floating heart particle spawned when Kaho gains hearts.
/// Pop-up jump → slow exponential drift downward → persists until collected.
/// Call <see cref="Collect"/> to fly toward a target and shrink to zero,
/// or <see cref="Dismiss"/> to fade out when there are no targets.
/// </summary>
public partial class FloatingHeart : Control {
  // ── Set before adding to the scene tree ──────────────────────
  public Texture2D HeartTexture { get; set; } = null!;
  public Shader GlowShader { get; set; } = null!;
  public float HeartScale { get; set; } = 1.0f;
  public float GlowIntensity { get; set; } = 1.9f;
  public float GlowBaseAlpha { get; set; } = 0.8f;
  public float CollectDuration { get; set; } = 0.4f;

  private Tween _settleTween;
  private bool _collecting;

  public override void _Ready() {
    var texSize = (Vector2)HeartTexture.GetSize();
    var size = texSize * HeartScale;
    Size = size;
    PivotOffset = size / 2f;
    MouseFilter = MouseFilterEnum.Ignore;

    var mat = new ShaderMaterial { Shader = GlowShader };
    mat.SetShaderParameter("glow_high", GlowIntensity);
    mat.SetShaderParameter("base_alpha", GlowBaseAlpha);
    AddChild(new TextureRect {
      Texture = HeartTexture,
      ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
      Size = size,
      MouseFilter = MouseFilterEnum.Ignore,
      Material = mat,
    });

    // If Collect() was called before _Ready (e.g. burst+collect on same card),
    // the collect tween is already running — skip spawn animation.
    if (_collecting) return;

    Modulate = Colors.Transparent;
    float startY = Position.Y;
    float dropDist = (float)GD.RandRange(60.0, 150.0);

    _settleTween = CreateTween();
    _settleTween.TweenProperty(this, "modulate:a", 1.0f, 0.2f);
    _settleTween.Parallel()
      .TweenProperty(this, "position:y", startY - 25f, 0.12f)
      .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
    _settleTween
      .TweenProperty(this, "position:y", startY + dropDist, 8.0f)
      .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
  }

  /// <summary>Fly toward <paramref name="targetPos"/> (screen space), shrink to zero, then free.</summary>
  public Task Collect(Vector2 targetPos) {
    if (_collecting) return Task.CompletedTask;
    _collecting = true;
    _settleTween?.Kill();
    Modulate = Colors.White; // ensure visible even if spawn fade-in hadn't completed

    var tcs = new TaskCompletionSource();
    var tween = CreateTween();
    tween.TweenProperty(this, "position", targetPos - Size / 2f, CollectDuration)
      .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    tween.Parallel()
      .TweenProperty(this, "scale", Vector2.Zero, CollectDuration)
      .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
    tween.TweenCallback(Callable.From(() => { QueueFree(); tcs.SetResult(); }));
    return tcs.Task;
  }

  /// <summary>Fade out and free (used when collection has no targets).</summary>
  public void Dismiss() {
    if (_collecting) return;
    _collecting = true;
    _settleTween?.Kill();

    var tween = CreateTween();
    tween.TweenProperty(this, "modulate:a", 0.0f, 0.5f);
    tween.TweenCallback(Callable.From(QueueFree));
  }
}
