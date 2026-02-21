namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record AnimatedOffsetDefinition
{
    public CurveDefinition? OffsetX { get; init; }
    public CurveDefinition? OffsetY { get; init; }
    public SpaceMode Space { get; init; } = SpaceMode.Local;
    public bool Cumulative { get; init; } = false;
}
