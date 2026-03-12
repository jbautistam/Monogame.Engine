namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record AnimatedShapeDefinition : EmissionShapeDefinition
{
    public required EmissionShapeDefinition StartShape { get; init; }
    public required EmissionShapeDefinition EndShape { get; init; }
    public required float Duration { get; init; }
    public CurveDefinition? BlendCurve { get; init; }
    public bool Loop { get; init; } = true;
    public AnimationMode Mode { get; init; } = AnimationMode.Blend;
    
    public enum AnimationMode { Blend, Switch, ParticleAgeBased }
}
