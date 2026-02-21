using Bau.Libraries.BauGame.Engine.Entities.Common;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

/// <summary>
///		Clase con los datos de un emisor de partículas
/// </summary>
public class Emissor(ParticlesSystemEngineActor particlesSystemEngineActor)
{
    /// <summary>
    ///     Motor de partículas
    /// </summary>
    public ParticlesSystemEngineActor ParticlesSystemEngineActor { get; } = particlesSystemEngineActor;

    /// <summary>
    ///     Momento de inicio de emisión
    /// </summary>
    public required RangeStruct<float> StartEmision { get; init; }

    /// <summary>
    ///     Duración (0 si es infinito)
    /// </summary>
    public required RangeStruct<float> Duration { get; init; }

	/// <summary>
	///     Ratio de emisión
	/// </summary>
	public required RangeStruct<float> EmissionRate { get; init; }

    /// <summary>
    ///     Número de partículas por emisión
    /// </summary>
    public required RangeStruct<float> ParticlesPerEmission { get; init; }

    /// <summary>
    ///     Propiedades de lanzamiento de partículas
    /// </summary>
    public required ParticleSpawnProperties SpawnProperties { get; init; }

    /// <summary>
    ///     Figura de emisión
    /// </summary>
    public required AbstractEmissorShape Shape {get; init; }
}