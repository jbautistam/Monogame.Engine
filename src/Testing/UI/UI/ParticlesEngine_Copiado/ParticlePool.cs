using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine;

/// <summary>
///		Pool de partículas
/// </summary>
public class ParticlePool(ParticleEmitter emitter)
{
	/// <summary>
	///		Obtiene la siguiente partícula inactiva
	/// </summary>
	public ParticleModel? GetNext()
	{
		// Devuelve la primera partícula inactiva que encuentra
		foreach (ParticleModel particle in Particles)
			if (!particle.Enabled)
				return particle;
		// Si no se ha alcanzado el número máximo de partículas, se crea una nueva
		if (Particles.Count < Emitter.Profile.MaximumParticles)
		{
			// Añade la partícula
			Particles.Add(new ParticleModel());
			// y la devuelve
			return Particles[Particles.Count - 1];
		}
		// Si ha llegado hasta aquí es porque no ha encontrado nada
		return null;
	}

	/// <summary>
	///		Actualiza el pool de partículas (tiempo de vida)
	/// </summary>
	public void Update(GameTime gameTime)
	{
		foreach (ParticleModel particle in Particles)
			if (particle.Enabled)
			{
				// Ejecuta los modificadores
				foreach (Modifiers.AbstractParticleModifier modifier in Modifiers)
					modifier.Update(particle, (float) gameTime.ElapsedGameTime.TotalSeconds);
				// Cambia el tiempo de vida de la partícula
				particle.LifeTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
			}
	}

	/// <summary>
	///		Emisor de partículas
	/// </summary>
	public ParticleEmitter Emitter { get; } = emitter;

	/// <summary>
	///		Partículas
	/// </summary>
	public List<ParticleModel> Particles { get; } = [];

    /// <summary>
    ///     Modificadores
    /// </summary>
    public List<Modifiers.AbstractParticleModifier> Modifiers { get; } = [];
}