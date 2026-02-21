using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Comando de flash: destello blanco momentáneo
/// </summary>
public class FlashCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        float progress = GetProgress(sequenceTime);
        float intensity;
        
            // Cambia la intensidad según pasa el tiempo
            if (progress < 0.5f)
                intensity = progress * 2 * PeakIntensity;
            else
                intensity = (1 - progress) * 2 * PeakIntensity;
            // Cambia el color
            actor.Color = Color.Lerp(actor.Color, Color.White, intensity);
    }

    /// <summary>
    ///     Momento de máximo brillo (0..1)
    /// </summary>
    public float PeakIntensity { get; set; } = 0.5f;
}
