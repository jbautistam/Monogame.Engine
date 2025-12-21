using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Managers.Input;
using Bau.Libraries.BauGame.Engine.Managers.Input.Builders;

namespace EngineSample.Core;

/// <summary>
///		Clase principal del juego
/// </summary>
public class EngineSampleGame : Game
{
	public EngineSampleGame()
	{
		// Instancia el motor del juego
		GameEngine.Instantiate(this,
							   new Bau.Libraries.BauGame.Engine.Configuration.EngineSettings
									{
										FullScreen = false,
										WindowBorderless = false,
										ScreenBufferWidth = 1_200,
										ScreenBufferHeight = 720,
										ViewPortWidth = 1_200,
										ViewPortHeight = 720,
										ContentRoot = "Content",
										DisplayOrientation = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
										MainAssembly = System.Reflection.Assembly.GetExecutingAssembly(),
										ResourceFolder = "EngineSample.Core.Localization.Resources",
									}
							  );

		// Inicializa el motor
		GameEngine.Instance.Initialize();
	}

	/// <summary>
	///		Inicializa el juego incluyendo la configuración de la localización y añadiendo las pantallas iniciales al ScreenManager.
	/// </summary>
	protected override void Initialize()
	{
		// Inicializa el juego
		base.Initialize();
		// Indica que está en modo de depuración
		GameEngine.Instance.EngineSettings.DebugMode = false;
		GameEngine.Instance.EngineSettings.DebugFont = "Fonts/Hud";
		GameEngine.Instance.EngineSettings.DebugColor = Color.White;
		// Añade las escenas
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.MainMenu.MainMenuScene(GameLogic.Scenes.MainMenu.MainMenuScene.SceneName));
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.Games.GameScene(GameLogic.Scenes.Games.GameScene.SceneName));
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.TilesSample.TilesScene(GameLogic.Scenes.TilesSample.TilesScene.SceneName, 1));
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.Space.SpaceShipsScene(GameLogic.Scenes.Space.SpaceShipsScene.SceneName));
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.GraphicNovel.GraphicNovelScene(GameLogic.Scenes.GraphicNovel.GraphicNovelScene.SceneName));
		// Prepara la escena
		GameEngine.Instance.SceneManager.ChangeScene("MainMenu");
		// Prepara los mapeos
		CreateMappings(GameEngine.Instance.InputManager);
	}

	/// <summary>
	///		Carga el contenido del juego como texturas y sistemas de partículas
	/// </summary>
	protected override void LoadContent()
	{
		// Inicializa las texturas y animaciones
		new Configuration.ConfigurationLoader().LoadTexturesSettings();
		// Llama al método base
		base.LoadContent();
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
						.WithMouse(InputMappings.Status.JustPressed, Bau.Libraries.BauGame.Engine.Managers.Input.MouseController.MouseStatus.MouseButton.Left)
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
	///		Actualiza la lógica del juego. Se le llama una vez por frame
	/// </summary>
	protected override void Update(GameTime gameTime)
	{
		// Actualiza los controladores del motor
		GameEngine.Instance.Update(gameTime);
		// Llama al método base de modificación
		base.Update(gameTime);
	}

	/// <summary>
	///		Dibuja los gráficos del juego. Se le llama una vez por frame
	/// </summary>
	protected override void Draw(GameTime gameTime)
	{
		// Dibuja la escena actual
		GameEngine.Instance.Draw(gameTime);
		// Llama al método base de dibujo
		base.Draw(gameTime);
	}
}