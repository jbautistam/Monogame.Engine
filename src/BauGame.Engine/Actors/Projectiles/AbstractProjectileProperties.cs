namespace Bau.Libraries.BauGame.Engine.Actors.Projectiles;

/// <summary>
///		Propiedades base para proyectiles y explosiones
/// </summary>
public abstract class AbstractProjectileProperties
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
    ///     Nombre de la animación
    /// </summary>
    public string? Animation { get; set; }

    /// <summary>
    ///     Daño que causa
    /// </summary>
    public int Damage { get; set; }

    /// <summary>
    ///     Orden de dibujo
    /// </summary>
    public required int ZOrder { get; init; }

    /// <summary>
    ///     Indica si tiene alguna animación definida
    /// </summary>
	public bool HasAnimation => !string.IsNullOrWhiteSpace(Animation);
}
