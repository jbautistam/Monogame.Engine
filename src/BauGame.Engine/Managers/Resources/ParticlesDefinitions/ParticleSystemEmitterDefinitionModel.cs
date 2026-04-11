using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors.ParticlesEngine.Emitters;
using Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

namespace Bau.BauEngine.Managers.Resources.ParticlesDefinition;

/// <summary>
///     Definición de la emisión de partículas
/// </summary>
public class ParticleSystemEmitterDefinitionModel(ParticleEmitterShape shape)
{
    /// <summary>
    ///     Figura asociada al emisor
    /// </summary>
    public ParticleEmitterShape EmitterShape { get; } = shape;

    /// <summary>
    ///     Posición del emisor
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    ///     Perfil de emixión
    /// </summary>
    public ParticleEmitterProfile Profile { get; } = new();

    /// <summary>
    ///     Modificadores
    /// </summary>
    public List<AbstractParticleModifier> Modifiers { get; } = [];
}
