using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Comando para escalar un actor
/// </summary>
public class ScaleCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _startScale;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        // Guarda la escala inicial
        if (!_initialized)
        {
            _startScale = actor.Scale;
            _initialized = true;
        }
        // Asigna la escala actual
        actor.Scale = Interpolate(_startScale, TargetScale, sequenceTime);
    }

    /// <summary>
    ///     Escala final
    /// </summary>
    public required Vector2 TargetScale { get; init; }
}
