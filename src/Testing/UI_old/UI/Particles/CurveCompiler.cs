namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public sealed class CurveCompiler : ICurveCompiler
{
    private readonly Dictionary<CurveDefinition, ParticleCurve> _cache = new();
    
    public ParticleCurve Compile(CurveDefinition definition)
    {
        if (_cache.TryGetValue(definition, out var cached)) return cached;
        
        var curve = definition switch
        {
            ConstantCurveDefinition d => new ConstantCurve(d.Value),
            LinearCurveDefinition d => new LinearCurve(d.Start, d.End, d.Easing),
            BezierCurveDefinition d => d.ControlPoint2.HasValue
                ? new BezierCurve(d.Start, d.ControlPoint1, d.ControlPoint2.Value, d.End)
                : new BezierCurve(d.ControlPoint1, d.Start.Y, d.End.Y),
            KeyframeCurveDefinition d => new KeyframeCurve(d.Keyframes.Select(k => 
                new KeyframeCurve.Keyframe 
                { 
                    Time = k.Time, 
                    Value = k.Value, 
                    InTangent = k.InTangent, 
                    OutTangent = k.OutTangent,
                    Mode = (KeyframeCurve.TangentMode)k.Mode
                }).ToArray()),
            CompositeCurveDefinition d => new CompositeCurve(d.Segments.Select(s => 
                (Compile(s.Curve), s.Weight)).ToArray()),
            _ => throw new NotSupportedException()
        };
        
        _cache[definition] = curve;
        return curve;
    }
}
