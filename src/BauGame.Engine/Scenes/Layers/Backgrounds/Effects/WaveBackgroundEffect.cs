using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///     Efecto de onda sobre el fondo
/// </summary>
public class WaveBackgroundEffect(float duration, bool autoRewind) : AbstractBackgroundEffect(duration, autoRewind)
{
    // Variables privadas
    private float _phaseX = Tools.Randomizer.GetRandom(0, (float) Math.PI * 2);
    private float _phaseY = Tools.Randomizer.GetRandom(0, (float) Math.PI * 2 + 1.3f);

    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
    protected override void UpdateEffect(Managers.GameContext gameContext) 
    {
        float angle = ElapsedTime * Frequency * MathHelper.TwoPi;

            // Actualiza el desplazamiento del fondo
            State.ScreenOffset = new Vector2((float) Math.Sin(angle + _phaseX) * AmplitudeX, (float) Math.Cos(angle + _phaseY) * AmplitudeY);
    }

    /// <summary>
    ///     Amplitud de la onda en el eje X
    /// </summary>
    public required float AmplitudeX { get; init; }

    /// <summary>
    ///     Amplitud de la onda en el eje X
    /// </summary>
    public required float AmplitudeY { get; init; }

    /// <summary>
    ///     Frecuencia de la onda
    /// </summary>
    public float Frequency { get; init; }
}
