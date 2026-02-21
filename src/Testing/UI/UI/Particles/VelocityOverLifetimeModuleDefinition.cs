namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record VelocityOverLifetimeModuleDefinition : ModuleDefinition
{
    public CurveDefinition? OrbitalX { get; init; }
    public CurveDefinition? OrbitalY { get; init; }
    public CurveDefinition? Radial { get; init; }
    public CurveDefinition? SpeedModifier { get; init; }
    public SpaceMode Space { get; init; } = SpaceMode.Local;
}
