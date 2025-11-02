namespace EngineSample.Core.Configuration.Repositories.TmxFile;

/// <summary>
///     Grupo del objeto
/// </summary>
public class MapObjectGroupModel
{
    /// <summary>
    ///     Nombre del grupo de objetos
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Objetos del grupo
    /// </summary>
    public List<MapObjectModel> Objects { get; } = [];

    /// <summary>
    ///     Propiedades del grupo
    /// </summary>
    public List<MapPropertyModel> Properties { get; } = [];
}