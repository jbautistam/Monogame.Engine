using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Layers.Backgrounds;

namespace EngineSample.Core.GameLogic.Scenes.Particles;

/// <summary>
///		Escena de la partida
/// </summary>
internal class ParticlesScene(string name) : AbstractScene(name, new Bau.BauEngine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "Particles";

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
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "background", "bg-layer-5", null, 1));
		LayerManager.AddLayer(new ParticlesLayer(this, SceneName, 1));
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(Bau.BauEngine.Managers.GameContext gameContext)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores y el interface de usuario
			LayerManager.Update(gameContext);
			// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
			if (Bau.BauEngine.GameEngine.Instance.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
				nextScene = Bau.BauEngine.GameEngine.Instance.SceneManager.GetScene(MainMenu.MainMenuScene.SceneName) ?? this;
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
