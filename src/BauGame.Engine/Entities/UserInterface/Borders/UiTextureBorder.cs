using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Borders;

/// <summary>
///     Borde con textura
/// </summary>
public class UiTextureBorder(Styles.UiStyle style) : UiAbstractBorder(style)
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
		Rectangle destination = InflateWithSprite(position);

			// Dibuja el control
			if (Sprite is not null)
				camera.RenderingManager.SpriteRenderer.Draw(Sprite, destination, Vector2.Zero, 0, Color * Opacity);
	}

	/// <summary>
	///		Cambia el tamaño para que esté por fuera del fondo
	/// </summary>
	private Rectangle InflateWithSprite(Rectangle bounds)
	{
		return Inflate(bounds);
	}

    /// <summary>
    ///     Definición de la textura
    /// </summary>
    public SpriteDefinition? Sprite { get; set; }
}
