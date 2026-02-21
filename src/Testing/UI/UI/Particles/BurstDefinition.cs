namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record BurstDefinition
{
    public required float Time { get; init; }
    public required BurstCount Count { get; init; }
    public int Cycles { get; init; } = 1;
    public float Interval { get; init; } = 0f;
    public float Probability { get; init; } = 100f;
    public bool TimeRelativeToEmitter { get; init; } = true;
}
