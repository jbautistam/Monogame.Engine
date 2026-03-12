namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record InitRotationModuleDefinition : ModuleDefinition
{
    public FloatRange Rotation { get; init; } = new(0, 360);
    public bool AlignToVelocity { get; init; } = false;
    public float VelocityAlignmentOffset { get; init; } = 0f;
}
