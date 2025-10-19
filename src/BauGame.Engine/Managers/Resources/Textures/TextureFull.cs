using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

/// <summary>
///		Textura completa
/// </summary>
public class TextureFull(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
	/// <summary>
	///		Obtiene la región de la textura
	/// </summary>
	public override TextureRegion? GetRegion(string? name)
	{
		Texture2D? texture = GetTexture();

			// Obtiene la textura
			if (texture is not null)
				return new TextureRegion(NormalizeName(name))
								{
									Texture = texture,
									Region = new Microsoft.Xna.Framework.Rectangle(0, 0, texture.Width, texture.Height)
								};
			else
				return null;
	}
}
