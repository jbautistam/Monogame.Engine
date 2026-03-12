namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class KeyframeCurve : ParticleCurve
{
    private readonly Keyframe[] _keyframes;
    private readonly float[] _times;
    
    public struct Keyframe
    {
        public float Time, Value, InTangent, OutTangent;
        public TangentMode Mode;
    }
    
    public enum TangentMode { Auto, Linear, Constant, Free, Broken }
    
    public KeyframeCurve(Keyframe[] keyframes)
    {
        _keyframes = keyframes.OrderBy(k => k.Time).ToArray();
        _times = _keyframes.Select(k => k.Time).ToArray();
    }
    
    public override float Evaluate(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        
        int idx = Array.BinarySearch(_times, t);
        if (idx >= 0) return _keyframes[idx].Value;
        
        idx = ~idx - 1;
        if (idx < 0) return _keyframes[0].Value;
        if (idx >= _keyframes.Length - 1) return _keyframes[^1].Value;
        
        var k0 = _keyframes[idx];
        var k1 = _keyframes[idx + 1];
        float localT = (t - k0.Time) / (k1.Time - k0.Time);
        
        return HermiteInterpolate(k0, k1, localT);
    }
    
    private float HermiteInterpolate(Keyframe k0, Keyframe k1, float t)
    {
        float t2 = t * t, t3 = t2 * t;
        float h00 = 2 * t3 - 3 * t2 + 1;
        float h10 = t3 - 2 * t2 + t;
        float h01 = -2 * t3 + 3 * t2;
        float h11 = t3 - t2;
        
        float dt = k1.Time - k0.Time;
        float m0 = k0.OutTangent * dt;
        float m1 = k1.InTangent * dt;
        
        return h00 * k0.Value + h10 * m0 + h01 * k1.Value + h11 * m1;
    }
    
    public override (float min, float max) GetApproximateRange()
    {
        float min = float.MaxValue, max = float.MinValue;
        foreach (var k in _keyframes)
        {
            min = MathF.Min(min, k.Value);
            max = MathF.Max(max, k.Value);
        }
        return (min, max);
    }
}
