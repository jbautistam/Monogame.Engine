namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record TextureShapeDefinition : EmissionShapeDefinition
{
    public required string TexturePath { get; init; }
    public float Threshold { get; init; } = 0.5f;
    public bool InheritColor { get; init; } = false;
    public bool UseAlphaAsDensity { get; init; } = true;
    public Vector2 Size { get; init; } = Vector2.One;
    public int SampleResolution { get; init; } = 64;
}
