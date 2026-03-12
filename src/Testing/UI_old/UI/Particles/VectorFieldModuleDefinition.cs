namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record VectorFieldModuleDefinition : ModuleDefinition
{
    public required VectorFieldDefinition Field { get; init; }
    public ApplyMode Mode { get; init; } = ApplyMode.Force;
    public bool RelativeToVelocity { get; init; } = false;
    public float MaxDelta { get; init; } = 100f;
    
    public enum ApplyMode { Force, Velocity, Drag }
}
