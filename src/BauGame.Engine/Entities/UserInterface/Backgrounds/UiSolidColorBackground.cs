using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;

/// <summary>
///     Fondo de color sólido
/// </summary>
public class UiSolidColorBackground(Styles.UiStyle style) : UiAbstractBackground(style)
{
    /// <summary>
	///		Actualiza el control
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Rectangle position, Managers.GameContext gameContext)
	{
		camera.SpriteBatchController.FiguresRenderer.DrawRectangle(position, Color * Opacity);
	}
}
