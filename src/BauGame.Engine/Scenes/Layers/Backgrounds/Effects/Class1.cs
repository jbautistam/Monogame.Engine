using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects
{
	internal class Class1
	{
	}
}
public class FadeOutCommand : BackgroundCommand
{
    private readonly Color _targetColor;
    private readonly bool _toBlack;

    /// <summary>
    /// Desvanece el fondo a negro.
    /// </summary>
    public FadeOutCommand(float duration, bool autoRewind = false)
        : this(Color.Black, duration, autoRewind) { }

    /// <summary>
    /// Desvanece el fondo a un color específico.
    /// </summary>
    public FadeOutCommand(Color targetColor, float duration, bool autoRewind = false)
    {
        _targetColor = targetColor;
        _toBlack = (targetColor == Color.Black);
        Duration = duration;
        AutoRewind = autoRewind;
        OverrideMask = BackgroundStateMask.Tint;
    }

    protected override void OnUpdate(GameTime gameTime) { }

    public override BackgroundState GetCurrentState()
    {
        // Tiempo normalizado: 0 = inicio (sin fade), 1 = final (color total)
        float t = AutoRewind && Duration > 0
            ? (ElapsedTime % Duration) / Duration
            : MathHelper.Clamp(ElapsedTime / Duration, 0f, 1f);

        // Interpolamos desde blanco (sin efecto) hacia el color objetivo
        // Pero como usamos multiplicación, mejor: Color.White → _targetColor
        Color currentTint = Color.Lerp(Color.White, _targetColor, t);

        return new BackgroundState { Tint = currentTint };
    }
}

public class ZoomToViewCommand : BackgroundCommand
{
    private readonly Vector2 _startViewCenter;
    private readonly Vector2 _targetViewCenter;
    private readonly float _startZoom;
    private readonly float _targetZoom;

    public ZoomToViewCommand(
        Vector2 startViewCenter,
        Vector2 targetViewCenter,
        float startZoom,
        float targetZoom,
        float duration,
        bool autoRewind = false)
    {
        _startViewCenter = startViewCenter;
        _targetViewCenter = targetViewCenter;
        _startZoom = startZoom;
        _targetZoom = targetZoom;
        Duration = duration;
        AutoRewind = autoRewind;
    }

    protected override void OnUpdate(GameTime gameTime) { }

    public override BackgroundState GetCurrentState()
    {
        float t = AutoRewind && Duration > 0
            ? (ElapsedTime % Duration) / Duration
            : MathHelper.Clamp(ElapsedTime / Duration, 0f, 1f);

        Vector2 currentCenter = Vector2.Lerp(_startViewCenter, _targetViewCenter, t);
        float currentZoom = MathHelper.Lerp(_startZoom, _targetZoom, t);

        return new BackgroundState
        {
            ViewCenter = currentCenter - _startViewCenter, // delta
            Zoom = currentZoom / _startZoom                 // factor relativo
        };
    }
}

public class TurbulenceCommand : BackgroundCommand
{
    private readonly float _amplitudeX;
    private readonly float _amplitudeY;
    private readonly Random _random = new Random();

    // Frecuencias y fases internas
    private readonly (float freq, float phase)[] _waves;

    public TurbulenceCommand(float amplitudeX, float amplitudeY, int waveCount = 3, float duration = 1f, bool autoRewind = true)
    {
        _amplitudeX = amplitudeX;
        _amplitudeY = amplitudeY;
        Duration = duration;
        AutoRewind = autoRewind;

        _waves = new (float, float)[waveCount];
        for (int i = 0; i < waveCount; i++)
        {
            float freq = 0.5f + (float)_random.NextDouble() * 3f; // 0.5–3.5 Hz
            float phase = (float)(_random.NextDouble() * Math.PI * 2);
            _waves[i] = (freq, phase);
        }
    }

    protected override void OnUpdate(GameTime gameTime) { }

    public override BackgroundState GetCurrentState()
    {
        float totalX = 0f, totalY = 0f;
        foreach (var (freq, phase) in _waves)
        {
            float angle = ElapsedTime * freq * MathHelper.TwoPi + phase;
            totalX += (float)Math.Sin(angle);
            totalY += (float)Math.Cos(angle + 1.3f);
        }

        // Normalizar y escalar
        float x = totalX * (_amplitudeX / _waves.Length);
        float y = totalY * (_amplitudeY / _waves.Length);

        return new BackgroundState
        {
            ScreenOffset = new Vector2(x, y),
            Zoom = 1f,
            ViewCenter = Vector2.Zero,
            Rotation = 0f,
            Tint = Color.White
        };
    }
}

public class ColorCycleCommand : BackgroundCommand
{
    private readonly Color[] _colors;
    private readonly bool _smooth;

    public ColorCycleCommand(Color[] colors, float duration, bool smooth = true, bool autoRewind = true)
    {
        _colors = colors ?? new[] { Color.White };
        if (_colors.Length == 0) _colors = new[] { Color.White };
        _smooth = smooth;
        Duration = duration;
        AutoRewind = autoRewind;
        OverrideMask = BackgroundStateMask.Tint;
    }

    protected override void OnUpdate(GameTime gameTime) { }

    public override BackgroundState GetCurrentState()
    {
        if (_colors.Length == 1)
            return new BackgroundState { Tint = _colors[0] };

        float cycleTime = AutoRewind && Duration > 0 ? Duration : float.MaxValue;
        float t = ElapsedTime % cycleTime;
        float step = cycleTime / _colors.Length;
        int index = (int)(t / step) % _colors.Length;
        Color a = _colors[index];
        Color b = _colors[(index + 1) % _colors.Length];

        if (_smooth && step > 0)
        {
            float localT = (t % step) / step;
            return new BackgroundState { Tint = Color.Lerp(a, b, localT) };
        }
        else
        {
            return new BackgroundState { Tint = a };
        }
    }
}

public class SpiralCommand : BackgroundCommand
{
    private readonly float _radiusSpeed;   // píxeles/segundo
    private readonly float _rotationSpeed; // radianes/segundo
    private readonly Vector2 _centerOffset; // centro de la espiral en pantalla

    public SpiralCommand(
        float radiusSpeed,
        float rotationSpeed,
        Vector2 centerOffset,
        float duration,
        bool autoRewind = false)
    {
        _radiusSpeed = radiusSpeed;
        _rotationSpeed = rotationSpeed;
        _centerOffset = centerOffset;
        Duration = duration;
        AutoRewind = autoRewind;
    }

    protected override void OnUpdate(GameTime gameTime) { }

    public override BackgroundState GetCurrentState()
    {
        float t = AutoRewind && Duration > 0
            ? (ElapsedTime % Duration) / Duration
            : MathHelper.Clamp(ElapsedTime / Duration, 0f, 1f);

        float time = AutoRewind ? ElapsedTime : (t * Duration);
        float radius = _radiusSpeed * time;
        float angle = _rotationSpeed * time;

        Vector2 offset = _centerOffset + new Vector2(
            (float)Math.Cos(angle) * radius,
            (float)Math.Sin(angle) * radius
        );

        return new BackgroundState
        {
            ScreenOffset = offset,
            Zoom = 1f,
            ViewCenter = Vector2.Zero,
            Rotation = 0f,
            Tint = Color.White
        };
    }
}

