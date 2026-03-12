namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Cambia el ZOrder del actor (instantáneo)
/// </summary>
public class ZOrderCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        actor.ZOrder = Target;
    }

    /// <summary>
    ///     Profundidad destino
    /// </summary>
    public required int Target { get; init; }
}
