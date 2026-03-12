namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / MATERIAL & ASSETS
// ============================================================================

public sealed record MaterialDefinition
{
    public required string ShaderName { get; init; }
    public string? TexturePath { get; init; }
    public Dictionary<string, object> Parameters { get; init; } = new();
    public BlendMode BlendMode { get; init; } = BlendMode.Alpha;
    public Rectangle? SourceRect { get; init; }
    public Vector2 Pivot { get; init; } = Vector2.Zero;
}
