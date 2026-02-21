namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class LinearCurve : ParticleCurve
{
    public float Start { get; }
    public float End { get; }
    public EasingType Easing { get; }
    
    public LinearCurve(float start, float end, EasingType easing)
    {
        Start = start; End = end; Easing = easing;
    }
    
    public override float Evaluate(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        float easedT = EasingFunctions.Apply(t, Easing);
        return Start + (End - Start) * easedT;
    }
    
    public override (float min, float max) GetApproximateRange() => 
        Start < End ? (Start, End) : (End, Start);
}
