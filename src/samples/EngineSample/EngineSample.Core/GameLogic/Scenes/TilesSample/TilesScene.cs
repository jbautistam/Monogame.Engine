using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine;

namespace EngineSample.Core.GameLogic.Scenes.TilesSample;

/// <summary>
///		Escena de la partida
/// </summary>
internal class TilesScene(string name, int level) : AbstractScene(name, new Rectangle(0, 0, 5_000, 5_000))
{
	// Constantes públicas
	public const string SceneName = "Tiles";
	public const int PhysicsPlayerLayer = 1;
	public const int PhysicsPlayerProjectileLayer = 2;
	public const int PhysicsNpcLayer = 3;
	public const int PhysicsNpcProjectileLayer = 4;
	public const int PhysicsBackgroundLayer = 4;

	// Variables privadas
	private Common.HudLayer? _hudLayer;

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Guarda el interface de usuario Hud
		_hudLayer = new Common.HudLayer(this, Constants.LayerHud, 1);
		LayerManager.AddLayer(_hudLayer);
		// Añade la capa
		CreateGameLayers();
		// Crea los datos de físicas
		CreatePhysics();
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Crea las capas de partida
	/// </summary>
	private void CreateGameLayers()
	{
		// Añade la capa de fondo tiles
		LayerManager.AddLayer(CreateTileMapLayer());
		// Añade la capa del jugador
		LayerManager.AddLayer(new TilesPlayerLayer(this, SceneName, 1));

		// Crea la capa de fondo
		TileMapLayer CreateTileMapLayer()
		{
			TileMapLayer layer = new(this, "TileMap", PhysicsBackgroundLayer, 0);

				// Inicializa la capa (carga el mapa)
				layer.Initialize();
				// Devuelve la capa
				return layer;
		}
	}

	/// <summary>
	///		Crea los datos de las capas de físicas
	/// </summary>
	private void CreatePhysics()
	{
		PhysicsManager.LayersRelations.AddRelation(PhysicsPlayerLayer, PhysicsNpcLayer);
		PhysicsManager.LayersRelations.AddRelation(PhysicsPlayerLayer, PhysicsNpcProjectileLayer);
		PhysicsManager.LayersRelations.AddRelation(PhysicsNpcLayer, PhysicsPlayerProjectileLayer);
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores y el interface de usuario
			LayerManager.Update(gameContext);
			// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
			if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaulQuitAction))
				nextScene = GameEngine.Instance.SceneManager.GetScene(MainMenu.MainMenuScene.SceneName) ?? this;
			// Devuelve la nueva escena
			return nextScene;
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}

	/// <summary>
	///		Nivel de la escena
	/// </summary>
	public int Level { get; } = level;
}
