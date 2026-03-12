namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class BezierCurve : ParticleCurve
{
    private readonly Vector2 _p0, _p1, _p2, _p3;
    private readonly bool _isCubic;
    
    public BezierCurve(Vector2 control, float startY, float endY)
    {
        _p0 = new Vector2(0, startY);
        _p1 = control;
        _p2 = Vector2.Zero;
        _p3 = new Vector2(1, endY);
        _isCubic = false;
    }
    
    public BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        _p0 = p0; _p1 = p1; _p2 = p2; _p3 = p3;
        _isCubic = true;
    }
    
    public override float Evaluate(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        
        if (!_isCubic)
        {
            float u = 1 - t;
            Vector2 p = u * u * _p0 + 2 * u * t * _p1 + t * t * _p3;
            return p.Y;
        }
        else
        {
            float u = 1 - t, u2 = u * u, t2 = t * t;
            Vector2 p = u2 * u * _p0 + 3 * u2 * t * _p1 + 3 * u * t2 * _p2 + t2 * t * _p3;
            return p.Y;
        }
    }
    
    public override (float min, float max) GetApproximateRange()
    {
        float min = Math.Min(Math.Min(_p0.Y, _p1.Y), Math.Min(_p2.Y, _p3.Y));
        float max = Math.Max(Math.Max(_p0.Y, _p1.Y), Math.Max(_p2.Y, _p3.Y));
        return (min, max);
    }
}
