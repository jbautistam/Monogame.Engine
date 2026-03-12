namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public interface IRenderModule
{
    void Render(ParticleRenderer renderer, in ParticleDataArrays data, int aliveCount);
}
