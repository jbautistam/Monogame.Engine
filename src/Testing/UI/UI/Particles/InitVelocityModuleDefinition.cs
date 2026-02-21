namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record InitVelocityModuleDefinition : ModuleDefinition
{
    public required FloatRangeOrCurve Speed { get; init; }
    public DirectionMode DirectionMode { get; init; } = DirectionMode.FromShape;
    public FloatRange Direction { get; init; } = new(0, 360);
    public float Spread { get; init; } = 0f;
    public SpaceMode Space { get; init; } = SpaceMode.World;
    public float InheritVelocity { get; init; } = 0f;
    public CurveDefinition? InheritVelocityByDirection { get; init; }
}
