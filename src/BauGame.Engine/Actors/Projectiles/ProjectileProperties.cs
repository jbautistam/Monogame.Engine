namespace Bau.Libraries.BauGame.Engine.Actors.Projectiles;

/// <summary>
///     Clase con las propiedades de un proyectil
/// </summary>
public class ProjectileProperties : AbstractProjectileProperties
{
    /// <summary>
    ///     Velocidad
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    ///     Distancia máxima que puede recorrer el proyectil
    /// </summary>
    public required float MaxDistance { get; init; }

    /// <summary>
    ///     Indica si se debe mostrar la explosión cuando finalice la distancia (aunque no haya colisionado con nada)
    /// </summary>
    public bool ShowExplosionWhenEndDistance { get; set; }

    /// <summary>
    ///     Explosión
    /// </summary>
    public ExplosionProperties? Explosion { get; set; }
}
