namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public readonly ref struct ParticleRef
{
    public readonly ParticleDataArrays Arrays;
    public readonly int Index;
    
    public ParticleRef(ParticleDataArrays arrays, int index)
    {
        Arrays = arrays;
        Index = index;
    }
    
    public ref Vector2 Position => ref Arrays.Positions[Index];
    public ref Vector2 Velocity => ref Arrays.Velocities[Index];
    public ref Color Color => ref Arrays.Colors[Index];
    public ref Vector2 Scale => ref Arrays.Scales[Index];
    public ref float Rotation => ref Arrays.Rotations[Index];
    public ref float Age => ref Arrays.Ages[Index];
    public ref float Lifetime => ref Arrays.Lifetimes[Index];
    public ref float NormalizedTime => ref Arrays.NormalizedTimes[Index];
    public ref int FrameIndex => ref Arrays.FrameIndices[Index];
    public ref Vector2 Custom1 => ref Arrays.CustomData1[Index];
    public ref Vector2 Custom2 => ref Arrays.CustomData2[Index];
    public ref bool IsAlive => ref Arrays.IsAlive[Index];
}
