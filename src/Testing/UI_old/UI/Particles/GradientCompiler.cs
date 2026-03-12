namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public sealed class GradientCompiler : IGradientCompiler
{
    private readonly ICurveCompiler _curveCompiler;
    private readonly Dictionary<GradientDefinition, ParticleGradient> _cache = new();
    
    public GradientCompiler(ICurveCompiler curveCompiler) => _curveCompiler = curveCompiler;
    
    public ParticleGradient Compile(GradientDefinition definition)
    {
        if (_cache.TryGetValue(definition, out var cached)) return cached;
        
        var gradient = definition switch
        {
            GradientDefinition d => new AnalyticGradient(
                d.ColorStops.Select(s => new GradientDefinition.ColorStop 
                { 
                    Time = s.Time, 
                    Color = s.Color 
                }).ToArray(),
                d.AlphaStops?.Select(s => new GradientDefinition.AlphaStop 
                { 
                    Time = s.Time, 
                    Alpha = s.Alpha, 
                    Easing = s.Easing 
                }).ToArray(),
                d.ColorSpace,
                d.PremultiplyAlpha),
            _ => throw new NotSupportedException()
        };
        
        _cache[definition] = gradient;
        return gradient;
    }
}
