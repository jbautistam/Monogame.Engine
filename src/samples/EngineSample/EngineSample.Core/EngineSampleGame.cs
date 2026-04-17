using Microsoft.Xna.Framework.Input;
using Bau.BauEngine;
using Bau.BauEngine.Managers.Input;
using Bau.BauEngine.Managers.Input.Builders;
using Bau.BauEngine.Scenes;

namespace EngineSample.Core;

/// <summary>
///		Clase principal del juego
/// </summary>
public class EngineSampleGame : BauEngineGame
{
	public EngineSampleGame() : base("Content")
	{
	}

	/// <summary>
	///		Inicializa el juego incluyendo la configuración de la localización y añadiendo las pantallas iniciales al ScreenManager.
	/// </summary>
	protected override void InitializeGame()
	{
		// Carga la configuración del sistema
		EngineManager.MonogameServicesManager.Configure(new Configuration.ResourcesLoader(EngineManager).LoadConfiguration());
		// Carga la configuración de recursos
		new Configuration.ResourcesLoader(EngineManager).LoadResourcesSettings();
		// Prepara la escena
		EngineManager.SceneManager.ChangeScene("MainMenu", new Bau.BauEngine.Managers.GameContext());
		// Prepara los mapeos
		CreateMappings(EngineManager.InputManager);
	}

	/// <summary>
	///		Carga el contenido del juego como texturas y sistemas de partículas
	/// </summary>
	protected override void LoadContentGame()
	{
	}

	/// <summary>
	///		Crea los mapeos
	/// </summary>
	private void CreateMappings(InputManager inputManager)
	{
		InputMappingsBuilder builder = new();

			// Genera los mapeos
			builder.WithAction(InputMappings.DefaultIntroAction, true)
						.WithKeyboard(InputMappings.Status.JustPressed, Keys.Enter)
				   .WithAction(InputMappings.DefaultMouseClickAction, true)
						.WithMouse(InputMappings.Status.JustPressed, Bau.BauEngine.Managers.Input.MouseController.MouseStatus.MouseButton.Left)
				   .WithAction(InputMappings.DefaulQuitAction, true)
						.WithKeyboard(InputMappings.Status.JustPressed, Keys.Escape)
				   .WithAction(InputMappings.DefaultActionUp, false)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.Up)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.NumPad8)
				   .WithAction(InputMappings.DefaultActionDown, false)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.Down)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.NumPad2)
				   .WithAction(InputMappings.DefaultActionLeft, false)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.Left)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.NumPad4)
				   .WithAction(InputMappings.DefaultActionRight, false)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.Right)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.NumPad6)
					.WithAction(GameLogic.Constants.InputShootAction, false)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.Space)
					.WithAction(GameLogic.Constants.InputShootGrenadeAction, false)
						.WithKeyboard(InputMappings.Status.Pressed, Keys.G);
			// Añade los mapeos al sistema
			inputManager.Mappings.AddRange(builder.Build());
	}

	/// <summary>
	///		Obtiene una escena
	/// </summary>
	public override AbstractScene GetScene(string scene)
	{
		// Busca la escena adecuada
		if (!string.IsNullOrWhiteSpace(scene))
		{
			if (scene.Equals(GameLogic.Scenes.Games.GameScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.Games.GameScene(EngineManager.SceneManager, GameLogic.Scenes.Games.GameScene.SceneName);
			else if (scene.Equals(GameLogic.Scenes.TilesSample.TilesScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.TilesSample.TilesScene(EngineManager.SceneManager, GameLogic.Scenes.TilesSample.TilesScene.SceneName, 1);
			else if (scene.Equals(GameLogic.Scenes.Particles.ParticlesScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.Particles.ParticlesScene(EngineManager.SceneManager, GameLogic.Scenes.Particles.ParticlesScene.SceneName);
			else if (scene.Equals(GameLogic.Scenes.Space.SpaceShipsScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.Space.SpaceShipsScene(EngineManager.SceneManager, GameLogic.Scenes.Space.SpaceShipsScene.SceneName);
			else if (scene.Equals(GameLogic.Scenes.Animations.AnimationsScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.Animations.AnimationsScene(EngineManager.SceneManager, GameLogic.Scenes.Animations.AnimationsScene.SceneName);
			else if (scene.Equals(GameLogic.Scenes.UserInterfaceGridTest.UserInterfaceGridScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.UserInterfaceGridTest.UserInterfaceGridScene(EngineManager.SceneManager, GameLogic.Scenes.UserInterfaceGridTest.UserInterfaceGridScene.SceneName);
			else if (scene.Equals(GameLogic.Scenes.UserInterfaceGalleryTest.UserInterfaceGalleryScene.SceneName, StringComparison.CurrentCultureIgnoreCase))
				return new GameLogic.Scenes.UserInterfaceGalleryTest.UserInterfaceGalleryScene(EngineManager.SceneManager, GameLogic.Scenes.UserInterfaceGalleryTest.UserInterfaceGalleryScene.SceneName);
		}
		// Si ha llegado hasta aquí es porque no hay ninguna escena con ese nombre y devuelve la principal
		return new GameLogic.Scenes.MainMenu.MainMenuScene(EngineManager.SceneManager, GameLogic.Scenes.MainMenu.MainMenuScene.SceneName);
	}  
}