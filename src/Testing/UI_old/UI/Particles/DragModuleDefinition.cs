namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record DragModuleDefinition : ModuleDefinition
{
    public FloatRangeOrCurve Coefficient { get; init; } = new ConstantCurveDefinition { Value = 0.1f };
    public bool AffectsRotation { get; init; } = false;
    public float SleepThreshold { get; init; } = 0.001f;
}
