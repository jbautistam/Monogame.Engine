using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.TilesMap;

namespace EngineSample.Core.GameLogic.Scenes.TilesSample;

/// <summary>
///		Capa con el mapa de fondo
/// </summary>
internal class TileMapLayer(AbstractScene scene, string name, int physicsLayer, int sortOrder) : AbstractTilemapLayer(scene, name, physicsLayer, sortOrder)
{
	/// <summary>
	///		Arranca la capa
	/// </summary>
	protected override void StartLayer()
	{
		LoadTiles();
	}

	/// <summary>
	///		Carga la definíción de patrones
	/// </summary>
	private void LoadTiles()
	{
		List<TileDefinition>? tiles = GameEngine.Instance.FilesManager.StorageManager.LoadJsonData<List<TileDefinition>>("Content/Settings/tilesets.json");

			if (tiles is not null)
				Tiles.AddRange(tiles);
	}
}