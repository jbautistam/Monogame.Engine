namespace EngineSample.Core.GameLogic.Scenes.TilesSample.Loaders.TmxFile;

/// <summary>
///     Clase para los tilesets asociados a un mapa
/// </summary>
public class MapTilesetModel
{
    /// <summary>
    ///     Primer Id de los tiles
    /// </summary>
    public required int FirstGid { get; init; }

    /// <summary>
    ///     Nombre del tileset
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Ancho del tile
    /// </summary>
    public required int TileWidth { get; init; }

    /// <summary>
    ///     Alto del tile
    /// </summary>
    public required int TileHeight { get; init; }

    /// <summary>
    ///     Ruta relativa a la imagen
    /// </summary>
    public string? ImageSource { get; set; } 

    /// <summary>
    ///     Ancho de la imagen
    /// </summary>
    public int ImageWidth { get; init; }

    /// <summary>
    ///     Altura de la imagen
    /// </summary>
    public int ImageHeight { get; init; }

    /// <summary>
    ///     Nombre del archivo TSX externo
    /// </summary>
    public string? TsxSource { get; set; } // Si viene de un .tsx externo
}