namespace Bau.Libraries.BauGame.Engine.Core.Actors.Projectiles;

/// <summary>
///     Clase con las propiedades de un proyectil
/// </summary>
public class ProjectileProperties
{
    /// <summary>
    ///     Código de la textura
    /// </summary>
    public required string Texture { get; init; }

    /// <summary>
    ///     Región de la textura
    /// </summary>
    public required string Region { get; init; }

    /// <summary>
    ///     Velocidad
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    ///     Distancia máxima que puede recorrer el proyectil
    /// </summary>
    public required float MaxDistance { get; init; }

    /// <summary>
    ///     Daño que causa el proyectil
    /// </summary>
    public required int Damage { get; init; }
}
