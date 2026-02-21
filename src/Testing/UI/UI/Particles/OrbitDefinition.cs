namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record OrbitDefinition
{
    public float Radius { get; init; } = 1f;
    public float AngularSpeed { get; init; } = 1f;
    public float EllipseRatio { get; init; } = 1f;
}
