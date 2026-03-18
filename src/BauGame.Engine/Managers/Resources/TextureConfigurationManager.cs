using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Managers.Resources.Textures.Configuration;
using Bau.BauEngine.Scenes.Cameras;

namespace Bau.BauEngine.Managers.Resources;

/// <summary>
///		Manager para configuración de texturas
/// </summary>
public class TextureConfigurationManager(ResourcesManager resourcesManager)
{
	// Registros públicos
	public record TextureResolved(Texture2D Texture, Rectangle Region, UiMargin? Padding, TextureRegionNineSliceConfiguration? NineSliceConfiguration);

	/// <summary>
	///		Crea una textura
	/// </summary>
	public TextureConfiguration Create(string name, string asset)
	{
		TextureConfiguration texture = new(name, asset);

			// Añade la textura al diccionario
			Assets.Add(name, texture);
			// y devuelve la textura creada
			return texture;
	}

	/// <summary>
	///		Crea una textura de tipo Atlas
	/// </summary>
	public void Create(string name, string asset, int rows, int columns)
	{
		TextureConfiguration texture = new(name, asset);

			// Crea las regiones
            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
				{
					TextureRegionAtlasConfiguration region = new(row, column, rows, columns);

						// Añade la región
						texture.Regions.Add(region.Name, region);
				}
			// Añade la textura al diccionario
			Assets.Add(name, texture);
	}

	/// <summary>
	///		Obtiene la textura y región asociada
	/// </summary>
	public TextureResolved? GetTextureRegion(Camera2D camera, string name, string? region) => GetTextureRegion(camera.Scene, name, region);

	/// <summary>
	///		Obtiene la textura y región asociada
	/// </summary>
	public TextureResolved? GetTextureRegion(Scenes.AbstractScene scene, string name, string? region)
	{
		TextureConfiguration? configuration = Assets.Get(name);

			// Si tenemos una configuración con ese nombre
			if (configuration is not null)
			{
				Texture2D? texture = scene.LoadSceneAsset<Texture2D>(configuration.Asset);

					if (texture is not null)
					{
						if (configuration.Regions.Items.Count == 0)
							return new TextureResolved(texture, new Rectangle(0, 0, texture.Width, texture.Height), null, null);
						else if (!string.IsNullOrWhiteSpace(region))
						{
							TextureAbstractRegionConfiguration? textureRegion = configuration.Regions.Get(region);

								if (textureRegion is not null)
								{
									if (textureRegion is TextureRegionRectangleConfiguration rectangleConfiguration)
										return new TextureResolved(texture, rectangleConfiguration.GetBounds(texture), rectangleConfiguration.Padding, rectangleConfiguration.NineSliceConfiguration);
									else
										return new TextureResolved(texture, textureRegion.GetBounds(texture), null, null);
								}
						}
					}
			}
			else
			{
				Texture2D? texture = scene.LoadSceneAsset<Texture2D>(name);

					if (texture is not null)
						return new TextureResolved(texture, new Rectangle(0, 0, texture.Width, texture.Height), null, null);
			}
			// Si hemos llegado hasta aquí es porque no tenemos todos los datos
			return null;
	}

	/// <summary>
	///		Manager principal
	/// </summary>
	public ResourcesManager ResourcesManager { get; } = resourcesManager;

	/// <summary>
	///		Sprite sheets
	/// </summary>
	public Entities.Common.DictionaryModel<TextureConfiguration> Assets { get; } = new();
}