using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;
using Bau.Libraries.BauGame.Engine;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.Director.UserInterfaceTest;

/// <summary>
///		Escena de la partida
/// </summary>
internal class UserInterfaceScene(string name) : AbstractScene(name, new Bau.Libraries.BauGame.Engine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "UserInterfaceDirectorTest";
	private const string UserInterfaceCamera = nameof(UserInterfaceCamera);
	private const string WorldCamera = nameof(WorldCamera);

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Crea las cámaras
		CameraDirector.CreateWorldCamera(WorldCamera, Vector2.Zero, 0);
		CameraDirector.CreateScreenCamera(UserInterfaceCamera, Vector2.Zero, 1);
		// Añade las capas
		LayerManager.Clear();
		CreateBackgroundLayer();
		CreateUserInterfaceLayer();
		// Arranca las capas
		LayerManager.Start();

		// Crea la capa de fondo
		void CreateBackgroundLayer()
		{
			AbstractLayer layer = LayerManager.AddLayer(new FixedBackgroundLayer(this, "Background", "bg-layer-1", 1));

				// Añade la cámara a la capa
				layer.Cameras.Add(WorldCamera);
		}

		// Crea la capa de interface de usuario
		void CreateUserInterfaceLayer()
		{
			AbstractLayer layer = LayerManager.AddLayer(new UserInterfaceLayer(this, SceneName, 1));

				// Añade la cámara a la capa
				layer.Cameras.Add(UserInterfaceCamera);
		}
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
