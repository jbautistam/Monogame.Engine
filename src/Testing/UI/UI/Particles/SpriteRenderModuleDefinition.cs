namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// Render Modules
public sealed record SpriteRenderModuleDefinition : ModuleDefinition
{
    public required string TexturePath { get; init; }
    public Rectangle? SourceRect { get; init; }
    public Vector2 Pivot { get; init; } = Vector2.Zero;
    public BillboardMode Billboard { get; init; } = BillboardMode.Billboard;
    public SpriteEffects Flip { get; init; } = SpriteEffects.None;
    public float StretchFactor { get; init; } = 1f;
    
    public enum BillboardMode { Billboard, Stretched, Fixed, None }
}
