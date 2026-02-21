namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// Init Modules
public sealed record LifetimeModuleDefinition : ModuleDefinition
{
    public required FloatRangeOrCurve Lifetime { get; init; }
    public bool AllowEarlyDeath { get; init; } = true;
    public CurveDefinition? LifetimeScaleCurve { get; init; }
}
