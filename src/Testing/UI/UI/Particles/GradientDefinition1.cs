namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record GradientDefinition : GradientDefinition
{
    public required List<ColorStop> ColorStops { get; init; }
    public List<AlphaStop>? AlphaStops { get; init; }
    public BlendMode Mode { get; init; } = BlendMode.Blend;
    public bool PremultiplyAlpha { get; init; } = true;
    public int LutResolution { get; init; } = 256;
    
    public sealed record ColorStop
    {
        public required float Time { get; init; }
        public required ColorDefinition Color { get; init; }
    }
    
    public sealed record AlphaStop
    {
        public required float Time { get; init; }
        public required float Alpha { get; init; }
        public EasingType? Easing { get; init; }
    }
    
    public enum BlendMode { Blend, Fixed, BlendThenFixed }
}
