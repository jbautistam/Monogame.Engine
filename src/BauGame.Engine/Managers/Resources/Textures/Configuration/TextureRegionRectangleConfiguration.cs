using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures.Configuration;

/// <summary>
///		Configuración de una región para una textura
/// </summary>
public class TextureRegionRectangleConfiguration(string name) : TextureAbstractRegionConfiguration(name)
{
	/// <summary>
	///		Obtiene el rectángulo donde se encuentra la región a dibujar
	/// </summary>
	public override Rectangle GetBounds(Microsoft.Xna.Framework.Graphics.Texture2D texture) => Region;

	/// <summary>
	///		Región
	/// </summary>
	public required Rectangle Region { get; init; }

	/// <summary>
	///		Configuración para una textura NineSlice
	/// </summary>
	public TextureRegionNineSliceConfiguration? NineSliceConfiguration { get; set; }
}