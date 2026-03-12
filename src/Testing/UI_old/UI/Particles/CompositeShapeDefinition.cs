namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record CompositeShapeDefinition : EmissionShapeDefinition
{
    public required List<WeightedShape> Shapes { get; init; }
    public CombineMode Mode { get; init; } = CombineMode.SelectRandom;
    
    public sealed record WeightedShape
    {
        public required EmissionShapeDefinition Shape { get; init; }
        public required float Weight { get; init; }
        public Vector2 LocalOffset { get; init; } = Vector2.Zero;
    }
    
    public enum CombineMode { SelectRandom, EmitAll, Intersect, Union }
}
