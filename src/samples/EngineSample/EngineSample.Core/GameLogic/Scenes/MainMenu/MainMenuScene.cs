using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Core.Scenes;
using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.Backgrounds;
using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.Builders.UserInterface;
using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.UserInterface;
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
			layer.BackgroundLayers.Add(new BackgroundFixedLayer("bg-layer-0", 1));
			// Devuelve la capa creada
			return layer;
	}

	/// <summary>
	///		Crea la capa de HUD del interface de usuario
	/// </summary>
	private UserInterfaceLayer CreateHudLayer()
	{
		UserInterfaceBuilder builder = new(this, "Hud", 1);

			// Añade los componentes del interface de usuario
			builder.WithItem(new UserInterfaceLabelBuilder(builder.Layer, "Este es el texto de la etiqueta", 0.5f, 0.5f, 1, 1)
									.WithFont(DefaultFont)
									.Build()
							);
			builder.WithItem(CreateMainMenu(builder.Layer));
			builder.WithItem(new UserInterfaceBackgroundBuilder(builder.Layer, "sprites/gradient", 0.15f, 0.77f, 0.7f, 0.2f)
									.Build());
			builder.WithItem(new UserInterfaceBalloonLabelBuilder(builder.Layer,
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
			// y devuelve la capa creada
			return builder.Build();
	}

	/// <summary>
	///		Crea el menú principal
	/// </summary>
	private UiMenu CreateMainMenu(UserInterfaceLayer layer)
	{
		UserInterfaceMenuBuilder builder = new(layer, 0.1f, 0.1f, 0.5f, 0.5f);

			// Asigna los elementos al menú
			builder.WithBackground("Tiles/BlockA4")
					.WithUnselectedColor(Color.Green)
					.WithSelectedColor(Color.Navy)
					.WithHoverColor(Color.White)
					.WithSelectedBackground("Tiles/BlockA0")
					.WithUnselectedBackground("Tiles/BlockA3")
					.WithHoverBackground("Tiles/BlockB0")
					.WithOption(1, "Play", DefaultFont, 0.2f, 0, 0.6f, 1)
					.WithOption(2, "Update text", DefaultFont, 0.2f, 0.2f, 0.6f, 1)
					.WithOption(3, "Music", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption(4, "Effect", DefaultFont, 0.2f, 0.4f, 0.6f, 1)
					.WithOption(40, "Quit", DefaultFont, 0.2f, 0.4f, 0.6f, 1);
			// Guarda el menú en una variable
			_menu = builder.Build();
			return _menu;
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(GameTime gameTime)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores
			LayerManager.Update(gameTime);
			// Sale del juego si se ha pulsado el botón de Scape
			if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Core.Managers.Input.InputMappings.DefaulQuitAction))
				GameEngine.Instance.Exit();
			// Devuelve la nueva escena
			if (_menu is not null)
				switch (_menu.GetAndResetClickOption())
				{
					case 1:
							AbstractScene? gameScene = GameEngine.Instance.SceneManager.GetScene(Games.GameScene.SceneName);

								// Guarda la escena nueva
								if (gameScene is not null)
									nextScene = gameScene;
						break;
					case 2:
							ChangeText();
						break;
					case 3:
							PlaySong();
						break;
					case 4:
							PlayEffect();
						break;
					case 40:
							GameEngine.Instance.Exit();
						break;
				}
			// Devuelve la nueva escena
			return nextScene;
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
		UserInterfaceLayer? layer = LayerManager.Get<UserInterfaceLayer>("Hud");

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
