namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

// ============================================================================
// RUNTIME / EMISSION SHAPES
// ============================================================================

public readonly struct EmissionPoint
{
    public readonly Vector2 Position;
    public readonly Vector2 Direction;
    public readonly Color? InheritColor;
    public readonly float ProbabilityWeight;
    
    public EmissionPoint(Vector2 pos, Vector2 dir, Color? color = null, float weight = 1f)
    {
        Position = pos; Direction = dir; InheritColor = color; ProbabilityWeight = weight;
    }
}
