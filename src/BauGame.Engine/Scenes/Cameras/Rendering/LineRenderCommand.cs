using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Comando para dibujar una línea
/// </summary>
public class LineRenderCommand : AbstractRenderCommand
{
    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public override void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
		if (director.WhitePixel is not null)
		{
			Vector2 edge = End - Start;
            float length = edge.Length();

                if (length > 0.01f)
                {
			        float angle = (float) Math.Atan2(edge.Y, edge.X);

				        // Dibuja la línea como un rectángulo rotado
				        spriteBatch.Draw(director.WhitePixel, Start, null, Presentation.Color, angle, Vector2.Zero, 
                                         new Vector2(length, Thickness), SpriteEffects.None, 0);
                }
		}
	}

    /// <summary>
    ///     Inicio de la línea
    /// </summary>
    public Vector2 Start { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Fin de la línea
    /// </summary>
    public Vector2 End { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Ancho de línea
    /// </summary>
    public int Thickness { get; set; }

    /// <summary>
    ///     Datos de presentación
    /// </summary>
    public PresentationRenderModel Presentation { get; } = new();
}
