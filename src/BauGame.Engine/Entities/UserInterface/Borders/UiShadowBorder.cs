using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Borders;

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
	public override void Draw(Camera2D camera, Rectangle position, GameContext gameContext)
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
                        camera.SpriteBatchController.DrawRectangleOutline(expanded, color);
                }
            }
            else
                camera.SpriteBatchController.DrawRectangleOutline(shadowBounds, ShadowColor);
    }

    /// <summary>
    ///     Prepara los comandos de presentación
    /// </summary>
	public override void PrepareRenderCommands(RenderCommandsBuilder builder, Rectangle bounds, GameContext gameContext)
	{
        builder.WithCommand(Thickness, false)
               .WithTransform(bounds, Vector2.Zero)
               .WithShadow(Offset, ShadowColor, BlurRadius)
               .WithColor(Color * Opacity);
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
