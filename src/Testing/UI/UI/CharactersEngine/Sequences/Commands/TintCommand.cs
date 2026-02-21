using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Comando para cambiar el color de un actor
/// </summary>
public class TintCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Color _start;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        // Guarda el valor inicial
        if (!_initialized)
        {
            _start = actor.Color;
            _initialized = true;
        }
        // Aplica el valor interpolado
        actor.Color = Interpolate(_start, Target, sequenceTime);
    }

    /// <summary>
    ///     Color final
    /// </summary>
    public required Color Target { get; init; }
}
