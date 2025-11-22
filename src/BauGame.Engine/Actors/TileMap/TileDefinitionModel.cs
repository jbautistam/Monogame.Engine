namespace Bau.Libraries.BauGame.Engine.Actors.TileMap;

/// <summary>
///     Definición de un patrón
/// </summary>
public class TileDefinitionModel(
{
    /// <summary>
    ///     Código del patrón
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    ///     Código de textura
    /// </summary>
    public required string Texture { get; init; }

    /// <summary>
    ///     Región dentro de la textura
    /// </summary>
    public required string Region { get; init; }

    /// <summary>
    ///     Código de definición
    /// </summary>
    public string? Animation { get; set; }
}