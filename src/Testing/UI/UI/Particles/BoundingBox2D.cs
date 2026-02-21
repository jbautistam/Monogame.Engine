namespace ParticleEngine.Core;

public readonly struct BoundingBox2D
{
    public readonly Vector2 Min, Max;
    public BoundingBox2D(Vector2 min, Vector2 max) { Min = min; Max = max; }
    public bool Contains(Vector2 p) => p.X >= Min.X && p.X <= Max.X && p.Y >= Min.Y && p.Y <= Max.Y;
    public static BoundingBox2D Union(BoundingBox2D a, BoundingBox2D b) => new(
        new Vector2(MathF.Min(a.Min.X, b.Min.X), MathF.Min(a.Min.Y, b.Min.Y)),
        new Vector2(MathF.Max(a.Max.X, b.Max.X), MathF.Max(a.Max.Y, b.Max.Y)));
}
