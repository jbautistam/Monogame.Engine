using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

/// <summary>
///     Clase para dibujo de un <see cref="SpriteTextDefinition"/>
/// </summary>
public class SpriteTexttRenderer(SpriteTextDefinition sprite)
{
	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(Camera2D camera, string text, Vector2 position, Color color)
	{
		SpriteFont? spriteFont = Sprite.LoadAsset();

			if (spriteFont is not null)
				camera.SpriteBatchController.DrawString(spriteFont, text, position, color);
	}

    /// <summary>
    ///     Definición de la fuente a dibujar
    /// </summary>
    public SpriteTextDefinition Sprite { get; } = sprite;
}