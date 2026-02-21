namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public sealed class EmissionShapeCompiler : IEmissionShapeCompiler
{
    public EmissionShape Compile(EmissionShapeDefinition definition) => definition switch
    {
        PointShapeDefinition d => new PointShape(d),
        CircleShapeDefinition d => new CircleShape(d),
        RectangleShapeDefinition d => new RectangleShape(d),
        _ => throw new NotSupportedException()
    };
}
