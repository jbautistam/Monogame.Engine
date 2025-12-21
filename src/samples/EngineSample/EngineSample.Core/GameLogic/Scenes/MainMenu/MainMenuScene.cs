using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;
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
	// Enumerados privadas
	private enum MenuOption
	{
		Play,
		TilesSample,
		SpaceShips,
		GraphicNovel,
		UpdateText,
		Music,
		Effect,
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
		LayerManager.AddLayer(CreateBackgroundLayer());
		LayerManager.AddLayer(CreateHudLayer());
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Crea la capa con los fondos
	/// </summary>
	private BackgroundLayer CreateBackgroundLayer()
	{
		BackgroundLayer layer = new(this, "Background", 1);

			// Añade los fondos
			layer.BackgroundLayers.Add(new FixedBackgroundLayer("bg-layer-0", 1));
			// Devuelve la capa creada
			return layer;
	}

	/// <summary>
	///		Crea la capa de HUD del interface de usuario
	/// </summary>
	private UserInterfaceLayer CreateHudLayer()
	{
		UserInterfaceLayer uiLayer = new(this, "Menu", 1);
		UserInterfaceBuilder builder = new();

			// Añade los componentes del interface de usuario
			builder.WithItem(new UserInterfaceLabelBuilder(uiLayer, "Este es el texto de la etiqueta", 0.5f, 0.5f, 1, 1)
									.WithFont(DefaultFont)
									.Build()
							);
			builder.WithItem(CreateMainMenu(uiLayer));
			builder.WithItem(new UserInterfaceBackgroundBuilder(uiLayer, "sprites/gradient", 0.15f, 0.77f, 0.7f, 0.2f)
									.Build());
			builder.WithItem(new UserInterfaceBalloonLabelBuilder(uiLayer,
																	"""
																	Quiero comprobar cuanto es el ancho de este elemento en la pantalla y para eso tenemos que tener un texto muy largo que se salga de la pantalla
																	Este es un texto muy largo
																	que queremos mostrar
																	en varias líneas y poco
																	a poco
																	""",
																  0.2f, 0.8f, 0.6f, 0.2f)
									.WithFont(DefaultFont)
									.WithSpeed(0.05f)
									.WithId("lblPrompt")
									.Build()
							);
			// Añade los elementos generados a la capa
			uiLayer.Items.AddRange(builder.Build());
			// y devuelve la capa creada
			return uiLayer;
	}

	/// <summary>
	///		Crea el menú principal
	/// </summary>
	private UiMenu CreateMainMenu(AbstractUserInterfaceLayer layer)
	{
		UserInterfaceMenuBuilder builder = new(layer, 0.1f, 0.1f, 0.5f, 0.7f);

			// Asigna los elementos al menú
			builder.WithBackground("Tiles/BlockA4")
					.WithUnselectedColor(Color.Green)
					.WithSelectedColor(Color.Navy)
					.WithHoverColor(Color.White)
					.WithSelectedBackground("Tiles/BlockA0")
					.WithUnselectedBackground("Tiles/BlockA3")
					.WithHoverBackground("Tiles/BlockB0")
					.WithOption((int) MenuOption.Play, "Play", DefaultFont, 0.2f, 0, 0.6f, 1)
					.WithOption((int) MenuOption.SpaceShips, "SpaceShips", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.TilesSample, "Tiles sample", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.GraphicNovel, "Graphic novel", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.UpdateText, "Update text", DefaultFont, 0.2f, 0.2f, 0.6f, 1)
					.WithOption((int) MenuOption.Music, "Music", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Effect, "Effect", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.DebugMode, "Debug mode", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.Quit, "Quit", DefaultFont, 0.2f, 0.4f, 0.6f, 1);
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
					case MenuOption.UpdateText:
							ChangeText();
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
		if (AudioManager.IsPlayingMusic())
			AudioManager.Stop();
		else
			AudioManager.PlaySong("sounds/music");
	}

	/// <summary>
	///		Reproduce un efecto
	/// </summary>
	private void PlayEffect()
	{
		List<string> sounds = [ "sounds/monsterKilled", "sounds/PlayerExitReached", "sounds/PlayerFall", "sounds/PlayerGemCollected",
								"sounds/PlayerJump", "sounds/PlayerKilled", "sounds/PlayerPowerUp" ];

			// Reproduce uno de los sonidos aleatoriamente
			AudioManager.PlayEffect(sounds[new Random().Next(sounds.Count)]);
	}

	/// <summary>
	///		Cambia el texto del prompt
	/// </summary>
	private void ChangeText()
	{
		AbstractUserInterfaceLayer? layer = LayerManager.Get<AbstractUserInterfaceLayer>(Constants.LayerHud);

			if (layer is not null)
			{
				UiBalloonLabel? lblPrompt = layer.GetItem<UiBalloonLabel>("lblPrompt");

					if (lblPrompt is not null)
					{
						lblPrompt.Text = "Pero este es otro texto que se presenta después de todo el resto";
						lblPrompt.Color = Color.Red;
					}
			}
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
