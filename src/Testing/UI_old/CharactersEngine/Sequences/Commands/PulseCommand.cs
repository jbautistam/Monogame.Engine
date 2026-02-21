using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Pulso - latido: escala oscilante
/// </summary>
public class PulseCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _startScale;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        float progress = GetProgress(sequenceTime);
        float sine = MathF.Sin(progress * MathHelper.TwoPi * Pulses);
        float pulse = (sine + 1) / 2 * Amplitude * (1 - MathTools.Easing.EasingFunctionsHelper.Apply(progress, Easing));

            // Recoge los datos iniciales
            if (!_initialized)
            {
                _startScale = actor.Scale;
                _initialized = true;
            }
            // Cambia la escala
            actor.Scale = _startScale * (1 + pulse);
    }

    /// <summary>
    ///     Cuánto crece el actor. Por ejemplo: 0.2 es un 20% más grande
    /// </summary>
    public float Amplitude { get; init; }

    /// <summary>
    ///     Número de pulsos
    /// </summary>
    public required int Pulses { get; init; }
}
