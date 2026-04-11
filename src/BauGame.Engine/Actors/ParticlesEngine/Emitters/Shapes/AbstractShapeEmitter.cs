using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;

/// <summary>
///		Figura abstracta para emisores
/// </summary>
public abstract class AbstractShapeEmitter
{
    /// <summary>
    ///     Ubicación de la emisión
    /// </summary>
    public enum EmissionLocationMode
    {
        /// <summary>En la superficie de la figura por todo su área</summary>
        Surface,
        /// <summary>Desde el borde de la figura</summary>
        Border
    }
    /// <summary>
    ///     Dirección de la emisión
    /// </summary>
    public enum EmissionDirectionMode
    {
        /// <summary>Ángulo aleatorio total</summary>
        Random,
        /// <summary>Por la normal de la figura hacia afuera</summary>
        Outward,
        /// <summary>Por la normal de la figura hacia el centro</summary>
        Inward,
        /// <summary>Perpendicular a la normal siguiendo la línea</summary>
        Tangent,
        /// <summary>Un vector fijo definido por el usuario (para dirección fija como lluvia, por ejemplo</summary>
        Fixed
    }
    // Registro con los datos de emisión. Direction es un vector normalizado de longitud 1
    public record EmissionData(Vector2 Position, Vector2 Direction);

    /// <summary>
    ///     Obtiene los datos de emisión de la siguiente partícula con los valores de emisión predeterminados
    /// </summary>
    public EmissionData GetEmissionData(Vector2? fixedDirection = null)
    {
        return GetEmissionData(EmissionLocation, EmissionDirection, fixedDirection);
    }

    /// <summary>
    ///     Obtiene los datos de emisión de la siguiente partícula
    /// </summary>
    public abstract EmissionData GetEmissionData(EmissionLocationMode location, EmissionDirectionMode directionMode, Vector2? fixedDirection);

    /// <summary>
    ///     Resuelve una dirección
    /// </summary>
    protected Vector2 ResolveDirection(EmissionDirectionMode mode, Vector2 normal, Vector2? fixedDirection)
    {
        switch (mode)
        {
            case EmissionDirectionMode.Outward:
                return normal;
            case EmissionDirectionMode.Inward:
                return -normal;
            case EmissionDirectionMode.Tangent: // Rota la normal 90 grados (X, Y) -> (-Y, X)                
                return new Vector2(-normal.Y, normal.X);
            case EmissionDirectionMode.Fixed:
                return Vector2.Normalize(fixedDirection ?? Vector2.One);
            case EmissionDirectionMode.Random:
            default:
                float angle = (float) (Tools.Randomizer.Random.NextDouble() * MathHelper.TwoPi);

                    return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }
    }

    /// <summary>
    ///     Clona un objeto
    /// </summary>
    public abstract AbstractShapeEmitter Clone();

    /// <summary>
    ///     Ubicación de cómo se emiten las partículas (predefinida)
    /// </summary>
    public EmissionLocationMode EmissionLocation { get; set; } = EmissionLocationMode.Surface;

    /// <summary>
    ///     Dirección de la emisión (predefinida)
    /// </summary>
    public EmissionDirectionMode EmissionDirection { get; set; } = EmissionDirectionMode.Random;
}