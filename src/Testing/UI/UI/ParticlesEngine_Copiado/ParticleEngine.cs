using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine;

/// <summary>
///		Motor de partículas
/// </summary>
public class ParticleEngine
{
    /// <summary>
    ///     Actualiza el motor
    /// </summary>
    public void Update(GameTime gameTime)
	{
		foreach (ParticleEmitter emitter in Emitters)
			if (emitter.Enabled)
				emitter.Update(gameTime);
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