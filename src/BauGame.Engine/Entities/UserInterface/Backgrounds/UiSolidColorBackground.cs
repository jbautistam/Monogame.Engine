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
		camera.SpriteBatchController.DrawRectangle(position, Color * Opacity);
	}

	/// <summary>
	///		Prepara los comandos de presentación
	/// </summary>
	public override void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, Rectangle bounds, Managers.GameContext gameContext)
	{
		//TODO: aún no hace nada
		//builder.WithCommand(Sprite)
		//		.WithTransform(bounds, Vector2.Zero)
		//		.WithColor(Color * Opacity);
	}  
}
