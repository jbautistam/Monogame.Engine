using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Layers.Backgrounds;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.Particles;

/// <summary>
///		Escena de la partida
/// </summary>
public class ParticlesScene(SceneManager sceneManager, string name) : AbstractScene(sceneManager, name, new Bau.BauEngine.Entities.Common.WorldDefinitionModel(1_000, 1_000, 200, 200))
{
	// Constantes públicas
	public const string SceneName = "Particles";
	// Variables privadas
	private ParticlesLayer? _particlesLayer;

	/// <summary>
	///		Arranca la escena
	/// </summary>
	protected override void StartScene()
	{
		// Cra la capa de partículas
		_particlesLayer = new ParticlesLayer(this, SceneName, 1);
		// Añade las capas
		LayerManager.AddLayer(new FixedBackgroundLayer(this, "background", "bg-layer-5", null, 1));
		LayerManager.AddLayer(new ParticlesUserInterfaceLayer(this, "particles", 1));
		LayerManager.AddLayer(_particlesLayer);
		// Arranca las capas
		LayerManager.Start();
	}

	/// <summary>
	///		Actualiza la escena
	/// </summary>
	protected override string? UpdateScene(Bau.BauEngine.Managers.GameContext gameContext)
	{
		// Actualiza los actores y el interface de usuario
		LayerManager.Update(gameContext);
		// Sale de la partida si se ha pulsado el botón de Scape o el Back del GamePad
		if (SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaulQuitAction))
			return MainMenu.MainMenuScene.SceneName;
		else
			return string.Empty;
	}

	/// <summary>
	///		Muestra un sistema de partículas
	/// </summary>
	public void ShowParticles(string name, Vector2 position)
	{
		if (_particlesLayer is not null)
			_particlesLayer.ShowParticles(name, position);
	}

	/// <summary>
	///		Finaliza la escena
	/// </summary>
	protected override void EndScene()
	{
	}
}
