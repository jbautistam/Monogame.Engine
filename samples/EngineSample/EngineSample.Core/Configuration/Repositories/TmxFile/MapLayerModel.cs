namespace EngineSample.Core.Configuration.Repositories.TmxFile;

/// <summary>
///     Datos de la capa de un mapa
/// </summary>
public class MapLayerModel
{
    /// <summary>
    ///     Nombre de la capa
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Ancho de la capa
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    ///     Alto de la capa
    /// </summary>
    public required int Height { get; init; }

    /// <summary>
    ///     Códigos de tiles: fila por fila
    /// </summary>
    public List<int> Tiles { get; } = [];

    /// <summary>
    ///     Propiedades de la capa
    /// </summary>
    public List<MapPropertyModel> Properties { get; } = [];
}
