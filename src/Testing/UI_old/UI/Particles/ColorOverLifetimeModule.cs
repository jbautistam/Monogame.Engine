namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class ColorOverLifetimeModule : IUpdateModule
{
    private readonly ColorOverLifetimeModuleDefinition _def;
    private readonly ParticleGradient _gradient;
    
    public ColorOverLifetimeModule(ColorOverLifetimeModuleDefinition def, IGradientCompiler compiler)
    {
        _def = def;
        _gradient = compiler.Compile(def.Gradient);
    }
    
    public void Update(in ParticleRef p, float deltaTime, float emitterTime)
    {
        float t = _def.UseParticleTime ? p.NormalizedTime : (emitterTime % 1f);
        p.Color = _gradient.IsRandomized ? _gradient.Evaluate(t, p.Index) : _gradient.Evaluate(t);
    }
}
