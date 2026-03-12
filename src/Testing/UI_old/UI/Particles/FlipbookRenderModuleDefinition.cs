namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record FlipbookRenderModuleDefinition : ModuleDefinition
{
    public required string TexturePath { get; init; }
    public required Point GridSize { get; init; }
    public FloatRange FrameRate { get; init; } = new(30, 30);
    public AnimationMode Mode { get; init; } = AnimationMode.Loop;
    public bool SyncWithLifetime { get; init; } = false;
    public CurveDefinition? TimeCurve { get; init; }
    public int? FixedRow { get; init; }
    
    public enum AnimationMode { Loop, Once, PingPong, RandomRow, RandomStart }
}
