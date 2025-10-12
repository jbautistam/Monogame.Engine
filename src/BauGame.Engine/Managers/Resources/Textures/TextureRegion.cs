using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

/// <summary>
///		Región de una textura
/// </summary>
public class TextureRegion(string name)
{
	/// <summary>
	///		Dibuja la textura
	/// </summary>
	public void Draw(Scenes.Cameras.Camera2D camera, Vector2 position, Vector2 origin, Vector2 scale, SpriteEffects spriteEffect, Color color, float rotation)
	{
		camera.SpriteBatchController.Draw(Texture, position, Region, origin, scale, spriteEffect, color, rotation, 1);
	}

	/// <summary>
	///		Identificador
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Textura
	/// </summary>
	public required Texture2D Texture { get; init; }

	/// <summary>
	///		Región
	/// </summary>
	public required Rectangle Region { get; init; }
}