namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record ParticleSystemDefinition
{
    public string? Name { get; init; }
    public int MaxParticles { get; init; } = 1000;
    public SimulationSpace Space { get; init; } = SimulationSpace.Local;
    public bool PlayOnAwake { get; init; } = true;
    public bool Prewarm { get; init; } = false;
    public float PrewarmTime { get; init; } = 0f;
    public bool FollowTransform { get; init; } = true;
    public bool AllowEmitterMovement { get; init; } = false;
    public bool Loop { get; init; } = true;
    public float SimulationSpeed { get; init; } = 1f;
    
    public required List<EmitterDefinition> Emitters { get; init; }
    public List<VectorFieldDefinition> GlobalVectorFields { get; init; } = new();
    public SystemMovementDefinition? SystemMovement { get; init; }
    
    public bool EnableLod { get; init; } = true;
    public float LodBlendDistance { get; init; } = 2f;
    public float CullDistance { get; init; } = float.MaxValue;
    public bool PauseWhenCulled { get; init; } = true;
    public bool SimulateWhenCulled { get; init; } = false;
    public List<LodDefinition> LodLevels { get; init; } = new();
    
    public TemporalSettingsDefinition? Temporal { get; init; }
}
