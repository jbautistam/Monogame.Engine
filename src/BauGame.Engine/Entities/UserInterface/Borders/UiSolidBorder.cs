using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Borders;

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
	public override void Draw(Camera2D camera, Rectangle position, GameContext gameContext)
    {
        // Arriba
        camera.SpriteBatchController.DrawLine(new Vector2(position.X, position.Y), 
                                              new Vector2(position.X + position.Width, position.Y), 
                                              Color * Opacity, Thickness);
        // Abajo
        camera.SpriteBatchController.DrawLine(new Vector2(position.X, position.Bottom - Thickness), 
                                              new Vector2(position.X + position.Width, position.Bottom - Thickness), 
                                              Color * Opacity, Thickness);
        // Izquierda
        camera.SpriteBatchController.DrawLine(new Vector2(position.X, position.Y), 
                                              new Vector2(position.X, position.Bottom), 
                                              Color * Opacity, Thickness);
        // Derecha
        camera.SpriteBatchController.DrawLine(new Vector2(position.Right, position.Y), 
                                              new Vector2(position.Right, position.Bottom), 
                                              Color * Opacity, Thickness);
    }
}
