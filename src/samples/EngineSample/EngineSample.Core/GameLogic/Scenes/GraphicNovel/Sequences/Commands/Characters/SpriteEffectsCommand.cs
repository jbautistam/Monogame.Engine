namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Characters;

/// <summary>
///     Cambia el ZOrder del actor (instantáneo)
/// </summary>
public class SpriteEffectsCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        actor.Renderer.SpriteEffects = SpriteEffects;
    }

    /// <summary>
    ///     Cambio de efectos del sprite
    /// </summary>
    public required Microsoft.Xna.Framework.Graphics.SpriteEffects SpriteEffects { get; init; }
}
