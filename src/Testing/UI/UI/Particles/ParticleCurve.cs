namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public abstract class ParticleCurve
{
    public abstract float Evaluate(float t);
    public float EvaluateClamped(float t) => Evaluate(Math.Clamp(t, 0f, 1f));
    public abstract (float min, float max) GetApproximateRange();
    public virtual bool IsConstant => false;
}
