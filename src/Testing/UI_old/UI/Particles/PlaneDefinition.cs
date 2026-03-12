namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record PlaneDefinition
{
    public required Vector2 Normal { get; init; }
    public required float Distance { get; init; }
    public bool Infinite { get; init; } = true;
    public Vector2? Size { get; init; }
}
