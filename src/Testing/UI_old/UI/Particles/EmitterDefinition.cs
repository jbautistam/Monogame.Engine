namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record EmitterDefinition
{
    public string? Name { get; init; } = "Emitter";
    public required EmissionSettingsDefinition Emission { get; init; }
    public List<ModuleDefinition> InitModules { get; init; } = new();
    public List<ModuleDefinition> UpdateModules { get; init; } = new();
    public List<ModuleDefinition> RenderModules { get; init; } = new();
    public MaterialDefinition? Material { get; init; }
    public int SortingOrder { get; init; } = 0;
    public bool Enabled { get; init; } = true;
    public float StartDelay { get; init; } = 0f;
    public AnimatedOffsetDefinition? AnimatedOffset { get; init; }
    public bool IndependentRotation { get; init; } = false;
    public CurveDefinition? RotationCurve { get; init; }
}
