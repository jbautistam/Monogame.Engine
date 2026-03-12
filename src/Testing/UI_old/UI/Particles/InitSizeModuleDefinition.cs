namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record InitSizeModuleDefinition : ModuleDefinition
{
    public FloatRangeOrCurve? UniformSize { get; init; }
    public Vector2Range? SeparateSize { get; init; }
    public bool StretchByVelocity { get; init; } = false;
    public float StretchFactor { get; init; } = 0.1f;
    public float MaxStretch { get; init; } = 3f;
}
