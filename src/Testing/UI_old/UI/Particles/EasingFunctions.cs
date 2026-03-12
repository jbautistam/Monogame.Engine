namespace ParticleEngine.Core;

public static class EasingFunctions
{
    public static float Apply(float t, EasingType type) => type switch
    {
        EasingType.Linear => t,
        EasingType.SineIn => 1 - MathF.Cos(t * MathF.PI / 2),
        EasingType.SineOut => MathF.Sin(t * MathF.PI / 2),
        EasingType.SineInOut => -(MathF.Cos(MathF.PI * t) - 1) / 2,
        EasingType.QuadIn => t * t,
        EasingType.QuadOut => 1 - (1 - t) * (1 - t),
        EasingType.QuadInOut => t < 0.5f ? 2 * t * t : 1 - MathF.Pow(-2 * t + 2, 2) / 2,
        EasingType.CubicIn => t * t * t,
        EasingType.CubicOut => 1 - MathF.Pow(1 - t, 3),
        EasingType.CubicInOut => t < 0.5f ? 4 * t * t * t : 1 - MathF.Pow(-2 * t + 2, 3) / 2,
        EasingType.ExpoIn => t == 0 ? 0 : MathF.Pow(2, 10 * (t - 1)),
        EasingType.ExpoOut => t == 1 ? 1 : 1 - MathF.Pow(2, -10 * t),
        EasingType.BackIn => t * t * ((1.70158f + 1) * t - 1.70158f),
        EasingType.BackOut => 1 - MathF.Pow(1 - t, 2) * ((1.70158f + 1) * (1 - t) - 1.70158f),
        _ => t
    };
}
