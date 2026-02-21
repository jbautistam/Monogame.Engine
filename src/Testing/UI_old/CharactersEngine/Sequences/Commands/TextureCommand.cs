using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Cambio de textura (instantáneo, no tiene interpolación)
/// </summary>
public class TextureCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (Texture is not null)
            actor.Texture = Texture;
    }

    /// <summary>
    ///     Textura
    /// </summary>
    public Texture2D? Texture { get; set; }
}
