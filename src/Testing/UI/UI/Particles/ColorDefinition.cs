namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record ColorDefinition
{
    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }
    public byte A { get; init; } = 255;
    
    public static ColorDefinition FromColor(Color c) => new() { R = c.R, G = c.G, B = c.B, A = c.A };
    public Color ToColor() => new(R, G, B, A);
}
