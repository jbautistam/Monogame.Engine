using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Servicio de búsqueda de rutas con el algoritmo A*
/// </summary>
public class PathfindingService
{
    // Coste de movimientos
    private (int dx, int dy, int moveCost)[] Directions =
                            {
                                (0, -1, 10), (1, -1, 14), (1, 0, 10), (1, 1, 14),
                                (0, 1, 10), (-1, 1, 14), (-1, 0, 10), (-1, -1, 14)
                            };
    // Variables privadas
    private GridMap _gridMap;

    public PathfindingService(PathsCache pathsCache)
    {
        PathsCache = pathsCache;
        _gridMap = pathsCache.MapManager.GridMap;
    }

    /// <summary>
    ///     Busca una ruta entre dos puntos
    /// </summary>
    public List<Vector2> FindPath(Point startGrid, Point targetGrid)
    {
        // Si realmente es posible encontrar una ruta entre los puntos
        if (_gridMap.IsWalkable(startGrid) && _gridMap.IsWalkable(targetGrid))
        {
            PathNode start = new(startGrid);
            PathNode target = new(targetGrid);
            PriorityQueue<PathNode, int> openSet = new();
            HashSet<PathNode> closedSet = [];
            Dictionary<Point, PathNode> neighbors = [];

                // Inicializa el nodo inicial
                start.GCost = 0;
                start.HCost = GetMovementCost(start, target);
                // Encola el nodo inicial
                openSet.Enqueue(start, start.HCost);
                // Mientras quede algún nodo por buscar
                while (openSet.Count > 0)
                {
                    PathNode current = openSet.Dequeue();

                        // Si hemos llegado al final, devolvemos la ruta suavizada
                        if (current == target)
                            return Smooth(GetRawPath(start, target));
                        else
                        {
                            // Añadimos el nodo actual a la lista de nodos visitados
                            closedSet.Add(current);
                            // Buscamos el nodo vecino en todas las direcciones
                            foreach ((int dx, int dy, int moveCost) dir in Directions)
                            {
                                Point neighborPosition = new(current.Position.X + dir.dx, current.Position.Y + dir.dy);

                                    // Si realmente podemos llegar a ese punto
                                    if (_gridMap.IsWalkable(neighborPosition))
                                    {
                                        PathNode neighbor = GetNeighbor(neighbors, neighborPosition);

                                            // Si no hemos pasado ya por ese punto
                                            if (!closedSet.Contains(neighbor))
                                            {
                                                int tentativeG = current.GCost + _gridMap.MoveCost(neighbor.Position) * dir.moveCost;

                                                    // Si el coste es menor que el coste que teníamos hasta ahora
                                                    if (tentativeG < neighbor.GCost)
                                                    {
                                                        // Ajustamos los datos del nodo
                                                        neighbor.Parent = current;
                                                        neighbor.GCost = tentativeG;
                                                        neighbor.HCost = GetMovementCost(neighbor, target);
                                                        // Si no lo teníamos ya en la lista de nodos a visitar, lo encolamos
                                                        if (!openSet.UnorderedItems.Any(item => item.Element == neighbor))
                                                            openSet.Enqueue(neighbor, neighbor.HCost);
                                                    }
                                            }
                                    }
                         }
                    }
                }
        }
        // Si ha llegado hasta aquí es porque no hay una ruta válida
        return [];
    }

    /// <summary>
    ///     Obtiene / crea un nodo del diccionario en un punto
    /// </summary>
	private PathNode GetNeighbor(Dictionary<Point, PathNode> neighbors, Point point)
	{
		if (neighbors.TryGetValue(point, out PathNode? neighbor))
            return neighbor;
        else
        {
            PathNode node = new(point);

                // Añade el nodo al diccionario
                neighbors.Add(point, node);
                // Devuelve el nodo
                return node;
        }
	}

	/// <summary>
	///     Obtiene el peso de movimiento de un nodo a otro
	/// </summary>
	private int GetMovementCost(PathNode from, PathNode to)
    {
        int dx = Math.Abs(from.Position.X - to.Position.X);
        int dy = Math.Abs(from.Position.Y - to.Position.Y);

            if (dx > dy)
                return 14 * dy + 10 * (dx - dy);
            else
                return 14 * dx + 10 * (dy - dx);
    }

    /// <summary>
    ///     Suaviza una ruta
    /// </summary>
    private List<Vector2> Smooth(List<PathNode> rawPath)
    {
        List<Vector2> smoothed = [];

            // Obtiene la lista de puntos
            if (rawPath.Count <= 2)
                foreach (PathNode node in rawPath)
                    smoothed.Add(_gridMap.ToWorld(node.Position));
            else
            {
                int start = 0;
                int pathCount = rawPath.Count;

                    // Añade el primer punto
                    smoothed.Add(_gridMap.ToWorld(rawPath[0].Position));
                    // Recorre los puntos a partir del inicial
                    while (start < pathCount - 1)
                    {
                        int end = pathCount - 1;
                        bool jumpMade = false;

                            // Comprueba si hay un punto al que se pueda saltar fácilmente desde el inicio
                            while (end > start + 1 && !jumpMade)
                            {
                                if (_gridMap.HasLineOfSight(rawPath[start].Position, rawPath[end].Position))
                                {
                                    smoothed.Add(_gridMap.ToWorld(rawPath[end].Position));
                                    start = end;
                                    jumpMade = true;
                                }
                                else
                                    end--;
                            }
                            // Si se ha saltado, se añade el nuevo punto y se salta
                            if (!jumpMade)
                            {
                                smoothed.Add(_gridMap.ToWorld(rawPath[start + 1].Position));
                                start++;
                            }
                    }
            }
            // Devuelve la lista suavizada
            return smoothed;
    }

    /// <summary>
    ///     Obtiene una lista de nodos a partir del grafo de nodos creados
    /// </summary>
    private List<PathNode> GetRawPath(PathNode start, PathNode target)
    {
        List<PathNode> rawPath = [ target ];
        PathNode current = target;

            // Crea la lista
            while (current.Parent is not null)
            {
                current = current.Parent;
                rawPath.Add(current);
            }
            // Da la vuelta a la lista
            rawPath.Reverse();
            // Devuelve la lista creada
            return rawPath;
    }

    /// <summary>
    ///     Caché de rutas
    /// </summary>
    public PathsCache PathsCache { get; }
}
