namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

// ============================================================================
// RUNTIME / LOD
// ============================================================================

public readonly struct LodSettings
{
    public float EmissionMultiplier { get; init; }
    public int MaxParticles { get; init; }
    public bool DisableTrails { get; init; }
    public bool DisableCollision { get; init; }
    public int UpdateEveryNFrames { get; init; }
    public float LifetimeMultiplier { get; init; }
    public float SizeMultiplier { get; init; }
    public bool DisableRendering { get; init; }
    public string? SimplifiedSystemPrefab { get; init; }
    
    public static readonly LodSettings FullQuality = new()
    {
        EmissionMultiplier = 1f,
        MaxParticles = -1,
        UpdateEveryNFrames = 1,
        LifetimeMultiplier = 1f
    };
    
    public static readonly LodSettings Culled = new()
    {
        EmissionMultiplier = 0f,
        DisableRendering = true
    };
    
    public void Lerp(LodSettings a, LodSettings b, float t)
    {
        // Interpolación de settings para transición suave
    }
}
