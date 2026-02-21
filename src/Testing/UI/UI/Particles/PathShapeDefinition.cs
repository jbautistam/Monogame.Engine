namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record PathShapeDefinition : EmissionShapeDefinition
{
    public required List<Vector2> Points { get; init; }
    public PathInterpolation Interpolation { get; init; } = PathInterpolation.Linear;
    public bool Closed { get; init; } = false;
    public bool EmissionByLength { get; init; } = true;
    public float Thickness { get; init; } = 0f;
    
    public enum PathInterpolation { Linear, CatmullRom, QuadraticBezier, CubicBezier }
}
