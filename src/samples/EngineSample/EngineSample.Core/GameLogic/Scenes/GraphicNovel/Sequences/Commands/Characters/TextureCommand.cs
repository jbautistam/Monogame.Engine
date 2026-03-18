namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Characters;

/// <summary>
///     Cambio de textura (instantáneo, no tiene interpolación)
/// </summary>
public class TextureCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        if (!string.IsNullOrWhiteSpace(Texture))
            actor.Renderer.Sprite = new Bau.BauEngine.Entities.Sprites.SpriteDefinition(Texture, Region);
    }

    /// <summary>
    ///     Textura
    /// </summary>
    public required string Texture { get; init; }

    /// <summary>
    ///     Región
    /// </summary>
    public string? Region { get; set; }
}
