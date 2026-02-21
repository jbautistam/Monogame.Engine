namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record LineShapeDefinition : EmissionShapeDefinition
{
    public Vector2 Start { get; init; } = new(-1, 0);
    public Vector2 End { get; init; } = new(1, 0);
    public bool RandomPosition { get; init; } = true;
    public float NormalSpread { get; init; } = 0f;
    public float Thickness { get; init; } = 0f;
}
