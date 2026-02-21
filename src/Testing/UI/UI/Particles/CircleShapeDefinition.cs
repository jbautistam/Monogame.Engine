namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record CircleShapeDefinition : EmissionShapeDefinition
{
    public float InnerRadius { get; init; } = 0f;
    public required float Radius { get; init; }
    public float ArcStart { get; init; } = 0f;
    public float ArcSpan { get; init; } = 360f;
    public bool ArcRotates { get; init; } = false;
    public float ArcRotationSpeed { get; init; } = 0f;
}
