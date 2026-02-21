namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record CollisionModuleDefinition : ModuleDefinition
{
    public CollisionType Type { get; init; } = CollisionType.Planes;
    public List<PlaneDefinition> Planes { get; init; } = new();
    public uint LayerMask { get; init; } = 0xFFFFFFFF;
    public FloatRangeOrCurve Radius { get; init; } = new(0.1f, 0.1f);
    public CollisionResponse Response { get; init; } = CollisionResponse.Bounce;
    public float Bounciness { get; init; } = 0.5f;
    public float Friction { get; init; } = 0.1f;
    public float LifetimeLoss { get; init; } = 0f;
    public int MaxCollisions { get; init; } = 3;
    public bool Discrete { get; init; } = false;
    
    public enum CollisionType { Planes, Boxes, Circles, Tilemap, Physics2D, Heightfield }
    public enum CollisionResponse { Bounce, Stop, Die, Stick, Slide }
}
