namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record TemporalSettingsDefinition
{
    public bool MotionBlur { get; init; } = false;
    public float MotionBlurStrength { get; init; } = 0.5f;
    public bool PhysicsSubstep { get; init; } = false;
    public float SubstepThreshold { get; init; } = 10f;
    public bool InterpolatedFlipbook { get; init; } = true;
    public bool AccumulationTrails { get; init; } = false;
    public int AccumulationFrames { get; init; } = 4;
}
