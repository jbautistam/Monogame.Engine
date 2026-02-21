namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class InitColorModule : IInitModule
{
    private readonly InitColorModuleDefinition _def;
    private readonly ParticleGradient _gradient;
    
    public InitColorModule(InitColorModuleDefinition def, IGradientCompiler compiler)
    {
        _def = def;
        _gradient = compiler.Compile(def.Color.Gradient);
    }
    
    public void Initialize(in ParticleRef p, in EmissionPoint emissionPoint, float emitterTime)
    {
        Color color = _def.InheritFromShape && emissionPoint.InheritColor.HasValue
            ? emissionPoint.InheritColor.Value
            : _gradient.Evaluate(0, p.Index);
        
        p.Color = ApplyVariations(color, p.Index);
    }
    
    private Color ApplyVariations(Color c, int seed)
    {
        var r = new Random(seed);
        
        // Simplificado: aplicar variaciones directamente en RGB
        float h = _def.HueVariation.GetValue(r) / 360f;
        float s = _def.SaturationVariation.GetValue(r);
        float v = _def.ValueVariation.GetValue(r);
        float a = _def.AlphaVariation.GetValue(r);
        
        return new Color(
            (byte)Math.Clamp(c.R + v * 255, 0, 255),
            (byte)Math.Clamp(c.G + s * 255, 0, 255),
            (byte)Math.Clamp(c.B + h * 255, 0, 255),
            (byte)Math.Clamp(c.A + a * 255, 0, 255));
    }
}
