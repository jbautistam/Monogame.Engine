using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Modifiers;

/// <summary>
///     Clase abstracta para los modificadores de partículas
/// </summary>
public abstract class AbstractParticleModifier
{
    /// <summary>
    ///     Forma de ejecución del modificador
    /// </summary>
    public enum ModifierExecutionTime
    {
        /// <summary>Cuando nace la partícula</summary>
        OnSpawn,
        /// <summary>En cada modificación</summary>
        PerFrame,
        /// <summary>Dependiente del tiempo de la vida de la partícula</summary>
        OverLifetime
    }

    /// <summary>
    ///     Actualiza los datos de la partícula
    /// </summary>
    public void Update(ParticleModel particle, float deltaTime)
    {
        UpdateSelf(particle, deltaTime, particle.LifeTime / MathHelper.Max(particle.TotalLifeTime, 0.001f));
    }

    /// <summary>
    ///     Actualiza los datos de la partícula
    /// </summary>
    protected abstract void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge);
}
