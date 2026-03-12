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
// RUNTIME / DATA STRUCTURES
// ============================================================================

public struct ParticleDataArrays
{
    public Vector2[] Positions;
    public Vector2[] Velocities;
    public Color[] Colors;
    public Vector2[] Scales;
    public float[] Rotations;
    public float[] Ages;
    public float[] Lifetimes;
    public float[] NormalizedTimes;
    public int[] FrameIndices;
    public Vector2[] CustomData1;
    public Vector2[] CustomData2;
    public bool[] IsAlive;
    public byte[] StateFlags;
    
    public void Initialize(int capacity)
    {
        Positions = new Vector2[capacity];
        Velocities = new Vector2[capacity];
        Colors = new Color[capacity];
        Scales = new Vector2[capacity];
        Rotations = new float[capacity];
        Ages = new float[capacity];
        Lifetimes = new float[capacity];
        NormalizedTimes = new float[capacity];
        FrameIndices = new int[capacity];
        CustomData1 = new Vector2[capacity];
        CustomData2 = new Vector2[capacity];
        IsAlive = new bool[capacity];
        StateFlags = new byte[capacity];
    }
}
