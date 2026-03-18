using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Managers.Resources.Textures;

/// <summary>
///		Región de una textura
/// </summary>
public class TextureRegion(AbstractTexture texture, string name)
{
	/// <summary>
	///		Dibuja la textura en una posición con escala
	/// </summary>
	public void Draw(Scenes.Rendering.RenderingManager renderingManager, Vector2 position, Vector2 origin, Vector2 scale, SpriteEffects effects, 
					 Color color, float rotation)
	{
		SpriteDefinition sprite = new(Texture.Id, Name)
										{
											SpriteEffect = effects
										};

			// Dibuja el sprite
			renderingManager.SpriteRenderer.Draw(sprite, position, origin, scale, rotation, color);
	}

	/// <summary>
	///		Dibuja la textura en un rectángulo concreto (ajusta al ancho y alto del rectángulo)
	/// </summary>
	public void Draw(Scenes.Rendering.RenderingManager renderingManager, Rectangle destination, Vector2 origin, SpriteEffects effects, Color color, float rotation)
	{
		SpriteDefinition sprite = new(Texture.Id, Name)
										{
											SpriteEffect = effects
										};

			// Dibuja el sprite
			renderingManager.SpriteRenderer.Draw(sprite, destination, origin, rotation, color);
	}

	/// <summary>
	///		Textura
	/// </summary>
	public AbstractTexture Texture { get; } = texture;

	/// <summary>
	///		Identificador de la región
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Región
	/// </summary>
	public required Rectangle Region { get; init; }

	/// <summary>
	///		Padding para diujo de texto
	/// </summary>
	public Entities.UserInterface.UiMargin? Padding { get; set; }
}