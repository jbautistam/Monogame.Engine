namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Behaviors;

/// <summary>
///     Clase abstracta para el comportamiento de una partícula
/// </summary>
public abstract class AbstractParticleBehavior(Particle particle)
{
    /// <summary>
    ///     Inicializa el comportamiento
    /// </summary>
    public abstract void Reset();

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    public abstract void Update(Managers.GameContext gameContext);

    /// <summary>
    ///     Datos de la partícula
    /// </summary>
    public Particle Particle { get; } = particle;
}
