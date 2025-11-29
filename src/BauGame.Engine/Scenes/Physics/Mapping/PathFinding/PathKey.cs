using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Clave canónica para una ruta
/// </summary>
public struct PathKey : IEquatable<PathKey>
{
    public PathKey(Point start, Point end)
    {
        if (start.X < end.X || (start.X == end.X && start.Y <= end.Y))
        {
            Start = start;
            End = end;
        }
        else
        {
            Start = end;
            End = start;
        }
    }

    /// <summary>
    ///     Comprueb si un <see cref="PathKey"/> es igual a otro
    /// </summary>
    public bool Equals(PathKey other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    ///     Compureba si es igual a un objeto
    /// </summary>
    public override bool Equals(object? obj) => obj is PathKey other && Equals(other);

    /// <summary>
    ///     Obtiene el código Hash
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    ///     Punto inicial de la ruta
    /// </summary>
    public Point Start { get; }

    /// <summary>
    ///     Punto final de la ruta
    /// </summary>
    public Point End { get; }
}
