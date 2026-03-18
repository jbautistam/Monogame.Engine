using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Borders;

/// <summary>
///     Borde simple de línea continua con color uniforme
/// </summary>
public class UiSolidBorder(Styles.UiStyle style) : UiAbstractBorder(style)
{
	/// <summary>
	///		Actualiza el borde
	/// </summary>
	public override void Update(GameContext gameContext) {}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, GameContext gameContext)
    {
        if (ShadowOffset != Vector2.Zero)
            DrawShadow(renderingManager, position, gameContext);
        else if (CornerRadius > 0)
            DrawRoundBorder(renderingManager, position, gameContext);
        else
            DrawOutline(renderingManager, position, gameContext);
    }

	/// <summary>
	///		Dibuja el rectángulo
	/// </summary>
	private void DrawOutline(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, GameContext gameContext)
    {
        // Arriba
        renderingManager.FiguresRenderer.DrawLine(new Vector2(position.X, position.Y), 
                                                  new Vector2(position.X + position.Width, position.Y), 
                                                  Color * Opacity, Thickness);
        // Abajo
        renderingManager.FiguresRenderer.DrawLine(new Vector2(position.X, position.Bottom - Thickness), 
                                                  new Vector2(position.X + position.Width, position.Bottom - Thickness), 
                                                  Color * Opacity, Thickness);
        // Izquierda
        renderingManager.FiguresRenderer.DrawLine(new Vector2(position.X, position.Y), 
                                                  new Vector2(position.X, position.Bottom), 
                                                  Color * Opacity, Thickness);
        // Derecha
        renderingManager.FiguresRenderer.DrawLine(new Vector2(position.Right, position.Y), 
                                                  new Vector2(position.Right, position.Bottom), 
                                                  Color * Opacity, Thickness);
    }

	/// <summary>
	///		Dibuja el control con la sombra
	/// </summary>
	private void DrawShadow(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, GameContext gameContext)
    {
        Rectangle shadowBounds = new(position.X + (int) ShadowOffset.X, position.Y + (int) ShadowOffset.Y, position.Width, position.Height);
            
            // Dibuja la sombra como un rectángulo semitransparente más grande
            if (ShadowBlurRadius > 0)
            {
                // Simula blur con capas concéntricas cada vez más transparentes
                for (int step = ShadowBlurRadius; step >= 0; step--)
                {
                    Rectangle expanded = new Rectangle(shadowBounds.X - step, shadowBounds.Y - step, 
                                                       shadowBounds.Width + step * 2, shadowBounds.Height + step * 2);
                    float alpha = ShadowColor.A / 255f * (1f - (step / (float) (ShadowBlurRadius + 1)));
                    Color color = new(ShadowColor.R, ShadowColor.G, ShadowColor.B, (byte) (alpha * 255));
                    
                        // Dibuja el rectángulo
                        renderingManager.FiguresRenderer.DrawRectangleOutline(expanded, color);
                }
            }
            else
                renderingManager.FiguresRenderer.DrawRectangleOutline(shadowBounds, ShadowColor);
    }

	/// <summary>
	///		Dibuja el control con el borde redondeado
	/// </summary>
	private void DrawRoundBorder(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, GameContext gameContext)
    {
        int radius = (int) Math.Min(CornerRadius, Math.Min(position.Width, position.Height) / 2f);

            // Dibuja los lados dejando espacio para las esquinas
            renderingManager.FiguresRenderer.DrawRectangle(new Rectangle(position.X + radius, position.Y, position.Width - radius * 2, Thickness), 
                                                           Color * Opacity);
            renderingManager.FiguresRenderer.DrawRectangle(new Rectangle(position.X + radius, position.Bottom - Thickness, position.Width - radius * 2, Thickness), 
                                                           Color * Opacity);
            renderingManager.FiguresRenderer.DrawRectangle(new Rectangle(position.X, position.Y + radius, Thickness, position.Height - radius * 2), 
                                                           Color * Opacity);
            renderingManager.FiguresRenderer.DrawRectangle(new Rectangle(position.Right - Thickness, position.Y + radius, Thickness, position.Height - radius * 2), 
                                                           Color * Opacity);
            // Dibuja las esquinas
            DrawCorner(renderingManager, position.X + radius, position.Y + radius, -1, -1, radius);
            DrawCorner(renderingManager, position.Right - radius, position.Y + radius, 1, -1, radius);
            DrawCorner(renderingManager, position.X + radius, position.Bottom - radius, -1, 1, radius);
            DrawCorner(renderingManager, position.Right - radius, position.Bottom - radius, 1, 1, radius);
    }

    /// <summary>
    ///     Dibuja una esquina. Utiliza una aproximación dibujando segmentos pequeños escalonados
    /// </summary>
    private void DrawCorner(Scenes.Rendering.RenderingManager renderingManager, int cx, int cy, int dirX, int dirY, int radius)
    {
        int steps = Math.Max(3, radius / 2);

            // Dibuja rectángulos pequeños            
            for (int step = 0; step < steps; step++)
            {
                float angle = MathHelper.PiOver2 * (step / (float) (steps - 1));
                int x = cx + (int) (dirX * (radius - Math.Sin(angle) * radius));
                int y = cy + (int) (dirY * (radius - Math.Cos(angle) * radius));
                
                    // Dibuja el rectángulo
                    renderingManager.FiguresRenderer.DrawRectangle(new Rectangle(x, y, Thickness, Thickness), Color * Opacity);
            }
    }

    /// <summary>
    ///     Radio del borde redondeado
    /// </summary>
    public int CornerRadius { get; set; }

    /// <summary>
    ///     Color de la sombra
    /// </summary>
    public Color ShadowColor { get; set; } = new(0, 0, 0, 128);

    /// <summary>
    ///     Desplazamiento de la sombra
    /// </summary>
    public Vector2 ShadowOffset { get; set; } = new(4, 4);

    /// <summary>
    ///     Radio de blur (no se utilizan shaders, se simula)
    /// </summary>
    public int ShadowBlurRadius { get; set; } = 4;
}
