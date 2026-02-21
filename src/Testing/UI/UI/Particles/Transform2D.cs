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
// RUNTIME / TRANSFORM & MOVEMENT
// ============================================================================

public sealed class Transform2D
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public float AngularVelocity { get; set; }
    public Vector2 AnimatedOffset { get; set; }
    
    public void PhysicsUpdate(float deltaTime)
    {
        Velocity += Acceleration * deltaTime;
        Position += Velocity * deltaTime;
        Rotation += AngularVelocity * deltaTime;
    }
    
    public Matrix2D GetAnimatedMatrix() => 
        Matrix2D.CreateTRS(Position + AnimatedOffset, Rotation, Scale);
}
