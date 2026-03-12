namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record BezierCurveDefinition : CurveDefinition
{
    public required Vector2 Start { get; init; }
    public required Vector2 End { get; init; }
    public required Vector2 ControlPoint1 { get; init; }
    public Vector2? ControlPoint2 { get; init; }
    public int LutResolution { get; init; } = 64;
}
