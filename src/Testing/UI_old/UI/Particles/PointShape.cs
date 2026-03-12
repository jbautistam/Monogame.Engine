namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class PointShape : EmissionShape
{
    public PointShape(PointShapeDefinition def) : base(def) { }
    public override EmissionPoint Sample(ParticleRandom random) => 
        new(TransformPoint(Vector2.Zero), Vector2.Zero);
    public override float GetApproximateArea() => 0f;
    public override BoundingBox2D GetBounds() => 
        new(TransformPoint(Vector2.Zero), Vector2.Zero);
}
