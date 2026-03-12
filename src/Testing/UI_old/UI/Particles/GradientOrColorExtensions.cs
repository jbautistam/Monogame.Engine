namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public static class GradientOrColorExtensions
{
    public static GradientDefinition Gradient(this GradientOrColor goc) => goc switch
    {
        GradientDefinition g => g,
        _ => throw new InvalidOperationException()
    };
}
