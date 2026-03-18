using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Layers.Backgrounds;
using Bau.BauEngine;

namespace EngineSample.Core.GameLogic.Scenes.UserInterfaceGridTest;

/// <summary>
///		Escena de la partida
/// </summary>
internal class UserInterfaceGridScene(string name) : AbstractScene(name, new Bau.BauEngine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = nameof(UserInterfaceGridScene);

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Añade la capa
		LayerManager.Clear();
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "Background", "bg-layer-1", null, 1));
		LayerManager.AddLayer(new UserInterfaceGridLayer(this, SceneName, 1));
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
			if (GameEngine.Instance.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
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
