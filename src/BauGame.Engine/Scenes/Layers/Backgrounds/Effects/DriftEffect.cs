using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///     Efecto de velocidad sobre un fondo
/// </summary>
public class DriftEffect(float duration, bool autoRewind) : AbstractBackgroundEffect(duration, autoRewind)
{
    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
	protected override void UpdateEffect(Managers.GameContext gameContext)
	{
        State.ScreenOffset = Velocity * Progress;
    }

    /// <summary>
    ///     Velocidad
    /// </summary>
    public required Vector2 Velocity { get; init; }
}