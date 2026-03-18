using Microsoft.Xna.Framework;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Entities.UserInterface.Backgrounds;

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
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, Managers.GameContext gameContext)
	{
		if (Sprite is not null)
			renderingManager.SpriteRenderer.Draw(Sprite, position, Vector2.Zero, 0, Color * Opacity);
	}

	/// <summary>
	///		Definición del sprite
	/// </summary>
	public SpriteDefinition? Sprite { get; set; }
}