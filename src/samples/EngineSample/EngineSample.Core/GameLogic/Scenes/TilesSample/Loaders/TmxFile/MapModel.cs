namespace EngineSample.Core.GameLogic.Scenes.TilesSample.Loaders.TmxFile;

/// <summary>
///     Definición del mapa
/// </summary>
public class MapModel
{
    /// <summary>
    ///     Orientación: ortogonal, isométrico...
    /// </summary>
    public required string Orientation { get; init; }

    /// <summary>
    ///     Ancho en tiles
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    ///     Alto en tiles
    /// </summary>
    public required int Height { get; init; }

    /// <summary>
    ///     Ancho de los tiles en pixel
    /// </summary>
    public required int TileWidth { get; init; }

    /// <summary>
    ///     Alto de los tiles en pixel
    /// </summary>
    public required int TileHeight { get; init; }

    /// <summary>
    ///     Conjuntos de tiles
    /// </summary>
    public List<MapTilesetModel> Tilesets { get; } = [];

    /// <summary>
    ///     Capas del mapa
    /// </summary>
    public List<MapLayerModel> Layers { get; } = [];

    /// <summary>
    ///     Grupos de objetos del mapa
    /// </summary>
    public List<MapObjectGroupModel> ObjectGroups { get; } = [];

    /// <summary>
    ///     Propiedades del mapa
    /// </summary>
    public List<MapPropertyModel> Properties { get; } = [];
}