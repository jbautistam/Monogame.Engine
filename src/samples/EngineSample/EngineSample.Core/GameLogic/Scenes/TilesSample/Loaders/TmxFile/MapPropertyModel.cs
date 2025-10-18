namespace EngineSample.Core.GameLogic.Scenes.TilesSample.Loaders.TmxFile;

/// <summary>
///     Propiedad asociada a un elemento
/// </summary>
public class MapPropertyModel
{
    /// <summary>
    ///     Nombre de la propiedad
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Valor de la propiedad
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    ///     Tipo de la propiedad "string", "int", "float", "bool", etc.
    /// </summary>
    public required string Type { get; init; } 
}