namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record ConstantCurveDefinition : CurveDefinition
{
    public required float Value { get; init; }
}
