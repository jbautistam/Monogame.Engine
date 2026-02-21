namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// VECTOR FIELD MODULE
public sealed record VectorFieldDefinition
{
    public required string TexturePath { get; init; }
    public float Strength { get; init; } = 1f;
    public SpaceMode Space { get; init; } = SpaceMode.World;
    public Vector2 Tiling { get; init; } = Vector2.One;
    public Vector2 Offset { get; init; } = Vector2.Zero;
    public AnimationMode Animation { get; init; } = AnimationMode.Scroll;
    public float ScrollSpeed { get; init; } = 0f;
    public float RotationSpeed { get; init; } = 0f;
    
    public enum AnimationMode { Static, Scroll, Rotate, Custom }
}
