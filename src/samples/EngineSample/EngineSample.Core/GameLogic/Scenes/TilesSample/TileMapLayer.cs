using Bau.Libraries.BauGame.Engine.Actors.TileMap;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Scenes.TilesSample;

/// <summary>
///		Capa con el mapa de fondo
/// </summary>
internal class TileMapLayer(AbstractScene scene, string name, int physicsLayer, int sortOrder) : AbstractLayer(scene, name, LayerType.Game, sortOrder)
{
	// Variables privadas
	private TileMapActor? _mapActor;

	/// <summary>
	///		Inicializa la capa
	/// </summary>
	internal void Initialize()
	{
		int tileId = 0;

			// Crea el actor
			_mapActor = new TileMapActor(this, PhysicsLayer, 0);
			// Lo añade a la capa
			Actors.Add(_mapActor);
			// Añade las definiciones
			_mapActor.AddDefinition(1, "BlockA0", "BlockA0", null);
			_mapActor.AddDefinition(2, "BlockA1", "BlockA1", null);
			_mapActor.AddDefinition(3, "BlockA2", "BlockA2", null);
			_mapActor.AddDefinition(4, "BlockA3", "BlockA3", null);
			_mapActor.AddDefinition(5, "BlockA4", "BlockA4", null);
			_mapActor.AddDefinition(6, "BlockA5", "BlockA5", null);
			_mapActor.AddDefinition(7, "BlockA5", "BlockA6", null);
			_mapActor.AddDefinition(8, "BlockB0", "BlockB0", null);
			_mapActor.AddDefinition(9, "BlockB1", "BlockB1", null);
			_mapActor.AddDefinition(10, "Platform", "Platform", null);
			_mapActor.AddDefinition(11, "Exit", "Exit", null);
			// Añade los elementos del mapa
			for (int x = 500; x < 1000; x += 40)
				for (int y = 500; y < 1000; y += 32)
					_mapActor.AddTile(++tileId % 11 + 1, x, y, true);
	}

	/// <summary>
	///		Arranca la capa
	/// </summary>
	protected override void StartLayer()
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
	protected override void UpdatePhysicsLayer(GameContext gameContext)
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Actualiza la capa
	/// </summary>
	protected override void UpdateLayer(GameContext gameContext)
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	protected override void DrawLayer(Camera2D camera, GameContext gameContext)
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndLayer()
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Identificador de la capa de físicas
	/// </summary>
	private int PhysicsLayer { get; } = physicsLayer;
}