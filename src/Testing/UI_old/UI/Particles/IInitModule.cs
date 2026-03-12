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
// RUNTIME / MODULE INTERFACES
// ============================================================================

public interface IInitModule
{
    void Initialize(in ParticleRef particle, in EmissionPoint emissionPoint, float emitterTime);
}
