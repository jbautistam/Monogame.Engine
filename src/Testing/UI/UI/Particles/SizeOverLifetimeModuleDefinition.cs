namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record SizeOverLifetimeModuleDefinition : ModuleDefinition
{
    public CurveDefinition? Uniform { get; init; }
    public CurveDefinition? X { get; init; }
    public CurveDefinition? Y { get; init; }
    public bool FixedScreenSize { get; init; } = false;
}
