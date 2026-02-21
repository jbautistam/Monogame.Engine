namespace ParticleEngine.Core;

public readonly record struct ColorRange(Color Min, Color Max)
{
    public Color GetValue(Random random) => Color.Lerp(Min, Max, (float)random.NextDouble());
}
