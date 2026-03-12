namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record RectangleShapeDefinition : EmissionShapeDefinition
{
    public required Vector2 Size { get; init; }
    public float CornerRadius { get; init; } = 0f;
    public bool Centered { get; init; } = true;
}
