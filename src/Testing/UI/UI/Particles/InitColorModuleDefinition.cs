namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record InitColorModuleDefinition : ModuleDefinition
{
    public required GradientOrColor Color { get; init; }
    public FloatRange HueVariation { get; init; } = new(0, 0);
    public FloatRange SaturationVariation { get; init; } = new(0, 0);
    public FloatRange ValueVariation { get; init; } = new(0, 0);
    public FloatRange AlphaVariation { get; init; } = new(0, 0);
    public bool InheritFromShape { get; init; } = false;
}
