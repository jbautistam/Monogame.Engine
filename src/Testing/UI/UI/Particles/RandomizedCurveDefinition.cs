namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record RandomizedCurveDefinition : CurveDefinition
{
    public required CurveDefinition BaseCurve { get; init; }
    public CurveVariationType VariationType { get; init; } = CurveVariationType.Absolute;
    public FloatRange Variation { get; init; } = new(0, 0);
    public int? FixedSeed { get; init; }
    
    public enum CurveVariationType { Absolute, Relative }
}
