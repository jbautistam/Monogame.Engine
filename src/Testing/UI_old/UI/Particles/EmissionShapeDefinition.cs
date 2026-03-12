namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / EMISSION SHAPES
// ============================================================================

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PointShapeDefinition), "point")]
[JsonDerivedType(typeof(CircleShapeDefinition), "circle")]
[JsonDerivedType(typeof(RectangleShapeDefinition), "rectangle")]
[JsonDerivedType(typeof(LineShapeDefinition), "line"))
[JsonDerivedType(typeof(PathShapeDefinition), "path"))
[JsonDerivedType(typeof(TextureShapeDefinition), "texture"))
[JsonDerivedType(typeof(CompositeShapeDefinition), "composite"))
[JsonDerivedType(typeof(AnimatedShapeDefinition), "animated"))
public abstract record EmissionShapeDefinition
{
    public Vector2 Offset { get; init; } = Vector2.Zero;
    public float Rotation { get; init; } = 0f;
    public Vector2 Scale { get; init; } = Vector2.One;
    public DistributionMode Distribution { get; init; } = DistributionMode.Uniform;
    public bool EdgeOnly { get; init; } = false;
    public float EdgeThickness { get; init; } = 0f;
    
    public enum DistributionMode { Uniform, Gaussian, InverseGaussian, Custom }
}
