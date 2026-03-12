using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Mueve un actor
/// </summary>
public class MoveCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _start;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        // Guarda la posición inicial
        if (!_initialized)
        {
            _start = actor.Position;
            _initialized = true;
        }
        // Calcula la posición interplada
        actor.Position = Interpolate(_start, Target, sequenceTime);
    }

    /// <summary>
    ///     Posición final
    /// </summary>
    public required Vector2 Target { get; init; }
}
