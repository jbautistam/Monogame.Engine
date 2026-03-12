namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / GRADIENTS
// ============================================================================

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GradientDefinition), "gradient")]
[JsonDerivedType(typeof(RandomizedGradientDefinition), "randomized")]
[JsonDerivedType(typeof(GradientCurveDefinition), "curve")]
public abstract record GradientDefinition
{
    public string? Name { get; init; }
    public int Version { get; init; } = 1;
    public ColorSpace ColorSpace { get; init; } = ColorSpace.Linear;
    
    public enum ColorSpace { Linear, SRGB, HSV, HSL, OKLab }
}
