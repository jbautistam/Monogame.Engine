using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

/// <summary>
///		Comportamiento de un spawner
/// </summary>
public class ParticlesPoolManager(Emisors.AbstractEmissorShape emissor, ParticleSpawnProperties particleSpawnProperties)
{
    // Variables privadas
    private float _elapsedTime, _timeSinceLastEmission;
    private Pool.ObjectPool<ParticleProperties> _particles = new();

    /// <summary>
    ///     Inicializa el manager del pool
    /// </summary>
	internal void Start(ParticlesSystemEngineActor owner)
	{
        Owner = owner;
	}

	/// <summary>
	///		Actualiza el comportamiento
	/// </summary>
	public void Update(GameContext gameContext)
	{
        // Actualiza las partículas
        if (!IsStopped && _elapsedTime > StartEmision)
            Generate(gameContext);
        // Desactiva el spawner
        if (IsStopped && _particles.CountEnabled() == 0)
            Enabled = false;
        // Actualiza el tiempo
        _elapsedTime += gameContext.DeltaTime;
	}

    /// <summary>
    ///     Genera las partículas
    /// </summary>
    private void Generate(GameContext gameContext)
    {
        if (Owner is not null)
        {
            float interval = 1f / MathHelper.Max(EmissionRate, 0.1f);

                // Calcula el tiempo de siguiente emisión
                _timeSinceLastEmission += gameContext.DeltaTime;
                // Lanza las partículas
                if (_timeSinceLastEmission >= interval)
                {
                    // Genera las partículas
                    for (int index = 0; index < ParticlesPerEmission; index++)
                    {
                        Vector2 position = Emissor.GetEmissionPosition(Owner.Position);

                            // Genera las partículas
                            CreateParticle(position);
                    }
                    // Inicializa el tiempo de emisión
                    _timeSinceLastEmission = 0;
                }
        }
    }

    /// <summary>
    ///     Crea una partícula
    /// </summary>
	private void CreateParticle(Vector2 position)
	{
        ParticleProperties? particle = _particles.GetFirstInactive();

            // Añade la partícula al pool si no existía o la reinicia si ya existía
            if (particle is null)
            {
                particle = new ParticleProperties();
                _particles.Add(particle);
            }
            // Inicializa las propiedades de la partícula
            particle.Reset(position, ParticleSpawnProperties, ParticleBehaviors);
	}

    /// <summary>
    ///     Dibuja las partículas
    /// </summary>
	internal void Draw(Camera2D camera, GameContext gameContext)
	{
		foreach (ParticleProperties particle in _particles.Enumerate())
            particle.Draw(camera, gameContext);
	}

    /// <summary>
    ///     Propietario del manager
    /// </summary>
    public ParticlesSystemEngineActor? Owner { get; private set; }

    /// <summary>
    ///     Momento de inicio de emisión
    /// </summary>
    public float StartEmision { get; set; } = 0;

    /// <summary>
    ///     Duración (0 si es infinito)
    /// </summary>
    public float Duration { get; set; } = 1.0f;

	/// <summary>
	///     Ratio de emisión
	/// </summary>
	public float EmissionRate { get; set; } = 0.5f;

    /// <summary>
    ///     Número de partículas por emisión
    /// </summary>
    public float ParticlesPerEmission { get; set; }

    /// <summary>
    ///     Indica si el emisor está activo
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     Indica si se ha detenido la emisión
    /// </summary>
    public bool IsStopped => !Enabled || (Duration > 0 && _elapsedTime > Duration);

	/// <summary>
	///		Emisor
	/// </summary>
	public Emisors.AbstractEmissorShape Emissor { get; } = emissor;

    /// <summary>
    ///     Propiedades de emisión de las partículas
    /// </summary>
    public ParticleSpawnProperties ParticleSpawnProperties { get; } = particleSpawnProperties;

    /// <summary>
    ///     Comportamientos de las partículas
    /// </summary>
    public List<Behaviors.AbstractParticleBehavior> ParticleBehaviors { get; } = [];
}