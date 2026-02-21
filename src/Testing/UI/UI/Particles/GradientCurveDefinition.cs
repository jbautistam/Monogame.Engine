namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record GradientCurveDefinition : GradientDefinition
{
    public required CurveDefinition RedCurve { get; init; }
    public required CurveDefinition GreenCurve { get; init; }
    public required CurveDefinition BlueCurve { get; init; }
    public CurveDefinition? AlphaCurve { get; init; }
    public bool NormalizedCurves { get; init; } = true;
}
