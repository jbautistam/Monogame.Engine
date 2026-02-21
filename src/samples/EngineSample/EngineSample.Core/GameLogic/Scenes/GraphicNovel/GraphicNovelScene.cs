using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;
using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel;

/// <summary>
///		Escena de la partida
/// </summary>
internal class GraphicNovelScene(string name) : AbstractScene(name, new Bau.Libraries.BauGame.Engine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "GraphicNovel";
	public const int PhysicsPlayerLayer = 1;
	public const int PhysicsPlayerProjectileLayer = 2;
	public const int PhysicsNpcLayer = 3;
	public const int PhysicsNpcProjectileLayer = 4;
	// Variables privadas
	private Common.HudLayer? _hudLayer;
	private DynamicBackgroundLayer? _background;

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Guarda el interface de usuario Hud
		_hudLayer = new Common.HudLayer(this, Constants.LayerHud, 1);
		LayerManager.AddLayer(_hudLayer);
		// Añade la capa
		LayerManager.AddLayer(new GraphicNovelLayer(this, SceneName, 1));
		LayerManager.AddLayer(CreateBackgroundLayer());
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
	///		Crea la capa de fondo dinámica
	/// </summary>
	private DynamicBackgroundLayer CreateBackgroundLayer()
	{
		// Crea el fondo
		_background = new(this, "background", "bg-layer-0", 1);
		// Devuelve la capa de fondo
		return _background;
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override AbstractScene? UpdateScene(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		AbstractScene nextScene = this;

			// Actualiza los actores y el interface de usuario
			LayerManager.Update(gameContext);
			// Actualiza los efectos
			if (_background is not null)
			{
			/*
				if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.W))
					_background.Effects.Add(new WaveBackgroundEffect(3, false)
														{
															AmplitudeX = 10,
															AmplitudeY = 3,
															Frequency = 4
														}
										   );
				if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.S))
					_background.Effects.Add(new ShakeBackgroundEffect(8, false)
														{
															Magnitude = 40
														}
											);
				if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.P))
					_background.Effects.Add(new PulseEffect(3, false)
														{
															Magnitude = 4,
															Target = PulseEffect.PulseTarget.Zoom
														}
											);
				if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.Q))
					_background.Effects.Add(new PulseEffect(3, false)
														{
															Magnitude = 0.7f,
															Target = PulseEffect.PulseTarget.Rotation
														}
											);
				if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.D))
					_background.Effects.Add(new DriftEffect(3, false)
														{
															Velocity = new Microsoft.Xna.Framework.Vector2(3, -5)
														}
											);
				if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.Z))
					_background.Effects.Add(new ZoomEffect(3, false)
														{
															Start = 1,
															End = 1.3f
														}
											);
			*/
			}
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
