using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///     Efecto de una onda
/// </summary>
public class WaveCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private float _phaseX = Bau.BauEngine.Tools.Randomizer.GetRandom(0, (float) Math.PI * 2);
    private float _phaseY = Bau.BauEngine.Tools.Randomizer.GetRandom(0, (float) Math.PI * 2 + 1.3f);
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        float angle = Progress * Frequency * MathHelper.TwoPi;

            // Actualiza la posición del fondo
            actor.Transform.Bounds.Location = new Vector2((float) Math.Sin(angle + _phaseX) * AmplitudeX, (float) Math.Cos(angle + _phaseY) * AmplitudeY);
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