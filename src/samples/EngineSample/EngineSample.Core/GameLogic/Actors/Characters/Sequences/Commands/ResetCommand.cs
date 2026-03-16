using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Resetea la posición y la opacidad de un actor (instantáneo)
/// </summary>
public class ResetCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        actor.Transform.Bounds.TopLeft = ToWorld(Position);
        actor.Renderer.Opacity = Opacity;
        actor.Renderer.SpriteEffects = SpriteEffects;
    }

    /// <summary>
    ///     Posición final
    /// </summary>
    public required Vector2 Position { get; init; }

    /// <summary>
    ///     Opacidad final
    /// </summary>
    public required float Opacity { get; init; }

    /// <summary>
    ///     Efectos del sprite
    /// </summary>
    public required Microsoft.Xna.Framework.Graphics.SpriteEffects SpriteEffects { get; init; }
}
