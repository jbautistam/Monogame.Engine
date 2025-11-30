using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Nodo de una ruta para los cálculos de búsqueda
/// </summary>
public class PathNode(Point position)
{
    /// <summary>
    ///     Posición en el grid
    /// </summary>
    public Point Position { get; } = position;

    /// <summary>
    ///     Coste G
    /// </summary>
    public int GCost { get; set; } = int.MaxValue;

    /// <summary>
    ///     Coste H
    /// </summary>
    public int HCost { get; set; }

    /// <summary>
    ///     Nodo padre
    /// </summary>
    public PathNode? Parent { get; set; }
}
