using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Managers.Resources.Textures;

/// <summary>
///     Definición de un sprite sheet sobre una textura
/// </summary>
public class TextureSpriteSheet(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
    /// <summary>
    ///     Obtiene una región
    /// </summary>
	public override TextureRegion? GetRegion(string? name)
	{
        Texture2D? texture = GetTexture();

            if (texture is not null)
                return new TextureRegion(this, NormalizeName(name))
                                {
                                    Region = Regions.Get(NormalizeName(name))
                                };
            else
                return null;
	}

	/// <summary>
	///     Regiones
	/// </summary>
	public Entities.Common.DictionaryModel<Rectangle> Regions { get; } = new();
}