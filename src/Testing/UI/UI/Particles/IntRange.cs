namespace ParticleEngine.Core;

public readonly record struct IntRange(int Min, int Max)
{
    public int GetValue(Random random) => random.Next(Min, Max + 1);
}
