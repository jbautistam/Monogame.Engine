namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class CompositeCurve : ParticleCurve
{
    private readonly (ParticleCurve curve, float start, float end)[] _segments;
    
    public CompositeCurve((ParticleCurve, float)[] weightedCurves)
    {
        float total = weightedCurves.Sum(w => w.Item2);
        _segments = new (ParticleCurve, float, float)[weightedCurves.Length];
        
        float current = 0;
        for (int i = 0; i < weightedCurves.Length; i++)
        {
            float norm = weightedCurves[i].Item2 / total;
            _segments[i] = (weightedCurves[i].Item1, current, current + norm);
            current += norm;
        }
    }
    
    public override float Evaluate(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        
        foreach (var (curve, start, end) in _segments)
        {
            if (t >= start && t <= end)
            {
                float localT = (t - start) / (end - start);
                return curve.Evaluate(localT);
            }
        }
        
        return _segments[^1].curve.Evaluate(1f);
    }
    
    public override (float min, float max) GetApproximateRange()
    {
        float globalMin = float.MaxValue, globalMax = float.MinValue;
        foreach (var (curve, _, _) in _segments)
        {
            var (min, max) = curve.GetApproximateRange();
            globalMin = MathF.Min(globalMin, min);
            globalMax = MathF.Max(globalMax, max);
        }
        return (globalMin, globalMax);
    }
}
