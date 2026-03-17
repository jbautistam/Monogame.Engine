using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

/// <summary>
///     Textura de nueve partes para botones
/// </summary>
public class NineSliceTexture(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
	/// <summary>
	///		Obtiene la región de la textura
	/// </summary>
	public override TextureRegion? GetRegion(string? name)
	{
		Texture2D? texture = GetTexture();

			// Obtiene la textura
			if (texture is not null)
				return new TextureRegion(this, NormalizeName(name))
								{
									Region = new Rectangle(0, 0, texture.Width, texture.Height)
								};
			else
				return null;
	}

    /// <summary>
    ///     Tamaño de sección a la izquierda
    /// </summary>
    public int LeftSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección a la derecha
    /// </summary>
    public int RightSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección desde arriba
    /// </summary>
    public int TopSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección desde abajo
    /// </summary>
    public int BottomSlice { get; set; }
}