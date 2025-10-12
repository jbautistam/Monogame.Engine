using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Managers.Resources;

/// <summary>
///		Manager para "texturas
/// </summary>
public class TextureManager(ResourcesManager resourcesManager)
{
	/// <summary>
	///		Crea una textura
	/// </summary>
	public void Create(string id, string asset)
	{
		Assets.Add(id, new Textures.TextureFull(this, id, asset));
	}

	/// <summary>
	///		Crea un sprite sheet indicando las filas y columnas
	/// </summary>
	public void Create(string id, string asset, int rows, int columns)
	{
		Textures.TextureAtlas texture = new(this, id, asset)
											{
												Rows = rows,
												Columns = columns
											};

			// Añade la textura a la colección
			Assets.Add(id, texture);
	}

	/// <summary>
	///		Crea un sprite sheet indicando las regiones
	/// </summary>
	public void Create(string id, string asset, List<(string name, Rectangle region)> regions)
	{
		Textures.TextureSpriteSheet texture = new(this, id, asset);

			// Añade las regiones
			texture.Regions.AddRange(regions);
			// Añade el spritesheet a la colección
			Assets.Add(id, texture);
	}

	/// <summary>
	///		Manager principal
	/// </summary>
	public ResourcesManager ResourcesManager { get; } = resourcesManager;

	/// <summary>
	///		Sprite sheets
	/// </summary>
	public Base.DictionaryModel<Textures.AbstractTexture> Assets { get; } = new();
}