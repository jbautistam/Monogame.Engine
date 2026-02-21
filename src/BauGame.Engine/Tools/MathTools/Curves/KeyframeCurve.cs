using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Curves;

// ==================== CURVA POR KEYFRAMES ====================
public class KeyframeCurve : AbstractCurve
{
    private readonly List<Keyframe> _keyframes;
    private readonly bool _closed; // Curva cerrada (loop)
    
    public KeyframeCurve(params Keyframe[] keyframes)
    {
        _keyframes = keyframes.OrderBy(k => k.Time).ToList();
        _closed = false;
        
        // Calcular tangentes automáticas si no se especificaron
        ComputeAutoTangents();
    }
    
    public KeyframeCurve(bool closed, params Keyframe[] keyframes)
    {
        _keyframes = keyframes.OrderBy(k => k.Time).ToList();
        _closed = closed;
        
        if (closed && _keyframes.Count > 2)
        {
            // Duplicar primer/último para continuidad
            var first = _keyframes[0];
            var last = _keyframes[_keyframes.Count - 1];
            // Ajustar tiempos para loop
        }
        
        ComputeAutoTangents();
    }
    
    public override Vector2 Evaluate(float t)
    {
        if (_keyframes.Count == 0) return Vector2.Zero;
        if (_keyframes.Count == 1) return _keyframes[0].Position;
        
        // Encontrar segmento
        int idx = 0;
        for (int i = 0; i < _keyframes.Count - 1; i++)
        {
            if (t >= _keyframes[i].Time && t <= _keyframes[i + 1].Time)
            {
                idx = i;
                break;
            }
        }
        
        // Si estamos más allá del último
        if (t >= _keyframes[_keyframes.Count - 1].Time)
            return _keyframes[_keyframes.Count - 1].Position;
        
        var k0 = _keyframes[idx];
        var k1 = _keyframes[idx + 1];
        
        // Normalizar t al segmento
        float segDuration = k1.Time - k0.Time;
        float localT = (t - k0.Time) / segDuration;
        
        // Aplicar easing del keyframe inicial
        float easedT = Easing.EasingFunctionsHelper.Interpolate(0, 1, localT, k0.Easing ?? Easing.EasingFunctionsHelper.EasingType.Linear);
        
        // Interpolación Hermite (cúbica con tangentes)
        return HermiteInterpolate(k0, k1, easedT);
    }
    
    private Vector2 HermiteInterpolate(Keyframe k0, Keyframe k1, float t)
    {
        Vector2 p0 = k0.Position;
        Vector2 p1 = k1.Position;
        
        // Tangentes escaladas por duración del segmento
        float dt = k1.Time - k0.Time;
        Vector2 m0 = (k0.TangentOut ?? Vector2.Zero) * dt;
        Vector2 m1 = (k1.TangentIn ?? Vector2.Zero) * dt;
        
        // Polinomios de Hermite
        float t2 = t * t;
        float t3 = t2 * t;
        
        float h00 = 2 * t3 - 3 * t2 + 1;
        float h10 = t3 - 2 * t2 + t;
        float h01 = -2 * t3 + 3 * t2;
        float h11 = t3 - t2;
        
        return h00 * p0 + h10 * m0 + h01 * p1 + h11 * m1;
    }
    
    // Calcular tangentes automáticas (Catmull-Rom)
    private void ComputeAutoTangents()
    {
        for (int i = 0; i < _keyframes.Count; i++)
        {
            var k = _keyframes[i];
            
            // Si no tiene tangente saliente
            if (!k.TangentOut.HasValue && i < _keyframes.Count - 1)
            {
                Vector2 tangent = ComputeCatmullRomTangent(i);
                _keyframes[i] = new Keyframe(
                    k.Time, k.Position, 
                    k.TangentIn, 
                    tangent * 0.5f, // Escalar para suavidad
                    k.Easing);
            }
            
            // Si no tiene tangente entrante
            if (!k.TangentIn.HasValue && i > 0)
            {
                Vector2 tangent = ComputeCatmullRomTangent(i);
                _keyframes[i] = new Keyframe(
                    k.Time, k.Position,
                    tangent * 0.5f,
                    k.TangentOut,
                    k.Easing);
            }
        }
    }
    
    private Vector2 ComputeCatmullRomTangent(int index)
    {
        Vector2 prev = index > 0 ? _keyframes[index - 1].Position : _keyframes[0].Position;
        Vector2 curr = _keyframes[index].Position;
        Vector2 next = index < _keyframes.Count - 1 ? _keyframes[index + 1].Position : _keyframes[_keyframes.Count - 1].Position;
        
        // Tangente como promedio de vectores adyacentes
        return (next - prev) * 0.5f;
    }
    
    // Derivada para rotación automática
    public Vector2 Derivative(float t)
    {
        // Aproximación numérica o analítica de Hermite
        float dt = 0.001f;
        Vector2 p0 = Evaluate(t - dt);
        Vector2 p1 = Evaluate(t + dt);
        return (p1 - p0) / (2 * dt);
    }
}
