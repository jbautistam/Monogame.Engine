using Bau.BauEngine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine;

/// <summary>
///		Motor de partículas
/// </summary>
public class ParticleEngine(Scenes.Layers.AbstractLayer layer, int? zOrder) : AbstractActor(layer, zOrder)
{
	/// <summary>
	///		Arranca el trabajo con el actor
	/// </summary>
	protected override void StartSelf()
	{
	}

    /// <summary>
    ///     Actualiza el motor
    /// </summary>
    protected override void UpdateSelf(GameContext gameContext)
	{
		foreach (ParticleEmitter emitter in Emitters)
			if (emitter.Enabled)
				emitter.Update(gameContext);
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndSelf(GameContext gameContext)
	{
	}

	/// <summary>
	///		Posición del motor de partículas
	/// </summary>
	public Vector2 Position { get; set; }

	/// <summary>
	///		Emisores
	/// </summary>
	public List<ParticleEmitter> Emitters { get; } = [];
}