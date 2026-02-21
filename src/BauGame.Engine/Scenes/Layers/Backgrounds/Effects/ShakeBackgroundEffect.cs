using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///     Efecto de shake sobre un fondo
/// </summary>
public class ShakeBackgroundEffect(float duration, bool autoRewind) : AbstractBackgroundEffect(duration, autoRewind)
{
    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
	protected override void UpdateEffect(GameContext gameContext)
	{
        float intensity = (1f - Progress) * (1f - Progress) * Magnitude;

            // Cambia el desplazamiento de la pantalla
            State.ScreenOffset = Tools.Randomizer.GetRandomDirection() * intensity;
    }

    /// <summary>
    ///     Magnitud del efecto
    /// </summary>
    public required float Magnitude { get; init; }
}