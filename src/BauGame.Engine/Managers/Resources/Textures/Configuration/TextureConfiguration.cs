using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures.Configuration;

/// <summary>
///		Configuración para una textura
/// </summary>
public class TextureConfiguration(string name, string asset)
{
	/// <summary>
	///		Obtiene los límites de una región dentro de la textura
	/// </summary>
	public Rectangle GetBounds(Texture2D texture, string region)
	{
		if (Regions.Items.Count == 0) // ... no se han definido regiones, se devuelve la textura entera
			return new Rectangle(0, 0, texture.Width, texture.Height);
		else
		{
			TextureAbstractRegionConfiguration? regionConfiguration = Regions.Get(region);

				// Devuelve el rectángulo original
				if (regionConfiguration is not null)
					return regionConfiguration.GetBounds(texture);
				else
					return new Rectangle();
		}
	}

	/// <summary>
	///		Nombre de la textura
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Nombre del archivo de textura en el directorio de contenido
	/// </summary>
	public string Asset { get; } = asset;

	/// <summary>
	///		Regiones de la textura
	/// </summary>
	public Entities.Common.DictionaryModel<TextureAbstractRegionConfiguration> Regions { get; } = new();
}
