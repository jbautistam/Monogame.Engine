namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

public sealed record KeyframeCurveDefinition : CurveDefinition
{
    public required List<Keyframe> Keyframes { get; init; }
    public WrapMode PreWrapMode { get; init; } = WrapMode.Clamp;
    public WrapMode PostWrapMode { get; init; } = WrapMode.Clamp;
    
    public sealed record Keyframe
    {
        public required float Time { get; init; }
        public required float Value { get; init; }
        public float InTangent { get; init; }
        public float OutTangent { get; init; }
        public TangentMode Mode { get; init; } = TangentMode.Auto;
    }
    
    public enum TangentMode { Auto, Linear, Constant, Free, Broken }
    public enum WrapMode { Clamp, Loop, PingPong, Once }
}
