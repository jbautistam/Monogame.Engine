using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Servicio de búsqueda de rutas con el algoritmo A*
/// </summary>
public class PathfindingService
{
    // Coste de movimientos
    private (int dx, int dy, int moveCost)[] Directions8 =
                            {
                                (0, -1, 1), (1, -1, 14), (1, 0, 1), (1, 1, 14),
                                (0, 1, 1), (-1, 1, 14), (-1, 0, 1), (-1, -1, 14)
                            };
    // Variables privadas
    private PathNode[ , ] _nodes;
    private GridMap _gridMap;

    public PathfindingService(PathsCache pathsCache)
    {
        PathsCache = pathsCache;
        _gridMap = pathsCache.MapManager.GridMap;
        RebuildGraph();
    }

    private void RebuildGraph()
    {
        _nodes = new PathNode[_gridMap.Width, _gridMap.Height];
        for (int x = 0; x < _gridMap.Width; x++)
        {
            for (int y = 0; y < _gridMap.Height; y++)
            {
                Point pos = new(x, y);
                Vector2 worldPos = _gridMap.ToWorld(pos);
                _nodes[x, y] = new PathNode(_gridMap.GetTile(x, y), worldPos, pos);
            }
        }
    }

    private int GetHeuristicInt(PathNode a, PathNode b)
    {
        int dx = Math.Abs(a.GridPos.X - b.GridPos.X);
        int dy = Math.Abs(a.GridPos.Y - b.GridPos.Y);
        return dx > dy 
            ? 14 * dy + 10 * (dx - dy) 
            : 14 * dx + 10 * (dy - dx);
    }

    public List<Vector2> FindPath(Vector2 startWorld, Vector2 endWorld)
    {
        Point startGrid = _gridMap.ToGrid(startWorld);
        Point targetGrid = _gridMap.ToGrid(endWorld);

        PathNode start = _nodes[startGrid.X, startGrid.Y];
        PathNode target = _nodes[targetGrid.X, targetGrid.Y];

        if (!start.Walkable || !target.Walkable)
            return null;

        foreach (var node in _nodes)
        {
            node.GCost = int.MaxValue;
            node.Parent = null;
        }

        PriorityQueue<PathNode, int> openSet = new();
        HashSet<PathNode> closedSet = [];

        start.GCost = 0;
        start.HCost = GetHeuristicInt(start, target);
        openSet.Enqueue(start, start.FCost);

        while (openSet.Count > 0)
        {
            PathNode current = openSet.Dequeue();

            if (current == target)
                return ReconstructAndSmoothPath(start, target);

            closedSet.Add(current);

            foreach ((int dx, int dy, int moveCost) dir in Directions8)
            {
                int nx = current.GridPos.X + dir.dx;
                int ny = current.GridPos.Y + dir.dy;

                bool inBounds = nx >= 0 && nx < _gridMap.Width && ny >= 0 && ny < _gridMap.Height;
                if (inBounds)
                {
                    PathNode neighbor = _nodes[nx, ny];
                    bool isWalkable = neighbor.Walkable;
                    bool notClosed = !closedSet.Contains(neighbor);

                    if (isWalkable && notClosed)
                    {
                        int moveCostFactor = dir.moveCost == 1 ? 10 : 14;
                        int tentativeG = current.GCost + neighbor.MoveCost * moveCostFactor;

                        if (tentativeG < neighbor.GCost)
                        {
                            neighbor.Parent = current;
                            neighbor.GCost = tentativeG;
                            neighbor.HCost = GetHeuristicInt(neighbor, target);

                            bool alreadyInOpen = openSet.UnorderedItems.Any(i => i.Element == neighbor);
                            if (!alreadyInOpen)
                                openSet.Enqueue(neighbor, neighbor.FCost);
                        }
                    }
                }
            }
        }

        return null;
    }

    private List<Vector2> ReconstructAndSmoothPath(PathNode start, PathNode target)
    {
        List<PathNode> rawPath = [ target ];
        PathNode current = target;

        while (current.Parent is not null)
        {
            current = current.Parent;
            rawPath.Add(current);
        }
        rawPath.Reverse();

        if (rawPath.Count <= 2)
            return rawPath.Select(n => n.WorldPosition).ToList();

        List<Vector2> smoothed = [ rawPath[0].WorldPosition ];
        int i = 0;
        int pathCount = rawPath.Count;

        while (i < pathCount - 1)
        {
            int j = pathCount - 1;
            bool jumpMade = false;

            while (j > i + 1 && !jumpMade)
            {
                if (HasLineOfSight(rawPath[i], rawPath[j]))
                {
                    smoothed.Add(rawPath[j].WorldPosition);
                    i = j;
                    jumpMade = true;
                }
                else
                    j--;
            }

            if (!jumpMade)
            {
                smoothed.Add(rawPath[i + 1].WorldPosition);
                i++;
            }
        }

        return smoothed;
    }

    /// <summary>
    ///     Comprueba si hay una línea de visión abierta entre dos nodos
    /// </summary>
    private bool HasLineOfSight(PathNode from, PathNode to)
    {
        int x0 = from.GridPos.X, y0 = from.GridPos.Y;
        int x1 = to.GridPos.X, y1 = to.GridPos.Y;
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int error = dx - dy;
        bool finished = false;
        bool blocked = false;

            // Intenta dibujar una recta entre los dos puntos
            while (!finished && !blocked)
            {
                if (!_gridMap.IsWalkable(x0, y0))
                    blocked = true;
                else if (x0 == x1 && y0 == y1)
                    finished = true;
                else
                {
                    int errorDuplicate = 2 * error;

                        // Corrige el error del desplazamiento Y
                        if (errorDuplicate > -dy)
                        {
                            error -= dy;
                            x0 += sx;
                        }
                        // Corrige el error del desplazamiento X
                        if (errorDuplicate < dx)
                        {
                            error += dx;
                            y0 += sy;
                        }
                }
            }
            // Devuelve el valor que indica si hay una línea de visión
            return !blocked && finished;
    }

    /// <summary>
    ///     Caché de routas
    /// </summary>
    public PathsCache PathsCache { get; }
}
