using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Borders;

/// <summary>
///     Borde con una sombra proyectada para simular profundidad
/// </summary>
public class UiShadowBorder(Styles.UiStyle style) : UiAbstractBorder(style)
{
	/// <summary>
	///		Actualiza el fondo
	/// </summary>
	public override void Update(GameContext gameContext) {}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, Rectangle position, GameContext gameContext)
    {
        Rectangle shadowBounds = new(position.X + (int) Offset.X, position.Y + (int) Offset.Y, position.Width, position.Height);
            
            // Dibuja la sombra como un rectángulo semitransparente más grande
            if (BlurRadius > 0)
            {
                // Simula blur con capas concéntricas cada vez más transparentes
                for (int step = BlurRadius; step >= 0; step--)
                {
                    Rectangle expanded = new Rectangle(shadowBounds.X - step, shadowBounds.Y - step, 
                                                       shadowBounds.Width + step * 2, shadowBounds.Height + step * 2);
                    float alpha = ShadowColor.A / 255f * (1f - (step / (float) (BlurRadius + 1)));
                    Color color = new(ShadowColor.R, ShadowColor.G, ShadowColor.B, (byte) (alpha * 255));
                    
                        // Dibuja el rectángulo
                        renderingManager.FiguresRenderer.DrawRectangleOutline(expanded, color);
                }
            }
            else
                renderingManager.FiguresRenderer.DrawRectangleOutline(shadowBounds, ShadowColor);
    }

    /// <summary>
    ///     Color de la sombra
    /// </summary>
    public Color ShadowColor { get; set; } = new(0, 0, 0, 128);

    /// <summary>
    ///     Desplazamiento de la sombra
    /// </summary>
    public Vector2 Offset { get; set; } = new(4, 4);

    /// <summary>
    ///     Radio de blur (no se utilizan shaders, se simula)
    /// </summary>
    public int BlurRadius { get; set; } = 4;
}
