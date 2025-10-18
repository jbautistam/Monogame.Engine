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
										ScreenWidth = 1_024,
										ScreenHeight = 800,
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
		GameEngine.Instance.EngineSettings.DebugMode = true;
		// Añade las escenas
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.MainMenu.MainMenuScene(GameLogic.Scenes.MainMenu.MainMenuScene.SceneName));
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.Games.GameScene(GameLogic.Scenes.Games.GameScene.SceneName));
		GameEngine.Instance.SceneManager.AddScene(new GameLogic.Scenes.TilesSample.TilesScene(GameLogic.Scenes.TilesSample.TilesScene.SceneName, 1));
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
		GameEngine.Instance.ResourcesManager.LoadSettings("Content/Settings/textures.json");
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
			builder.WithAction(InputMappings.DefaultIntroAction)
						.WithKeyboard(InputMappings.Status.JustPressed, Keys.Enter)
				   .WithAction(InputMappings.DefaultMouseClickAction)
						.WithMouse(InputMappings.Status.JustPressed, Bau.Libraries.BauGame.Engine.Managers.Input.MouseController.MouseStatus.MouseButton.Left)
				   .WithAction(InputMappings.DefaulQuitAction)
						.WithKeyboard(InputMappings.Status.JustPressed, Keys.Escape);
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
		GameEngine.Instance.SceneManager.Draw(gameTime);
		// Llama al método base de dibujo
		base.Draw(gameTime);
	}
}