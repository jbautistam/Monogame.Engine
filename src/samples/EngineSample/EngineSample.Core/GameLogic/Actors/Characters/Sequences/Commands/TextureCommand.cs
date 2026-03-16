namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Cambio de textura (instantáneo, no tiene interpolación)
/// </summary>
public class TextureCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        if (!string.IsNullOrWhiteSpace(Texture))
        {
            actor.Renderer.Texture = Texture;
            actor.Renderer.Region = Region;
        }
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
