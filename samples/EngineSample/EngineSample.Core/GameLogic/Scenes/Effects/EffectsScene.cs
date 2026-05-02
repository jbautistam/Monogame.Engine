using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Scenes.Effects;

/// <summary>
///		Escena de la partida
/// </summary>
public class EffectsScene(SceneManager sceneManager) 
					: AbstractScene(sceneManager, SceneName, new Bau.BauEngine.Entities.Common.WorldDefinitionModel(2_000, 2_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = nameof(EffectsScene);

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Crea la capa donde se situan los actores de la novela gráfica
		EffectsLayer = new EffectsLayer(this, SceneName, 1);
		// Actualiza las dimensiones del mundo a las dimensiones de la pantalla
		UpdateViewPortSelf();
		// Crea las capas
		LayerManager.Clear();
		LayerManager.AddLayer(new Bau.BauEngine.Scenes.Layers.Backgrounds.FixedBackgroundLayer(this, "background", "bg-layer-4", null, 1));
		LayerManager.AddLayer(EffectsLayer);
		LayerManager.AddLayer(new EffectsUserInterfaceLayer(this, "UI", 2));
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override NextSceneContextModel? UpdateScene(Bau.BauEngine.Managers.GameContext gameContext)
	{
		// Actualiza los actores y el interface de usuario
		LayerManager.Update(gameContext);
		// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
		if (SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
			return new(MainMenu.MainMenuScene.SceneName);
		else
			return null;
	}

	/// <summary>
	///		Actualiza los datos cuando se cambia las dimensiones de la pantalla
	/// </summary>
	protected override void UpdateViewPortSelf()
	{
		Microsoft.Xna.Framework.Graphics.Viewport viewPort = GetViewPort();

			UpdateWorldDefinition(viewPort.Width, viewPort.Height);
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}

	/// <summary>
	///		Capa del juego
	/// </summary>
	public EffectsLayer? EffectsLayer { get; private set; }
}