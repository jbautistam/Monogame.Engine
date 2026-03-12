namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record NoiseModuleDefinition : ModuleDefinition
{
    public FloatRangeOrCurve Frequency { get; init; } = new(1f, 1f);
    public FloatRangeOrCurve Strength { get; init; } = new(1f, 1f);
    public int Octaves { get; init; } = 1;
    public float Persistence { get; init; } = 0.5f;
    public float ScrollSpeed { get; init; } = 0f;
    public int Seed { get; init; } = 0;
    public bool Deterministic { get; init; } = true;
}
