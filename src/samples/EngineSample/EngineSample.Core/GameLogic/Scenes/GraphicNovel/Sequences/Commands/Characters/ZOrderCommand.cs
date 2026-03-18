namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Characters;

/// <summary>
///     Cambia el ZOrder del actor (instantáneo)
/// </summary>
public class ZOrderCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        actor.ZOrder = ZOrder;
    }

    /// <summary>
    ///     Profundidad destino
    /// </summary>
    public required int ZOrder { get; init; }
}
