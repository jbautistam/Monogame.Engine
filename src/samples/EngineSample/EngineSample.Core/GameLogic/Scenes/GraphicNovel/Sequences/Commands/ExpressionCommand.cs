namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Cambio de expresión (instantáneo, no tiene interpolación)
/// </summary>
public class ExpressionCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        if (actor is Actors.CharacterActor character)
            character.UpdateExpression(Expression);
    }

    /// <summary>
    ///     Expresión
    /// </summary>
    public required string Expression { get; init; }
}
