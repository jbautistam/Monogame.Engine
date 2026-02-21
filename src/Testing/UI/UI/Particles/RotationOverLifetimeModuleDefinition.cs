namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record RotationOverLifetimeModuleDefinition : ModuleDefinition
{
    public FloatRangeOrCurve? AngularVelocity { get; init; }
    public CurveDefinition? AbsoluteRotation { get; init; }
}
