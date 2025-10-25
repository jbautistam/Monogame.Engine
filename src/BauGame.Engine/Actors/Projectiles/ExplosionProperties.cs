namespace Bau.Libraries.BauGame.Engine.Actors.Projectiles;

/// <summary>
///     Clase con las propiedades de una explosión
/// </summary>
public class ExplosionProperties : AbstractProjectileProperties
{
    /// <summary>
    ///     Velocidad
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    ///     Duración de la explosión
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    ///     Radio de la explosión
    /// </summary>
    public float Radius { get; set; }

    /// <summary>
    ///     Indica si provoca daños sólo cuando se crea
    /// </summary>
    public bool DamagesOnlyOnCreation { get; set; }
    
    /// <summary>
    ///     Indica si se deben aplicar fuerzas con la explosión
    /// </summary>
    public bool AppliesForce { get; set; }

    /// <summary>
    ///     Fuerza
    /// </summary>
    public float ForceStrength { get; set; }
}
