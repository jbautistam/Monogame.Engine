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
        float current = MathHelper.Lerp(StartZoom, EndZoom, Progress);

            // Calcula el zoom
            if (StartZoom != 0)
                State.Zoom = current / StartZoom;
    }

    /// <summary>
    ///     Zoom inicial
    /// </summary>
    public float StartZoom { get; init; }

    /// <summary>
    ///     Zoom final
    /// </summary>
    public float EndZoom { get; init; }
}
