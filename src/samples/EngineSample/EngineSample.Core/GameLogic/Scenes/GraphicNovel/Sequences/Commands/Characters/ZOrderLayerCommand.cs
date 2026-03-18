namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Characters;

/// <summary>
///     Cambia la capa de un personaje
/// </summary>
public class ZOrderLayerCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        if (actor is Actors.CharacterActor character)
        {
            // Cambia las copias lógicas
            character.Definition.LogicalLayer = LogicalLayer;
            character.Definition.LogicalZOrder = LogicalZOrder;
            // Cambia el zorder real del actor
            actor.ZOrder = character.Definition.ZOrder;
        }
    }

    /// <summary>
    ///     Capa destino
    /// </summary>
    public required int LogicalLayer { get; init; }

    /// <summary>
    ///     Profundidad destino
    /// </summary>
    public required int LogicalZOrder { get; init; }
}
