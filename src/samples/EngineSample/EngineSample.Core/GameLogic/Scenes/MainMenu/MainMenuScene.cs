using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine;

namespace EngineSample.Core.GameLogic.Scenes.MainMenu;

/// <summary>
///		Escena del menú principal
/// </summary>
internal class MainMenuScene(string name) : AbstractScene(name, null)
{
	// Constantes públicas
	public const string SceneName = "MainMenu";
	// Constantes privadas
	private const string DefaultFont = "Fonts/Hud";
	private const string MenuLayer = "Menu";
	private const string MenuStyle = nameof(MenuStyle);
	private const string MenuOptionsStyle = nameof(MenuOptionsStyle);
	// Enumerados privados
	private enum MenuOption
	{
		Play,
		TilesSample,
		SpaceShips,
		GraphicNovel,
		Animations,
		Music,
		Effect,
		UserInterface,
		UserInterfaceGrid,
		UserInterfaceGallery,
		DebugMode,
		Quit = 40
	}
	// Variables privadas
	private UiMenu? _menu;

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Añade la capa
		LayerManager.Clear();
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "background", "bg-layer-0", 1));
		LayerManager.AddLayer(CreateHudLayer());
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Crea la capa de HUD del interface de usuario
	/// </summary>
	private UserInterfaceLayer CreateHudLayer()
	{
		UserInterfaceLayer uiLayer = new(this, MenuLayer, 1);
		Configuration.ConfigurationLoader loader = new();

			// Carga los estilos
			uiLayer.Styles = loader.LoadStyles(uiLayer, "Settings/VisualNovel/Styles.xml");
			// Crea los componentes
			CreateComponents(uiLayer);
			// Devuelve la capa generada
			return uiLayer;
	}

	/// <summary>
	///		Crea los componentes de la capa
	/// </summary>
	private void CreateComponents(UserInterfaceLayer uiLayer)
	{
		UserInterfaceBuilder builder = new();

			// Añade los componentes del interface de usuario
			builder.WithItem(CreateMainMenu(uiLayer));
			// Añade los elementos generados a la capa
			uiLayer.Items.AddRange(builder.Build());
	}

	/// <summary>
	///		Crea el menú principal
	/// </summary>
	private UiMenu CreateMainMenu(AbstractUserInterfaceLayer layer)
	{
		UserInterfaceMenuBuilder builder = new(layer, 0.05f, 0.05f, 0.4f, 0.8f);

			// Asigna los elementos al menú
			builder.WithOption((int) MenuOption.Play, "Play", DefaultFont, MenuOptionsStyle, 0.2f, 0, 0.6f, 1)
					.WithOption((int) MenuOption.SpaceShips, "SpaceShips", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.TilesSample, "Tiles sample", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.GraphicNovel, "Graphic novel", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Animations, "Animations", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.UserInterface, "User interface", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.UserInterfaceGrid, "UI Grid", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.UserInterfaceGallery, "Gallery", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Music, "Music", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Effect, "Effect", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.DebugMode, "Debug mode", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Quit, "Quit", DefaultFont, MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithStyle(MenuStyle);
			// Guarda el menú en una variable
			_menu = builder.Build();
			return _menu;
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores
			LayerManager.Update(gameContext);
			// Sale del juego si se ha pulsado el botón de Scape
			if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaulQuitAction))
				GameEngine.Instance.Exit();
			// Devuelve la nueva escena
			if (_menu is not null)
				switch ((MenuOption?) _menu.GetAndResetClickOption())
				{
					case MenuOption.Play:
							nextScene = GetNewScene(Games.GameScene.SceneName);
						break;
					case MenuOption.TilesSample:
							nextScene = GetNewScene(TilesSample.TilesScene.SceneName);
						break;
					case MenuOption.SpaceShips:
							nextScene = GetNewScene(Space.SpaceShipsScene.SceneName);
						break;
					case MenuOption.GraphicNovel:
							nextScene = GetNewScene(GraphicNovel.GraphicNovelScene.SceneName);
						break;
					case MenuOption.Animations:
							nextScene = GetNewScene(Animations.AnimationsScene.SceneName);
						break;
					case MenuOption.UserInterface:
							nextScene = GetNewScene(UserInterfaceTest.UserInterfaceScene.SceneName);
						break;
					case MenuOption.UserInterfaceGrid:
							nextScene = GetNewScene(UserInterfaceGridTest.UserInterfaceGridScene.SceneName);
						break;
					case MenuOption.UserInterfaceGallery:
							nextScene = GetNewScene(UserInterfaceGalleryTest.UserInterfaceGalleryScene.SceneName);
						break;
					case MenuOption.Music:
							PlaySong();
						break;
					case MenuOption.Effect:
							PlayEffect();
						break;
					case MenuOption.DebugMode:
							GameEngine.Instance.EngineSettings.DebugMode = !GameEngine.Instance.EngineSettings.DebugMode;
						break;
					case MenuOption.Quit:
							GameEngine.Instance.Exit();
						break;
				}
			// Devuelve la nueva escena
			return nextScene;
	}

	/// <summary>
	///		Obtiene una escena
	/// </summary>
	private AbstractScene GetNewScene(string name)
	{
		AbstractScene? nextScene = GameEngine.Instance.SceneManager.GetScene(name);

			// Guarda la escena nueva
			if (nextScene is not null)
				return nextScene;
			else
				return this;
	}

	/// <summary>
	///		Reproduce la música
	/// </summary>
	private void PlaySong()
	{
		List<string> sounds = [ 
								"sounds/music/game",
								"sounds/music/fscm-productions-flowers",
								"sounds/music/fscm-productions-flowers-loop"
							  ];

			// Reproduce uno de los sonidos aleatoriamente
			GameEngine.Instance.AudioManager.PlaySong(sounds[new Random().Next(sounds.Count)], 
													  Bau.Libraries.BauGame.Engine.Managers.Audio.AudioManager.TransitionType.CrossFade, 
													  (float) TimeSpan.FromSeconds(5).TotalMilliseconds);
	}

	/// <summary>
	///		Reproduce un efecto
	/// </summary>
	private void PlayEffect()
	{
		List<string> sounds = [ 
								//"sounds/monsterKilled", "sounds/PlayerExitReached", "sounds/PlayerFall", "sounds/PlayerGemCollected",
								//"sounds/PlayerJump", "sounds/PlayerKilled", "sounds/PlayerPowerUp",
								"sounds/effects/game-explosion",
								"sounds/effects/laser-weapon-shot",
								"sounds/effects/short-laser-gun-shot",
								"sounds/effects/shot-light-explosion",
								"sounds/effects/war-explosions",
								"sounds/effects/whip-shot"
								];

			// Reproduce uno de los sonidos aleatoriamente
			GameEngine.Instance.AudioManager.PlayEffect(sounds[new Random().Next(sounds.Count)]);
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
