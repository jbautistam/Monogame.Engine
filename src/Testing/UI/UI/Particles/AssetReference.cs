namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record AssetReference
{
    public required AssetType Type { get; init; }
    public required string Path { get; init; }
    public string Guid { get; init; } = System.Guid.NewGuid().ToString();
    public bool Required { get; init; } = true;
    public string? FallbackPath { get; init; }
    public Dictionary<string, object> ImportSettings { get; init; } = new();
    public string? SubAsset { get; init; }
    public int Version { get; init; } = 1;
}
