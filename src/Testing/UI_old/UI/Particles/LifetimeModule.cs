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
// RUNTIME / MODULES - INIT
// ============================================================================

internal sealed class LifetimeModule : IInitModule
{
    private readonly LifetimeModuleDefinition _def;
    private readonly ParticleCurve? _scaleCurve;
    
    public LifetimeModule(LifetimeModuleDefinition def, ICurveCompiler compiler)
    {
        _def = def;
        _scaleCurve = def.LifetimeScaleCurve != null ? compiler.Compile(def.LifetimeScaleCurve) : null;
    }
    
    public void Initialize(in ParticleRef p, in EmissionPoint emissionPoint, float emitterTime)
    {
        float lifetime = _def.Lifetime.GetValue(emitterTime);
        if (_scaleCurve != null) lifetime *= _scaleCurve.Evaluate(p.Index / 1000f);
        p.Lifetime = MathF.Max(0.001f, lifetime);
    }
}
