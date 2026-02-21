namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class ConstantCurve : ParticleCurve
{
    public float Value { get; }
    public ConstantCurve(float value) => Value = value;
    public override float Evaluate(float t) => Value;
    public override (float min, float max) GetApproximateRange() => (Value, Value);
    public override bool IsConstant => true;
}
