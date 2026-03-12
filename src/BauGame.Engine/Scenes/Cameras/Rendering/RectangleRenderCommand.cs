using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Comando para dibujar rectángulos
/// </summary>
public class RectangleRenderCommand : AbstractRenderCommand
{
    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public override void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
        if (director.WhitePixel is not null && !Transform.Destination.IsEmpty)
        {
            if (MustShowShadow())
                DrawShadowRectangle(spriteBatch, director.WhitePixel, Transform.Destination);
            else if (CornerRadius > 0)
                DrawRoundedRectangle(spriteBatch, director.WhitePixel, Transform.Destination);
            else if (Fill)
                DrawSolidRectangle(spriteBatch, director.WhitePixel, Transform.Destination);
            else
                DrawRectangleOutline(spriteBatch, director.WhitePixel, Transform.Destination);
        }
    }

	/// <summary>
	///		Dibuja un rectángulo sólido
	/// </summary>
	public void DrawSolidRectangle(SpriteBatch spriteBatch, Texture2D whitePixel, Rectangle rectangle)
	{
		spriteBatch.Draw(whitePixel, rectangle, Presentation.Color);
	}

	/// <summary>
	///		Dibuja las líneas de un rectángulo
	/// </summary>
	private void DrawRectangleOutline(SpriteBatch spriteBatch, Texture2D whitePixel, Rectangle rectangle)
	{
        DrawRectangleOutline(spriteBatch, whitePixel, rectangle, Thickness, Presentation.Color);
	}

	/// <summary>
	///		Dibuja las líneas de un rectángulo
	/// </summary>
	private void DrawRectangleOutline(SpriteBatch spriteBatch, Texture2D whitePixel, Rectangle rectangle, int thickness, Color color)
	{
		// Arriba
		spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, Thickness), color);
		// Abajo
		spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - Thickness, rectangle.Width, Thickness), color);
		// Izquierda
		spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X, rectangle.Y, Thickness, rectangle.Height), color);
		// Derecha
		spriteBatch.Draw(whitePixel, new Rectangle(rectangle.X + rectangle.Width - Thickness, rectangle.Y, Thickness, rectangle.Height), color);
	}

	/// <summary>
	///		Dibuja un rectángulo con sombras
	/// </summary>
	public void DrawShadowRectangle(SpriteBatch spriteBatch, Texture2D whitePixel, Rectangle rectangle)
	{
        Rectangle shadowBounds = new(Transform.Destination.X + (int) ShadowOffset.X, Transform.Destination.Y + (int) ShadowOffset.Y, 
                                     Transform.Destination.Width, Transform.Destination.Height);
        int radius = Math.Min(2, ShadowBlurRadius);
            
            // Simula blur con capas concéntricas cada vez más transparentes
            for (int step = radius; step >= 0; step--)
            {
                Rectangle expanded = new Rectangle(shadowBounds.X - step, shadowBounds.Y - step, 
                                                    shadowBounds.Width + step * 2, shadowBounds.Height + step * 2);
                float alpha = ShadowColor.A / 255f * (1f - (step / (float) (radius + 1)));
                Color color = new(ShadowColor.R, ShadowColor.G, ShadowColor.B, (byte) (alpha * 255));
                    
                    // Dibuja el rectángulo
                    DrawRectangleOutline(spriteBatch, whitePixel, expanded, 1, color);
            }
            // Dibuja el rectángulo original
            DrawRectangleOutline(spriteBatch, whitePixel, Transform.Destination, Thickness, Presentation.Color);
	}

	/// <summary>
	///		Dibuja un rectángulo redondeado
	/// </summary>
	public void DrawRoundedRectangle(SpriteBatch spriteBatch, Texture2D whitePixel, Rectangle rectangle)
	{
        int radius = (int) Math.Min(CornerRadius, 0.5f * Math.Min(Transform.Destination.Width, Transform.Destination.Height));

            // Dibuja los lados dejando espacio para las esquinas
            spriteBatch.Draw(whitePixel, new Rectangle(Transform.Destination.X + radius, 
                                                       Transform.Destination.Y, Transform.Destination.Width - radius * 2, Thickness), 
                             Presentation.Color);
            spriteBatch.Draw(whitePixel, new Rectangle(Transform.Destination.X + radius, Transform.Destination.Bottom - Thickness, 
                                                       Transform.Destination.Width - radius * 2, Thickness), 
                             Presentation.Color);
            spriteBatch.Draw(whitePixel, new Rectangle(Transform.Destination.X, Transform.Destination.Y + radius, 
                                                       Thickness, Transform.Destination.Height - radius * 2), 
                             Presentation.Color);
            spriteBatch.Draw(whitePixel, new Rectangle(Transform.Destination.Right - Thickness, Transform.Destination.Y + radius, 
                                                       Thickness, Transform.Destination.Height - radius * 2), 
                             Presentation.Color);
            // Dibuja las esquinas
            DrawCorner(spriteBatch, whitePixel, Transform.Destination.X + radius, Transform.Destination.Y + radius, -1, -1, radius);
            DrawCorner(spriteBatch, whitePixel, Transform.Destination.Right - radius, Transform.Destination.Y + radius, 1, -1, radius);
            DrawCorner(spriteBatch, whitePixel, Transform.Destination.X + radius, Transform.Destination.Bottom - radius, -1, 1, radius);
            DrawCorner(spriteBatch, whitePixel, Transform.Destination.Right - radius, Transform.Destination.Bottom - radius, 1, 1, radius);
    }

    /// <summary>
    ///     Dibuja una esquina. Utiliza una aproximación dibujando segmentos pequeños escalonados
    /// </summary>
    private void DrawCorner(SpriteBatch spriteBatch, Texture2D whitePixel, int cx, int cy, int dirX, int dirY, int radius)
    {
        int steps = Math.Max(3, radius / 2);

            // Dibuja rectángulos pequeños            
            for (int step = 0; step < steps; step++)
            {
                float angle = MathHelper.PiOver2 * (step / (float) (steps - 1));
                int x = cx + (int) (dirX * (radius - Math.Sin(angle) * radius));
                int y = cy + (int) (dirY * (radius - Math.Cos(angle) * radius));
                
                    // Dibuja el rectángulo
                    spriteBatch.Draw(whitePixel, new Rectangle(x, y, Thickness, Thickness), Presentation.Color);
            }
    }

    /// <summary>
    ///     Indica si se debe dibujar la sombra
    /// </summary>
    public bool MustShowShadow() => ShadowOffset != Vector2.Zero;

    /// <summary>
    ///     Transformación
    /// </summary>
    public TransformRenderModel Transform { get; } = new();

    /// <summary>
    ///     Datos de presentación
    /// </summary>
    public PresentationRenderModel Presentation { get; } = new();

    /// <summary>
    ///     Ancho del borde
    /// </summary>
    public int Thickness { get; set; }

    /// <summary>
    ///     Radio de las esquinas
    /// </summary>
    public int CornerRadius { get; set; }

    /// <summary>
    ///     Indica si se debe rellenar
    /// </summary>
    public bool Fill { get; set; }

    /// <summary>
    ///     Color de la sombra
    /// </summary>
    public Color ShadowColor { get; set; }

    /// <summary>
    ///     Desplazamiento de la sombra
    /// </summary>
    public Vector2 ShadowOffset { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Radio de blur de la sombra
    /// </summary>
    public int ShadowBlurRadius { get; set; }
}
