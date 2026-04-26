using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Layers.Backgrounds;

namespace EngineSample.Core.GameLogic.Scenes.Animations;

/// <summary>
///		Escena de la partida
/// </summary>
internal class AnimationsScene(SceneManager sceneManager) : AbstractScene(sceneManager, SceneName, new Bau.BauEngine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "Animations";

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Añade la capa
		LayerManager.Clear();
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "Background", "bg-layer-1", null, 1));
		LayerManager.AddLayer(new AnimationsLayer(this, SceneName, 1));
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override NextSceneContextModel? UpdateScene(Bau.BauEngine.Managers.GameContext gameContext)
	{
		// Actualiza los actores y el interface de usuario
		LayerManager.Update(gameContext);
		// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
		if (SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
			return new NextSceneContextModel(MainMenu.MainMenuScene.SceneName);
		else
			return null;
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
