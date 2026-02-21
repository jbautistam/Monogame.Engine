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
// RUNTIME / GRADIENTS
// ============================================================================

public abstract class ParticleGradient
{
    public abstract Color Evaluate(float t);
    public virtual Color Evaluate(float t, int seed) => Evaluate(t);
    public abstract (Color min, Color max) GetApproximateRange();
    public virtual bool IsRandomized => false;
    public abstract Texture2D GenerateLutTexture(GraphicsDevice device, int width);
}
