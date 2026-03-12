using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Entities.Common;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;

/// <summary>
///		Componente de fondo
/// </summary>
public class UiBackground(Styles.UiStyle style) : UiAbstractBackground(style)
{
	/// <summary>
	///		Actualiza el control
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
		Sprite?.Update(gameContext);
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Rectangle position, Managers.GameContext gameContext)
	{
		Sprite?.Draw(camera, position, Vector2.Zero, 0, Color * Opacity);
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	public override void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, Rectangle bounds, Managers.GameContext gameContext)
	{
		if (Sprite is not null)
			builder.WithCommand(Sprite)
				   .WithTransform(bounds, Vector2.Zero)
				   .WithColor(Color * Opacity);
	}  

	/// <summary>
	///		Definición del sprite
	/// </summary>
	public SpriteDefinition? Sprite { get; set; }
}