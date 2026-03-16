using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Renderers;

/// <summary>
///     Clase para dibujo de un <see cref="SpriteTextDefinition"/>
/// </summary>
public class SpriteTextRenderer(SpriteBatchController spriteBatchController)
{
	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteTextDefinition sprite, string text, Vector2 position, Color color)
	{
		SpriteFont? spriteFont = sprite.LoadAsset();

			if (spriteFont is not null && SpriteBatchController.SpriteBatch is not null)
				SpriteBatchController.SpriteBatch.DrawString(spriteFont, text, position, color);
	}

    /// <summary>
    ///     Controlador de sprites
    /// </summary>
    public SpriteBatchController SpriteBatchController { get; } = spriteBatchController;
}