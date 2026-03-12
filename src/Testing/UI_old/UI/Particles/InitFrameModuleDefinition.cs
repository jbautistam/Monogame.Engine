namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record InitFrameModuleDefinition : ModuleDefinition
{
    public IntRange StartFrame { get; init; } = new(0, 0);
    public bool RandomStartFrame { get; init; } = true;
    public FloatRange AnimationSpeed { get; init; } = new(30, 30);
    public AnimationMode Mode { get; init; } = AnimationMode.Loop;
    
    public enum AnimationMode { Loop, Once, PingPong, RandomRow, RandomStart, Freeze }
}
