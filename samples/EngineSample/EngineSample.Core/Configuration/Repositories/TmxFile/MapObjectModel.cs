using Microsoft.Xna.Framework;

namespace EngineSample.Core.Configuration.Repositories.TmxFile;

/// <summary>
///     Objeto asociado a un mapa
/// </summary>
public class MapObjectModel
{
    /// <summary>
    ///     Id del objeto
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Nombre del objeto
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Coordenada X
    /// </summary>
    public float X { get; set; }

    /// <summary>
    ///     Coordenada Y
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    ///     Ancho del objeto
    /// </summary>
    public float Width { get; set; }

    /// <summary>
    ///     Alto del objeto
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    ///     Propiedades del objeto
    /// </summary>
    public List<MapPropertyModel> Properties { get; } = [];

    /// <summary>
    ///     Puntos para polígonos (opcional)
    /// </summary>
    public List<Vector2> PolygonPoints { get; } = [];
}