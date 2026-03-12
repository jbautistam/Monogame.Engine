namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record InitPositionModuleDefinition : ModuleDefinition
{
    public Vector2Range Offset { get; init; } = new(Vector2.Zero, Vector2.Zero);
    public bool ApplyRotation { get; init; } = true;
}
