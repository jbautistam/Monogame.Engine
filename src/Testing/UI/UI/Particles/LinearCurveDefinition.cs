namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record LinearCurveDefinition : CurveDefinition
{
    public required float Start { get; init; }
    public required float End { get; init; }
    public EasingType Easing { get; init; } = EasingType.Linear;
}
