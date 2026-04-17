using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Layers.Backgrounds;
using Bau.BauEngine;

namespace EngineSample.Core.GameLogic.Scenes.Space;

/// <summary>
///		Escena con naves espaciales
/// </summary>
internal class SpaceShipsScene(SceneManager sceneManager, string name) : AbstractScene(sceneManager, name, new Bau.BauEngine.Entities.Common.WorldDefinitionModel(5_000, 5_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "SpaceShips";
	public const int PhysicsPlayerLayer = 1;
	public const int PhysicsPlayerProjectileLayer = 2;
	public const int PhysicsNpcLayer = 3;
	public const int PhysicsNpcProjectileLayer = 4;
	public const int PhysicsPowerUpLayer = 5;

	// Variables privadas
	private Common.HudLayer? _hudLayer;

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Guarda el interface de usuario Hud
		_hudLayer = new Common.HudLayer(this, Constants.LayerHud, 1);
		LayerManager.Clear();
		LayerManager.AddLayer(_hudLayer);
		// Añade la capa
		LayerManager.AddLayer(new Layers.SpaceShipGameLayer(this, SceneName, 1));
		// Crea las capas con los fondos
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "fixed-bg", "bg-space-04", null, 1));
		LayerManager.AddLayer(new ParallaxBackgroundLayer(this, "parallax-bg-01", "parallax-01", null, 2, 0.1f));
		LayerManager.AddLayer(new ParallaxBackgroundLayer(this, "parallax-bg-02", "parallax-02", null, 3, 0.7f));
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
		PhysicsManager.LayersRelations.AddRelation(PhysicsPlayerLayer, PhysicsPowerUpLayer);
		PhysicsManager.LayersRelations.AddRelation(PhysicsNpcLayer, PhysicsPlayerProjectileLayer);
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override string? UpdateScene(Bau.BauEngine.Managers.GameContext gameContext)
	{
		// Actualiza los actores y el interface de usuario
		LayerManager.Update(gameContext);
		// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
		if (SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
			return MainMenu.MainMenuScene.SceneName;
		else
			return string.Empty;
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
