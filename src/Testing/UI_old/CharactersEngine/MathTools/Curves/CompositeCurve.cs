using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.MathTools.Interpolation.Curves;

// Curva compuesta: concatena múltiples segmentos
public class CompositeCurve : AbstractCurve
{
    private readonly List<CurveSegment> _segments = new();
    private readonly float _totalDuration;
    private readonly float[] _cumulativeDurations;
    
    public CompositeCurve(params CurveSegment[] segments)
    {
        _segments.AddRange(segments);
        _totalDuration = _segments.Sum(s => s.Duration);
        
        // Normalizar duraciones a 0-1
        _cumulativeDurations = new float[_segments.Count + 1];
        _cumulativeDurations[0] = 0;
        for (int i = 0; i < _segments.Count; i++)
        {
            _cumulativeDurations[i + 1] = _cumulativeDurations[i] + _segments[i].Duration / _totalDuration;
        }
    }
    
    public override Vector2 Evaluate(float t)
    {
        // Encontrar segmento activo
        int segmentIndex = 0;
        for (int i = 0; i < _segments.Count; i++)
        {
            if (t >= _cumulativeDurations[i] && t <= _cumulativeDurations[i + 1])
            {
                segmentIndex = i;
                break;
            }
        }
        
        // Normalizar t al segmento
        float segStart = _cumulativeDurations[segmentIndex];
        float segEnd = _cumulativeDurations[segmentIndex + 1];
        float segDuration = segEnd - segStart;
        float localT = (t - segStart) / segDuration;

	    // Aplicar easing del segmento
	    CurveSegment seg = _segments[segmentIndex];
        float easedT = Easing.EasingFunctionsHelper.Interpolate(segStart, segEnd, localT, seg.Easing ?? Easing.EasingFunctionsHelper.EasingType.Linear);
        
        // Evaluar curva del segmento
        return seg.Curve.Evaluate(easedT);
    }
    
    // Para transiciones suaves entre segmentos
    public Vector2 EvaluateSmooth(float t, float blendWidth = 0.1f)
    {
        // Encontrar segmentos adyacentes para blending
        int segIdx = 0;
        for (int i = 0; i < _segments.Count; i++)
        {
            if (t < _cumulativeDurations[i + 1])
            {
                segIdx = i;
                break;
            }
        }
        
        // Si estamos en zona de transición, blendear con siguiente
        float segStart = _cumulativeDurations[segIdx];
        float segEnd = _cumulativeDurations[segIdx + 1];
        float localT = (t - segStart) / (segEnd - segStart);
        
        var result = Easing.EasingFunctionsHelper.Interpolate(segStart, segEnd, localT, _segments[segIdx].Easing ?? Easing.EasingFunctionsHelper.EasingType.Linear);

        // Blend con siguiente segmento si estamos cerca del final
        if (localT > 1 - blendWidth && segIdx < _segments.Count - 1)
        {
            float blendT = (localT - (1 - blendWidth)) / blendWidth;
            var nextResult = EvaluateSegment(segIdx + 1, 0);
            result = Vector2.Lerp(result, nextResult, blendT);
        }
        
        return result;
    }
    
    private Vector2 EvaluateSegment(int index, float localT)
    {
        var seg = _segments[index];
        float easedT = seg.Duration seg.Easing.Evaluate(localT);
        return seg.Curve.Evaluate(easedT);
    }
}