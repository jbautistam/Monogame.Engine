namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record SubEmitterBinding
{
    public required SubEmitterTrigger Trigger { get; init; }
    public required ParticleSystemDefinition System { get; init; }
    public float Probability { get; init; } = 100f;
    public bool InheritPosition { get; init; } = true;
    public bool InheritRotation { get; init; } = true;
    public bool InheritVelocity { get; init; } = false;
    public float InheritVelocityScale { get; init; } = 1f;
    public bool InheritColor { get; init; } = false;
    public bool InheritSize { get; init; } = false;
    public Vector2 Offset { get; init; } = Vector2.Zero;
}
