namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record AccelerationModuleDefinition : ModuleDefinition
{
    public required CurveDefinition X { get; init; }
    public required CurveDefinition Y { get; init; }
    public SpaceMode Space { get; init; } = SpaceMode.World;
}
