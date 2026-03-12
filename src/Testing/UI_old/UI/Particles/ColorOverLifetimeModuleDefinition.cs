namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record ColorOverLifetimeModuleDefinition : ModuleDefinition
{
    public required GradientDefinition Gradient { get; init; }
    public bool UseParticleTime { get; init; } = true;
}
