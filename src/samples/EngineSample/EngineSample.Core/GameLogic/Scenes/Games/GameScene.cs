using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

namespace EngineSample.Core.GameLogic.Scenes.Games;

/// <summary>
///		Escena de la partida
/// </summary>
internal class GameScene(string name) : AbstractScene(name, new Bau.Libraries.BauGame.Engine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "Game";
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
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "background", "bg-layer-0", null, 1));
		LayerManager.AddLayer(new GameLayer(this, SceneName, 1));
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
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores y el interface de usuario
			LayerManager.Update(gameContext);
			// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
			if (Bau.Libraries.BauGame.Engine.GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaulQuitAction))
				nextScene = Bau.Libraries.BauGame.Engine.GameEngine.Instance.SceneManager.GetScene(MainMenu.MainMenuScene.SceneName) ?? this;
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
