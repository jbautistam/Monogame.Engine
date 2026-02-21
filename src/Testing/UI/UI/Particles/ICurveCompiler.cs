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
// RUNTIME / COMPILERS
// ============================================================================

public interface ICurveCompiler
{
    ParticleCurve Compile(CurveDefinition definition);
}
