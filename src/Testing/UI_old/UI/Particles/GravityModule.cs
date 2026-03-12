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
// RUNTIME / MODULES - UPDATE
// ============================================================================

internal sealed class GravityModule : IUpdateModule
{
    private readonly GravityModuleDefinition _def;
    private readonly ParticleCurve? _scaleCurve;
    
    public GravityModule(GravityModuleDefinition def, ICurveCompiler compiler)
    {
        _def = def;
        _scaleCurve = def.ScaleOverLifetime != null ? compiler.Compile(def.ScaleOverLifetime) : null;
    }
    
    public void Update(in ParticleRef p, float deltaTime, float emitterTime)
    {
        Vector2 gravity = _def.Acceleration;
        if (_scaleCurve != null) gravity *= _scaleCurve.Evaluate(p.NormalizedTime);
        p.Velocity += gravity * deltaTime;
    }
}
