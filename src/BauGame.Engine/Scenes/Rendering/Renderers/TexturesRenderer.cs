using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Renderers;

/// <summary>
///		Clase de presentación para textures
/// </summary>
public class TexturesRenderer(RenderingManager renderingManager)
{
	/// <summary>
	///		Dibuja una textura
	/// </summary>
	public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Vector2 origin, Vector2 scale, SpriteEffects spriteEffect, 
					 Color color, float rotation, int layerDepth = 0)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(texture, position, source, color, rotation, origin, scale, spriteEffect, layerDepth);
	}

	/// <summary>
	///		Dibuja una textura
	/// </summary>
	public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Vector2 origin, float scale, SpriteEffects spriteEffect, 
					 Color color, float rotation, int layerDepth = 0)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(texture, position, source, color, rotation, origin, scale, spriteEffect, layerDepth);
	}

	/// <summary>
	///		Dibuja una textura escalada a un rectángulo
	/// </summary>
	public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(texture, destinationRectangle, color);
	}

	/// <summary>
	///		Dibuja una textura
	/// </summary>
	public void Draw(Texture2D texture, Rectangle destination, Rectangle? source, Vector2 origin, Color color, float rotation, 
					 SpriteEffects spriteEffect, float layerDepth = 0)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(texture, destination, source, color, rotation, origin, spriteEffect, layerDepth);
	}

	/// <summary>
	///		Dibuja una textura
	/// </summary>
	public void Draw(Texture2D texture, Rectangle destination, Rectangle source, Color color)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(texture, destination, source, color);
	}

	/// <summary>
	///		Dibuja una textura en una posición
	/// </summary>
	public void Draw(Texture2D texture, Vector2 position, Color color)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(texture, position, color);
	}

	/// <summary>
	///		Manager de presentación
	/// </summary>
	public RenderingManager RenderingManager { get; } = renderingManager;
}