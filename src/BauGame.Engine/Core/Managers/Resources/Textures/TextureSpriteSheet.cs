using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Core.Managers.Resources.Textures;

/// <summary>
///     Definición de un sprite sheet sobre una textura
/// </summary>
public class TextureSpriteSheet(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
    /// <summary>
    ///     Obtiene una región
    /// </summary>
	public override TextureRegion? GetRegion(string name)
	{
        Texture2D? texture = GetTexture();

            if (texture is not null)
                return new TextureRegion(name)
                                {
                                    Texture = texture,
                                    Region = Regions.Get(name)
                                };
            else
                return null;
	}

    /// <summary>
    ///     Regiones
    /// </summary>
    public Base.DictionaryModel<Rectangle> Regions { get; } = new();
}