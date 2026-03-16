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
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, Managers.GameContext gameContext)
	{
		renderingManager.FiguresRenderer.DrawRectangle(position, Color * Opacity);
	}
}
