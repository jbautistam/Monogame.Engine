namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / SYSTEM & EMITTER
// ============================================================================

public sealed record SystemMovementDefinition
{
    public Vector2? ConstantVelocity { get; init; }
    public Vector2? Acceleration { get; init; }
    public CurveDefinition? PositionXCurve { get; init; }
    public CurveDefinition? PositionYCurve { get; init; }
    public OrbitDefinition? Orbit { get; init; }
    public NoiseMovementDefinition? PositionNoise { get; init; }
}
