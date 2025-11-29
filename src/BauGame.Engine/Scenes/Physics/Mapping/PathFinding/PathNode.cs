using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Nodo de una ruta
/// </summary>
public class PathNode(GridMap.TileType type, Vector2 worldPosition, Point gridPosition)
{
    /// <summary>
    ///     Tipo de nodo
    /// </summary>
    public GridMap.TileType Type { get; } = type;

    /// <summary>
    ///     Posición en el mundo
    /// </summary>
    public Vector2 WorldPosition { get; } = worldPosition;

    /// <summary>
    ///     Posición en el grid
    /// </summary>
    public Point GridPos { get; } = gridPosition;

    /// <summary>
    ///     Coste G
    /// </summary>
    public int GCost { get; set; }

    /// <summary>
    ///     Coste F
    /// </summary>
    public int FCost { get; set; }

    /// <summary>
    ///     Coste H
    /// </summary>
    public int HCost { get; set; }

    /// <summary>
    ///     Nodo padre
    /// </summary>
    public PathNode? Parent { get; set; }

    /// <summary>
    ///     Coste de movimiento
    /// </summary>
    public int MoveCost => (int) Type;

    /// <summary>
    ///     Indica si se puede pasar por este nodo
    /// </summary>
    public bool Walkable => Type != GridMap.TileType.Blocked;
}
