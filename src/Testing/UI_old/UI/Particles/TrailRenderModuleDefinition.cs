namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record TrailRenderModuleDefinition : ModuleDefinition
{
    public int MaxPoints { get; init; } = 20;
    public float MinVertexDistance { get; init; } = 0.1f;
    public FloatRange PointLifetime { get; init; } = new(0.5f, 0.5f);
    public CurveDefinition? WidthOverTrail { get; init; }
    public GradientDefinition? ColorOverTrail { get; init; }
    public bool WorldSpace { get; init; } = true;
    public string? TexturePath { get; init; }
    public bool DieWithParticle { get; init; } = true;
    public UvMode TextureMode { get; init; } = UvMode.Stretch;
    
    public enum UvMode { Stretch, Tile, PerSegment }
}
