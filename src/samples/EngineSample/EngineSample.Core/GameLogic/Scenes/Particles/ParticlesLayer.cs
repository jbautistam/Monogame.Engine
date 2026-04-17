using Bau.BauEngine.Actors.ParticlesEngine;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers.Games;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.Particles;

/// <summary>
///		Layer de la partida
/// </summary>
public class ParticlesLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	// Variables privadas
	private ParticleEngineActor? _particlesActor;

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		_particlesActor = Scene.SceneManager.EngineManager.ResourcesManager.ParticlesResourcesManagers.Create(this, "Smoke", new Vector2(100, 100));
	}

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateGameLayer(GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawGameLayer(Bau.BauEngine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
		// ... no hace nada, los actores ya se han modificado y esta capa no necesita nada más
	}

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndGameLayer()
	{
	}
}
