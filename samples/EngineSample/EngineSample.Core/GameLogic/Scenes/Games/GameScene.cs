using Microsoft.Xna.Framework;
using Bau.Monogame.Engine.Domain.Core.Scenes;
using Bau.Monogame.Engine.Domain.Core.Scenes.Layers;
using Bau.Monogame.Engine.Domain.Core.Scenes.Layers.Backgrounds;
using Bau.Monogame.Engine.Domain.Core.Scenes.Layers.Builders.UserInterface;
using Bau.Monogame.Engine.Domain;
using Bau.Monogame.Engine.Domain.Core.Scenes.Layers.UserInterface;

namespace EngineSample.Core.GameLogic.Scenes.Games;

/// <summary>
///		Escena de la partida
/// </summary>
internal class GameScene(string name) : AbstractScene(name, new Rectangle(0, 0, 5_000, 5_000))
{
	// Constantes públicas
	public const string SceneName = "Game";
	public const int PhysicsPlayerLayer = 1;
	public const int PhysicsPlayerProjectileLayer = 2;
	public const int PhysicsNpcLayer = 3;
	public const int PhysicsNpcProjectileLayer = 4;

	// Variables privadas
	private UserInterfaceLayer? _hudLayer;

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Guarda el interface de usuario Hud
		_hudLayer = CreateHudLayer();
		// Añade la capa
		LayerManager.AddLayer(new GameLayer(this, SceneName, 1));
		LayerManager.AddLayer(CreateBackgroundLayer());
		LayerManager.AddLayer(_hudLayer);
		// Crea los datos de físicas
		CreatePhysics();
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Crea los datos de las capas de físicas
	/// </summary>
	private void CreatePhysics()
	{
		PhysicsManager.LayersRelations.AddRelation(PhysicsPlayerLayer, PhysicsNpcLayer);
		PhysicsManager.LayersRelations.AddRelation(PhysicsPlayerLayer, PhysicsNpcProjectileLayer);
		PhysicsManager.LayersRelations.AddRelation(PhysicsNpcLayer, PhysicsPlayerProjectileLayer);
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

			// Añade la etiqueta
			builder.WithItem(new UserInterfaceLabelBuilder(builder.Layer, "Este es el texto de la etiqueta", 0.5f, 0.5f, 1, 1)
									.WithFont("Fonts/Hud")
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(builder.Layer, "Score", 0.01f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.Red)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(builder.Layer, "0", 0.07f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.White)
									.WithId("lblScore")
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(builder.Layer, "Inferior", 0, 0.95f, 1, 1)
									.WithFont("Fonts/Hud")
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(builder.Layer, "Derecha", 0.7f, 0.9f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.Red)
									.Build()
							);
			// y devuelve la capa creada
			return builder.Build();
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(GameTime gameTime)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores y el interface de usuario
			LayerManager.Update(gameTime);
			UpdateUserInterface();
			// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
			if (GameEngine.Instance.InputManager.IsAction(Bau.Monogame.Engine.Domain.Core.Managers.Input.InputMappings.DefaulQuitAction))
				nextScene = GameEngine.Instance.SceneManager.GetScene(MainMenu.MainMenuScene.SceneName) ?? this;
			// Devuelve la nueva escena
			return nextScene;
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	private void UpdateUserInterface()
	{
		if (_hudLayer is not null)
		{
			UiLabel? lblScore = _hudLayer.GetItem<UiLabel>("lblScore");

				// Cambia las etiquetas
				if (lblScore is not null)
					lblScore.Text = "1.221";
		}
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
