using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Layers.Backgrounds;
using Bau.BauEngine.Scenes.Layers.Builders.UserInterface;
using Bau.BauEngine.Entities.UserInterface;

namespace EngineSample.Core.GameLogic.Scenes.MainMenu;

/// <summary>
///		Escena del menú principal
/// </summary>
internal class MainMenuScene(SceneManager sceneManager) : AbstractScene(sceneManager, SceneName, null)
{
	// Constantes públicas
	public const string SceneName = "MainMenu";
	// Constantes privadas
	private const string MenuLayer = "Menu";
	private const string MenuStyle = nameof(MenuStyle);
	private const string MenuOptionsStyle = nameof(MenuOptionsStyle);
	// Enumerados privados
	private enum MenuOption
	{
		Play,
		TilesSample,
		SpaceShips,
		Animations,
		Particles,
		Music,
		Effect,
		UserInterfaceGrid,
		UserInterfaceGallery,
		DebugMode,
		Quit
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
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "background", "bg-layer-0", null, 1));
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
		Configuration.ResourcesLoader loader = new(SceneManager.EngineManager);

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
			builder.WithOption((int) MenuOption.Play, "Play", MenuOptionsStyle, 0.2f, 0, 0.6f, 1)
					.WithOption((int) MenuOption.SpaceShips, "SpaceShips", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.TilesSample, "Tiles sample", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Animations, "Animations", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Particles, "Particles", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.UserInterfaceGrid, "UI Grid", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.UserInterfaceGallery, "Gallery", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Music, "Music", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Effect, "Effect", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.DebugMode, "Debug mode", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Quit, "Quit", MenuOptionsStyle, 0.2f, 0.4f, 0.6f, 1)
					.WithStyle(MenuStyle);
			// Guarda el menú en una variable
			_menu = builder.Build();
			return _menu;
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override string? UpdateScene(Bau.BauEngine.Managers.GameContext gameContext)
	{
			// Actualiza los actores
			LayerManager.Update(gameContext);
			// Sale del juego si se ha pulsado el botón de Scape
			if (SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
				SceneManager.EngineManager.Exit();
			// Devuelve la nueva escena
			if (_menu is not null)
				switch ((MenuOption?) _menu.GetAndResetClickOption())
				{
					case MenuOption.Play:
						return Games.GameScene.SceneName;
					case MenuOption.TilesSample:
						return TilesSample.TilesScene.SceneName;
					case MenuOption.SpaceShips:
						return Space.SpaceShipsScene.SceneName;
					case MenuOption.Particles:
						return Particles.ParticlesScene.SceneName;
					case MenuOption.Animations:
						return Animations.AnimationsScene.SceneName;
					case MenuOption.UserInterfaceGrid:
						return UserInterfaceGridTest.UserInterfaceGridScene.SceneName;
					case MenuOption.UserInterfaceGallery:
						return UserInterfaceGalleryTest.UserInterfaceGalleryScene.SceneName;
					case MenuOption.Music:
							PlaySong();
						break;
					case MenuOption.Effect:
							PlayEffect();
						break;
					case MenuOption.DebugMode:
							SceneManager.EngineManager.EngineSettings.DebugSettings.IsDebugging = !SceneManager.EngineManager.EngineSettings.DebugSettings.IsDebugging;
						break;
					case MenuOption.Quit:
							SceneManager.EngineManager.Exit();
						break;
				}
			// Devuelve la nueva escena
			return string.Empty;
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
			SceneManager.EngineManager.AudioManager.PlaySong(sounds[new Random().Next(sounds.Count)], 
															 Bau.BauEngine.Managers.Audio.AudioManager.TransitionType.CrossFade, 
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
			SceneManager.EngineManager.AudioManager.PlayEffect(sounds[new Random().Next(sounds.Count)]);
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
