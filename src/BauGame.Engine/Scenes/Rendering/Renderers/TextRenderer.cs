using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Renderers;

/// <summary>
///		Clase de presentación para textos
/// </summary>
public class TextRenderer(RenderingManager renderingManager)
{
	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.DrawString(font, text, position, color);
	}

	/// <summary>
	///		Manager de presentación
	/// </summary>
	public RenderingManager RenderingManager { get; } = renderingManager;
}
