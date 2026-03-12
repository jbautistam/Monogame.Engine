namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record CompositeCurveDefinition : CurveDefinition
{
    public required List<Segment> Segments { get; init; }
    public bool AutoNormalize { get; init; } = true;
    
    public sealed record Segment
    {
        public required CurveDefinition Curve { get; init; }
        public required float Weight { get; init; }
        public EasingType BlendEasing { get; init; } = EasingType.Linear;
    }
}
