using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Mueve un actor a una posición directamente sin ninguna interpolación
/// </summary>
public class MoveToCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        actor.Transform.Bounds.TopLeft = ToWorld(Target);
    }

    /// <summary>
    ///     Posición final
    /// </summary>
    public required Vector2 Target { get; init; }
}
