using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Comando para dibujar un texto
/// </summary>
public class TextRenderCommand : AbstractRenderCommand
{
    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public override void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
        if (Font is not null && !string.IsNullOrEmpty(Text))
            spriteBatch.DrawString(Font, Text, Transform.Coordinates.Position, Presentation.Color, Transform.Coordinates.Rotation, 
                                   Transform.Coordinates.Origin, Transform.Coordinates.Scale.X, Presentation.Effect, 0f);
    }

    /// <summary>
    ///     Texto a representar
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    ///     Fuente
    /// </summary>
    public required SpriteFont Font { get; init; }

    /// <summary>
    ///     Transformación
    /// </summary>
    public TransformRenderModel Transform { get; } = new();

    /// <summary>
    ///     Presentación
    /// </summary>
    public PresentationRenderModel Presentation { get; } = new();
}
