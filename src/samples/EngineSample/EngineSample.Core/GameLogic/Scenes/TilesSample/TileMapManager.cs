using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using EngineSample.Core.GameLogic.Scenes.TilesSample.Loaders.TmxFile;

namespace EngineSample.Core.GameLogic.Scenes.TilesSample;

/// <summary>
///		Manager para la creación de las capas de tilemap
/// </summary>
internal class TileMapManager(TilesScene scene)
{
	/// <summary>
	///		Crea las capas que controlan el mapa
	/// </summary>
	internal List<AbstractLayer> CreateTileMapLayers(int level)
	{
		List<AbstractLayer> layers = [];
		MapModel? map = LoadMap(level);

			// Crea las capas del mapa
			if (map is not null)
			{
				layers.Add(Create("TileMap", TilesScene.PhysicsBackgroundLayer, 1));
			}
			// Devuelve las capas
			return layers;
	}

	/// <summary>
	///		Crea una capa del mapa
	/// </summary>
	private TileMapLayer Create(string name, int physicsLayer, int sortOrder)
	{
		return new TileMapLayer(Scene, name, physicsLayer, sortOrder);
	}

	/// <summary>
	///		Carga la definíción de mapa
	/// </summary>
	private MapModel? LoadMap(int level)
	{
		string? xml = GameEngine.Instance.FilesManager.StorageManager.ReadTextFile($"Content/Settings/Maps/Map_{level:000}.tmx");
		MapModel? map = null;

			// Interpreta el XML leido
			if (!string.IsNullOrWhiteSpace(xml))
				map = new TmxFileRepository().Load(xml);
			// Devuelve el mapa leido
			return map;
	}

	/// <summary>
	///		Escena
	/// </summary>
	internal TilesScene Scene { get; } = scene;
}
