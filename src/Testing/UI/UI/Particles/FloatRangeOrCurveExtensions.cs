namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

// ============================================================================
// UTILS
// ============================================================================

public static class FloatRangeOrCurveExtensions
{
    public static float GetValue(this FloatRangeOrCurve rangeOrCurve, int seed) => rangeOrCurve switch
    {
        ConstantCurveDefinition d => d.Value,
        LinearCurveDefinition d => new Random(seed).NextSingle() * (d.End - d.Start) + d.Start,
        _ => 0
    };
}
