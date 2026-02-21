namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record VortexModuleDefinition : ModuleDefinition
{
    public required Vector2 Center { get; init; }
    public FloatRangeOrCurve Strength { get; init; } = new(5f, 5f);
    public float Radius { get; init; } = 0f;
    public bool AttachToEmitter { get; init; } = false;
    public float Direction { get; init; } = 1f;
}
