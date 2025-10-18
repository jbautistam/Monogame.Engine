namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.TilesMap;

/// <summary>
///     Definición de un patrón
/// </summary>
public class TileDefinition
{
    /// <summary>
    ///     Código del patrón
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Código de textura
    /// </summary>
    public string Texture { get; set; } = default!;

    /// <summary>
    ///     Región dentro de la textura
    /// </summary>
    public string Region { get; set; } = default!;

    /// <summary>
    ///     Código de definición
    /// </summary>
    public string? Animation { get; set; }

    /// <summary>
    ///     Indica si es un patrón sólido
    /// </summary>
    public bool IsSolid { get; set; }
}