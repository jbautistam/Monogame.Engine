namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// Update Modules
public sealed record GravityModuleDefinition : ModuleDefinition
{
    public Vector2 Acceleration { get; init; } = new(0, -9.81f);
    public SpaceMode Space { get; init; } = SpaceMode.World;
    public CurveDefinition? ScaleOverLifetime { get; init; }
}
