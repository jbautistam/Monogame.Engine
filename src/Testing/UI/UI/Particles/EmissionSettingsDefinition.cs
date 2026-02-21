namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / EMISSION SETTINGS
// ============================================================================

public sealed record EmissionSettingsDefinition
{
    public float Duration { get; init; } = 5f;
    public bool Looping { get; init; } = true;
    public float StartDelay { get; init; } = 0f;
    public float SimulationSpeed { get; init; } = 1f;
    public SimulationSpace Space { get; init; } = SimulationSpace.Local;
    public required EmissionShapeDefinition Shape { get; init; }
    public EmissionMode Mode { get; init; } = EmissionMode.Constant;
    
    public float RateOverTime { get; init; } = 10f;
    public float RateOverDistance { get; init; } = 0f;
    public List<BurstDefinition> Bursts { get; init; } = new();
    public float? Interval { get; init; }
    
    public int MaxParticles { get; init; } = 1000;
    public LimitAction OnLimitReached { get; init; } = LimitAction.KillOldest;
    public float? EmissionReductionFactor { get; init; }
    
    public enum EmissionMode { Constant, Burst, Interval, Probabilistic, Manual }
    public enum SimulationSpace { Local, World, Custom }
    public enum LimitAction { Wait, KillOldest, KillYoungest, KillNearest, KillRandom }
}
