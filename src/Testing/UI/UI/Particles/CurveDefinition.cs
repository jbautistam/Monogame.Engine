namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ConstantCurveDefinition), "constant")]
[JsonDerivedType(typeof(LinearCurveDefinition), "linear")]
[JsonDerivedType(typeof(BezierCurveDefinition), "bezier")]
[JsonDerivedType(typeof(KeyframeCurveDefinition), "keyframes")]
[JsonDerivedType(typeof(CompositeCurveDefinition), "composite")]
[JsonDerivedType(typeof(RandomizedCurveDefinition), "randomized")]
public abstract record CurveDefinition
{
    public string? Name { get; init; }
    public int Version { get; init; } = 1;
    public FloatRange? OutputRange { get; init; }
}
