using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Resetea los datos de un actor
/// </summary>
public class ResetCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        actor.Transform.Bounds.Location = ToWorld(Position);
        actor.Transform.OriginPoint = Bau.BauEngine.Actors.Components.Transforms.TransformComponent.OriginPointType.Center;
        actor.Transform.Rotation = Rotation;
        actor.Renderer.Scale = Scale;
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
    ///     Escala
    /// </summary>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    ///     Rotación
    /// </summary>
    public required float Rotation { get; init; }

    /// <summary>
    ///     Efectos del sprite
    /// </summary>
    public required Microsoft.Xna.Framework.Graphics.SpriteEffects SpriteEffects { get; init; }
}
