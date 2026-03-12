namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record NoiseMovementDefinition
{
    public float Frequency { get; init; } = 1f;
    public float Amplitude { get; init; } = 0.1f;
    public int Octaves { get; init; } = 1;
}
