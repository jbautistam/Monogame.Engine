namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record BurstCount
{
    public required int Min { get; init; }
    public required int Max { get; init; }
    public int GetValue(ParticleRandom r) => r.Range(Min, Max);
}
