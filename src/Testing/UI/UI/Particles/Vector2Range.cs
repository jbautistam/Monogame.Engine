namespace ParticleEngine.Core;

public readonly record struct Vector2Range(Vector2 Min, Vector2 Max)
{
    public Vector2 GetValue(Random random) => new(
        Min.X + (float)(random.NextDouble() * (Max.X - Min.X)),
        Min.Y + (float)(random.NextDouble() * (Max.Y - Min.Y)));
}
