namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record SubEmitterModuleDefinition : ModuleDefinition
{
    public required List<SubEmitterBinding> Emitters { get; init; }
    public int MaxInstances { get; init; } = 10;
}
