using Microsoft.Xna.Framework;

/// <summary>
///     Métodos de extensión para el tratamiento de ángulos
/// </summary>
public static class AngleExtensions
{
    /// <summary>
    ///     Pasa de grados a radianes
    /// </summary>
    public static float ToRadians(this float degrees) => MathHelper.ToRadians(degrees);

    /// <summary>
    ///     Pasa de radianes a grados
    /// </summary>
    public static float ToDegrees(this float radians) => MathHelper.ToDegrees(radians);

    /// <summary>
    ///     Lerp entre dos ángulos
    /// </summary>
    public static float AngleLerp(this float from, float to, float deltaTime)
    {
        float diff = to - from;

            // Normaliza el ángulo
            while (diff < -MathF.PI) 
                diff += MathF.Tau;
            while (diff > MathF.PI) 
                diff -= MathF.Tau;
            // Devuelve el ángulo
            return from + diff * MathHelper.Clamp(deltaTime, 0f, 1f);
    }
}