using Bau.Libraries.BauGame.Engine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///     Efecto de shake sobre un fondo
/// </summary>
public class PulseEffect(float duration, bool autoRewind) : AbstractBackgroundEffect(duration, autoRewind)
{
    /// <summary>
    ///     Tipo de pulso
    /// </summary>
    public enum PulseTarget 
    { 
        Zoom, 
        Rotation 
    }

    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
	protected override void UpdateEffect(GameContext gameContext)
	{
        float pulse = (float) Math.Sin(Progress * MathHelper.Pi);

            // Cambia los datos del efecto sobre la pantalla
            if (Target == PulseTarget.Zoom)
                State.Zoom = 1f + pulse * Magnitude;
            else
                State.Rotation = pulse * Magnitude;
    }

    /// <summary>
    ///     Magnitud del efecto
    /// </summary>
    public required float Magnitude { get; init; }

    /// <summary>
    ///     Destino del pulso
    /// </summary>
    public PulseTarget Target { get; set; } = PulseTarget.Zoom;
}
