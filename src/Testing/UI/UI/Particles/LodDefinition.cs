namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / LOD & TEMPORAL
// ============================================================================

public sealed record LodDefinition
{
    public required float Distance { get; init; }
    public float EmissionMultiplier { get; init; } = 1f;
    public int MaxParticlesOverride { get; init; } = -1;
    public bool DisableTrails { get; init; } = false;
    public bool DisableCollision { get; init; } = false;
    public int UpdateEveryNFrames { get; init; } = 1;
    public float LifetimeMultiplier { get; init; } = 1f;
    public float SizeMultiplier { get; init; } = 1f;
    public string? SimplifiedSystemPrefab { get; init; }
    public int? TextureResolution { get; init; }
    public bool DisableRendering { get; init; } = false;
}
