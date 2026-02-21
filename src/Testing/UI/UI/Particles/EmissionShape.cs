namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public abstract class EmissionShape
{
    protected readonly EmissionShapeDefinition Definition;
    protected readonly Matrix2D Transform;
    
    protected EmissionShape(EmissionShapeDefinition def)
    {
        Definition = def;
        Transform = Matrix2D.CreateTRS(def.Offset, def.Rotation * MathHelper.Pi / 180f, def.Scale);
    }
    
    public abstract EmissionPoint Sample(ParticleRandom random);
    public virtual void Update(float deltaTime, float totalTime) { }
    public virtual bool IsAnimated => false;
    public abstract float GetApproximateArea();
    public abstract BoundingBox2D GetBounds();
    
    protected Vector2 TransformPoint(Vector2 local) => Matrix2D.Transform(local, Transform);
    protected Vector2 TransformDirection(Vector2 local)
    {
        var dir = Matrix2D.TransformNormal(local, Transform);
        return dir.LengthSquared() > 0.0001f ? Vector2.Normalize(dir) : dir;
    }
}
