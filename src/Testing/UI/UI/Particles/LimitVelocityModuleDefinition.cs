namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record LimitVelocityModuleDefinition : ModuleDefinition
{
    public FloatRangeOrCurve? MaxSpeed { get; init; }
    public FloatRangeOrCurve? MinSpeed { get; init; }
    public LimitMode Mode { get; init; } = LimitMode.Clamp;
    public float DampenFactor { get; init; } = 0.9f;
    
    public enum LimitMode { Clamp, Dampen, Kill }
}
