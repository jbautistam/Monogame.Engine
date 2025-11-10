namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Behaviors;

/// <summary>
///		Comportamiento de una partícula
/// </summary>
public abstract class AbstractParticleBehavior
{
	/// <summary>
	///		Clona el comportamiento
	/// </summary>
	public abstract AbstractParticleBehavior Clone();

	/// <summary>
	///		Inicializa las propiedades del comportamiento
	/// </summary>
	public void Reset(ParticleProperties particle)
	{
		Particle = particle;
	}

	/// <summary>
	///		Inicializa el comportamiento
	/// </summary>
	protected abstract void ResetBehavior();

	/// <summary>
	///		Actualiza el comportamiento
	/// </summary>
	public abstract void Update(Managers.GameContext gameContext);

	/// <summary>
	///		Partícula a la que se aplica el comportamiento
	/// </summary>
	public ParticleProperties Particle { get; private set; } = default!;
}
