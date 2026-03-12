using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Comando para representación de una textura
/// </summary>
public class TextureRenderCommand : AbstractRenderCommand
{
    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public override void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
        if (Texture is not null)
        {
            if (!Transform.Destination.IsEmpty)
            {
                if (Source.HasValue)
                    spriteBatch.Draw(Texture, Transform.Destination, Source.Value, Presentation.Color, 
                                     Transform.Coordinates.Rotation, Transform.Coordinates.Origin, Presentation.Effect, 0f);
                else
                    spriteBatch.Draw(Texture, Transform.Destination, null, Presentation.Color, 
                                     Transform.Coordinates.Rotation, Transform.Coordinates.Origin, Presentation.Effect, 0f);
            }
            else
                spriteBatch.Draw(Texture, Transform.Coordinates.Position, Source, Presentation.Color, 
                                 Transform.Coordinates.Rotation, Transform.Coordinates.Origin, Transform.Coordinates.Scale, Presentation.Effect, 0f);
        }
    }

    /// <summary>
    ///     Textura a dibujar
    /// </summary>
    public required Texture2D Texture { get; init; }

    /// <summary>
    ///     Rectángulo original dentro de la textura
    /// </summary>
    public Rectangle? Source { get; set; }

    /// <summary>
    ///     Transformación
    /// </summary>
    public TransformRenderModel Transform { get; } = new();

    /// <summary>
    ///     Datos de presentación
    /// </summary>
    public PresentationRenderModel Presentation { get; } = new();

    /// <summary>
    ///     Modo de dibujo
    /// </summary>
    public SpriteRenderCommand.DrawType DrawMode { get; set; } = SpriteRenderCommand.DrawType.Normal;
}
