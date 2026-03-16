using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Renderers;

/// <summary>
///		Clase de presentación para textos
/// </summary>
public class TextRenderer(SpriteBatchController spriteBatchController)
{
	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
	{
		if (SpriteBatchController.SpriteBatch is not null)
			SpriteBatchController.SpriteBatch.DrawString(font, text, position, color);
	}

	/// <summary>
	///		Controlador de dibujo principal
	/// </summary>
	public SpriteBatchController SpriteBatchController { get; } = spriteBatchController;
}
