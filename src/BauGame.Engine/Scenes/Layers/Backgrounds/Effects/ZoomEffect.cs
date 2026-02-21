using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///     Efecto de Zoom sobre el fondo
/// </summary>
public class ZoomEffect(float duration, bool autoRewind) : AbstractBackgroundEffect(duration, autoRewind)
{
    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
    protected override void UpdateEffect(Managers.GameContext gameContext) 
    {
        float current = MathHelper.Lerp(Start, End, Progress);

            // Calcula el zoom
            if (Start != 0)
                State.Zoom = current / Start;
    }

    /// <summary>
    ///     Inicio del Zoom
    /// </summary>
    public float Start { get; init; }

    /// <summary>
    ///     Fin del Zoom
    /// </summary>
    public float End { get; init; }
}
