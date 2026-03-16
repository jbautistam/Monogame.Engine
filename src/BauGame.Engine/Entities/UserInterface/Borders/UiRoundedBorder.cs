using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Borders;

/// <summary>
///     Borde con esquinas redondeadas
/// </summary>
public class UiRoundedBorder(Styles.UiStyle style) : UiAbstractBorder(style)
{
    /// <summary>
    ///     Actualiza el borde
    /// </summary>
	public override void Update(GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Rectangle position, GameContext gameContext)
    {
        int radius = (int) Math.Min(Radius, Math.Min(position.Width, position.Height) / 2f);

            // Dibuja los lados dejando espacio para las esquinas
            camera.SpriteBatchController.FiguresRenderer.DrawRectangle(new Rectangle(position.X + radius, position.Y, position.Width - radius * 2, Thickness), 
                                                                       Color * Opacity);
            camera.SpriteBatchController.FiguresRenderer.DrawRectangle(new Rectangle(position.X + radius, position.Bottom - Thickness, position.Width - radius * 2, Thickness), 
                                                                       Color * Opacity);
            camera.SpriteBatchController.FiguresRenderer.DrawRectangle(new Rectangle(position.X, position.Y + radius, Thickness, position.Height - radius * 2), 
                                                                       Color * Opacity);
            camera.SpriteBatchController.FiguresRenderer.DrawRectangle(new Rectangle(position.Right - Thickness, position.Y + radius, Thickness, position.Height - radius * 2), 
                                                                       Color * Opacity);
            // Dibuja las esquinas
            DrawCorner(camera, position.X + radius, position.Y + radius, -1, -1, radius);
            DrawCorner(camera, position.Right - radius, position.Y + radius, 1, -1, radius);
            DrawCorner(camera, position.X + radius, position.Bottom - radius, -1, 1, radius);
            DrawCorner(camera, position.Right - radius, position.Bottom - radius, 1, 1, radius);
    }

    /// <summary>
    ///     Dibuja una esquina. Utiliza una aproximación dibujando segmentos pequeños escalonados
    /// </summary>
    private void DrawCorner(Camera2D camera2D, int cx, int cy, int dirX, int dirY, int radius)
    {
        int steps = Math.Max(3, radius / 2);

            // Dibuja rectángulos pequeños            
            for (int step = 0; step < steps; step++)
            {
                float angle = MathHelper.PiOver2 * (step / (float) (steps - 1));
                int x = cx + (int) (dirX * (radius - Math.Sin(angle) * radius));
                int y = cy + (int) (dirY * (radius - Math.Cos(angle) * radius));
                
                    // Dibuja el rectángulo
                    camera2D.SpriteBatchController.FiguresRenderer.DrawRectangle(new Rectangle(x, y, Thickness, Thickness), Color * Opacity);
            }
    }

    /// <summary>
    ///     Radio del borde redondeado
    /// </summary>
    public int Radius { get; set; }
}
