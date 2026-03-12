namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record RandomizedGradientDefinition : GradientDefinition
{
    public required GradientDefinition BaseGradient { get; init; }
    public FloatRange HueShift { get; init; } = new(0, 0);
    public FloatRange SaturationShift { get; init; } = new(0, 0);
    public FloatRange ValueShift { get; init; } = new(0, 0);
    public FloatRange AlphaShift { get; init; } = new(0, 0);
    public bool PerParticleVariation { get; init; } = true;
    public int? FixedSeed { get; init; }
}
