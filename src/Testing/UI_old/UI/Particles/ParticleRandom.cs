namespace ParticleEngine.Core;

// ============================================================================
// RANDOM DETERMINISTICO
// ============================================================================

public sealed class ParticleRandom
{
    private readonly Random _random;
    private readonly int _seed;
    
    public ParticleRandom(int seed = 0)
    {
        _seed = seed;
        _random = new Random(seed);
    }
    
    public float NextFloat() => (float)_random.NextDouble();
    public float NextFloat(float max) => NextFloat() * max;
    public float Range(float min, float max) => min + NextFloat() * (max - min);
    public int Next(int max) => _random.Next(max);
    public int Range(int min, int max) => _random.Next(min, max + 1);
    public Vector2 NextVector2() => new(NextFloat() * 2 - 1, NextFloat() * 2 - 1);
    public Vector2 InsideUnitCircle()
    {
        float angle = NextFloat() * MathHelper.TwoPi;
        float radius = MathF.Sqrt(NextFloat());
        return new Vector2(MathF.Cos(angle) * radius, MathF.Sin(angle) * radius);
    }
    
    public float NextFloat(int particleIndex)
    {
        // Deterministic por índice
        var temp = new Random(_seed + particleIndex);
        return (float)temp.NextDouble();
    }
}
