using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;
using Bau.Libraries.BauGame.Engine;

namespace EngineSample.Core.GameLogic.Scenes.Space;

/// <summary>
///		Escena con naves espaciales
/// </summary>
internal class SpaceShipsScene(string name) : AbstractScene(name, new Bau.Libraries.BauGame.Engine.Models.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "SpaceShips";
	public const int PhysicsPlayerLayer = 1;
	public const int PhysicsPlayerProjectileLayer = 2;
	public const int PhysicsNpcLayer = 3;
	public const int PhysicsNpcProjectileLayer = 4;

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
		LayerManager.AddLayer(new Layers.SpaceShipGameLayer(this, SceneName, 1));
		LayerManager.AddLayer(CreateBackgroundLayer());
		// Crea los datos de físicas
		CreatePhysics();
		// Arranca las capas
		LayerManager.Start();
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
	///		Crea la capa con los fondos
	/// </summary>
	private BackgroundLayer CreateBackgroundLayer()
	{
		BackgroundLayer layer = new(this, "Background", 1);

			// Añade los fondos
			layer.BackgroundLayers.Add(new FixedBackgroundLayer("bg-space-04", 1));
			layer.BackgroundLayers.Add(new ParallaxBackgroundLayer("parallax-01", 2, 0.1f));
			layer.BackgroundLayers.Add(new ParallaxBackgroundLayer("parallax-02", 3, 0.7f));
			// Devuelve la capa creada
			return layer;
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
}
