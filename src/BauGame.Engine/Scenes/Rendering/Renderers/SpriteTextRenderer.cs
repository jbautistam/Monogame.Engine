using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Renderers;

/// <summary>
///     Clase para dibujo de un <see cref="SpriteTextDefinition"/>
/// </summary>
public class SpriteTextRenderer(RenderingManager renderingManager)
{
	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteTextDefinition? sprite, string text, Vector2 position, Color color)
	{
		SpriteFont? spriteFont = sprite?.LoadAsset();

			if (spriteFont is not null && RenderingManager.SpriteBatch is not null)
				RenderingManager.SpriteBatch.DrawString(spriteFont, text, position, color);
	}

    /// <summary>
    ///     Manager de representación
    /// </summary>
    public RenderingManager RenderingManager { get; } = renderingManager;
}