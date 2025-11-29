
### 📍 `GridPoint.cs`

```csharp
using System;

public readonly struct GridPoint : IEquatable<GridPoint>
{
    public readonly int X;
    public readonly int Y;

    public GridPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(GridPoint other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj) => obj is GridPoint other && Equals(other);

#if NET6_0_OR_GREATER
    public override int GetHashCode() => HashCode.Combine(X, Y);
#else
    public override int GetHashCode() => X * 1000000 + Y;
#endif

    public static bool operator ==(GridPoint a, GridPoint b) => a.Equals(b);
    public static bool operator !=(GridPoint a, GridPoint b) => !a.Equals(b);

    public override string ToString() => $"({X}, {Y})";
}
```

---

### 🧱 `TileType.cs`

```csharp
```

---

### 🗺️ `GridMap.cs`

```csharp
```

---

### 🧭 `Node.cs`

```csharp
using Microsoft.Xna.Framework;

public class Node
{
    public TileType Type;
    public Vector2 WorldPosition;
    public GridPoint GridPos;
    public int GCost;
    public int HCost;
    public Node Parent;

    public int MoveCost => (int)Type;
    public bool Walkable => Type != TileType.Blocked;

    public Node(TileType type, Vector2 worldPos, GridPoint pos)
    {
        Type = type;
        WorldPosition = worldPos;
        GridPos = pos;
    }
}
```

---

### 🔑 `CanonicalRouteKey.cs`

```csharp
using System;

public readonly struct CanonicalRouteKey : IEquatable<CanonicalRouteKey>
{
    public readonly GridPoint Start;
    public readonly GridPoint End;

    public CanonicalRouteKey(GridPoint a, GridPoint b)
    {
        if (a.X < b.X || (a.X == b.X && a.Y <= b.Y))
        {
            Start = a;
            End = b;
        }
        else
        {
            Start = b;
            End = a;
        }
    }

    public bool Equals(CanonicalRouteKey other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override bool Equals(object obj) => obj is CanonicalRouteKey other && Equals(other);

#if NET6_0_OR_GREATER
    public override int GetHashCode() => HashCode.Combine(Start, End);
#else
    public override int GetHashCode() => Start.GetHashCode() * 397 ^ End.GetHashCode();
#endif
}
```

---

### 📦 `RouteCacheEntry.cs`

```csharp
using System.Collections.Generic;

public class RouteCacheEntry
{
    public List<GridPoint> GridPath { get; }
    public int LastAccessFrame { get; set; }
    public int AccessCount { get; set; }

    public RouteCacheEntry(List<GridPoint> gridPath, int currentFrame)
    {
        GridPath = new List<GridPoint>(gridPath);
        LastAccessFrame = currentFrame;
        AccessCount = 1;
    }

    public void Touch(int currentFrame)
    {
        LastAccessFrame = currentFrame;
        AccessCount++;
    }
}
```

---

### 🧠 `SmartRouteCache.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

public class SmartRouteCache
{
    private readonly Dictionary<CanonicalRouteKey, RouteCacheEntry> _cache = new();
    private readonly PathfindingManager _pathfinder;
    private readonly int _maxEntries;
    private readonly int _maxIdleFrames;
    private int _currentFrame = 0;

    public SmartRouteCache(PathfindingManager pathfinder, int maxEntries = 100, int maxIdleFrames = 300)
    {
        _pathfinder = pathfinder ?? throw new ArgumentNullException(nameof(pathfinder));
        _maxEntries = maxEntries;
        _maxIdleFrames = maxIdleFrames;
    }

    public void Update()
    {
        _currentFrame++;
        if (_currentFrame % 60 == 0)
        {
            PruneExpiredEntries();
        }
    }

    private void PruneExpiredEntries()
    {
        var deadKeys = new List<CanonicalRouteKey>();
        foreach (var kvp in _cache)
        {
            if ((_currentFrame - kvp.Value.LastAccessFrame) > _maxIdleFrames)
            {
                deadKeys.Add(kvp.Key);
            }
        }

        foreach (var key in deadKeys)
        {
            _cache.Remove(key);
        }

        while (_cache.Count > _maxEntries)
        {
            var lru = _cache.OrderBy(kvp => kvp.Value.LastAccessFrame)
                            .ThenBy(kvp => kvp.Value.AccessCount)
                            .First();
            _cache.Remove(lru.Key);
        }
    }

    private bool IsRouteStillValid(List<GridPoint> gridPath)
    {
        foreach (var p in gridPath)
        {
            if (!_pathfinder.IsWalkable(p))
                return false;
        }
        return true;
    }

    public List<Vector2> GetOrCreatePath(Vector2 startWorld, Vector2 endWorld)
    {
        GridPoint startGrid = _pathfinder.WorldToGrid(startWorld);
        GridPoint endGrid = _pathfinder.WorldToGrid(endWorld);

        if (startGrid == endGrid)
            return new List<Vector2> { _pathfinder.GridToWorld(startGrid) };

        var key = new CanonicalRouteKey(startGrid, endGrid);
        bool isReversed = startGrid == key.End;

        if (_cache.TryGetValue(key, out var entry))
        {
            if (IsRouteStillValid(entry.GridPath))
            {
                entry.Touch(_currentFrame);
                var worldPath = _pathfinder.GridPathToWorld(entry.GridPath);
                return isReversed ? worldPath.AsEnumerable().Reverse().ToList() : worldPath;
            }
            else
            {
                _cache.Remove(key);
            }
        }

        var subrouteGrid = ExtractSubrouteGrid(startGrid, endGrid);
        if (subrouteGrid != null && IsRouteStillValid(subrouteGrid))
        {
            return _pathfinder.GridPathToWorld(subrouteGrid);
        }

        var fullPathWorld = _pathfinder.FindPath(startWorld, endWorld);
        if (fullPathWorld == null) return null;

        var fullPathGrid = fullPathWorld.Select(wp => _pathfinder.WorldToGrid(wp)).ToList();
        var newEntry = new RouteCacheEntry(fullPathGrid, _currentFrame);
        _cache[key] = newEntry;

        return isReversed 
            ? fullPathWorld.AsEnumerable().Reverse().ToList()
            : new List<Vector2>(fullPathWorld);
    }

    private List<GridPoint> ExtractSubrouteGrid(GridPoint start, GridPoint end)
    {
        foreach (var entry in _cache.Values)
        {
            var path = entry.GridPath;
            int startIdx = path.IndexOf(start);
            int endIdx = path.IndexOf(end);

            if (startIdx >= 0 && endIdx > startIdx)
                return path.GetRange(startIdx, endIdx - startIdx + 1);

            if (endIdx >= 0 && startIdx > endIdx)
            {
                var sub = path.GetRange(endIdx, startIdx - endIdx + 1);
                sub.Reverse();
                return sub;
            }
        }

        return null;
    }
}
```

---

### 🧭 `PathfindingManager.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

public class PathfindingManager
{
    private Node[,] _nodes;
    private readonly GridMap _gridMap;

    private static readonly (int dx, int dy, int moveCost)[] Directions8 =
    {
        (0, -1, 1), (1, -1, 14), (1, 0, 1), (1, 1, 14),
        (0, 1, 1), (-1, 1, 14), (-1, 0, 1), (-1, -1, 14)
    };

    public PathfindingManager(GridMap gridMap)
    {
        _gridMap = gridMap ?? throw new ArgumentNullException(nameof(gridMap));
        RebuildGraph();
    }

    private void RebuildGraph()
    {
        _nodes = new Node[_gridMap.Width, _gridMap.Height];
        for (int x = 0; x < _gridMap.Width; x++)
        {
            for (int y = 0; y < _gridMap.Height; y++)
            {
                GridPoint pos = new GridPoint(x, y);
                Vector2 worldPos = _gridMap.ToWorld(pos);
                _nodes[x, y] = new Node(_gridMap.GetTile(x, y), worldPos, pos);
            }
        }
    }

    public void SetTile(int x, int y, TileType type)
    {
        _gridMap.SetTile(x, y, type);
        if (x >= 0 && x < _gridMap.Width && y >= 0 && y < _gridMap.Height)
        {
            _nodes[x, y].Type = type;
        }
    }

    public GridPoint WorldToGrid(Vector2 world) => _gridMap.ToGrid(world);
    public Vector2 GridToWorld(GridPoint p) => _gridMap.ToWorld(p);
    public List<Vector2> GridPathToWorld(List<GridPoint> gridPath) => _gridMap.ToWorld(gridPath);
    public bool IsWalkable(GridPoint p) => _gridMap.IsWalkable(p);
    public bool IsWalkable(int x, int y) => _gridMap.IsWalkable(x, y);

    private int GetHeuristicInt(Node a, Node b)
    {
        int dx = Math.Abs(a.GridPos.X - b.GridPos.X);
        int dy = Math.Abs(a.GridPos.Y - b.GridPos.Y);
        return dx > dy 
            ? 14 * dy + 10 * (dx - dy) 
            : 14 * dx + 10 * (dy - dx);
    }

    public List<Vector2> FindPath(Vector2 startWorld, Vector2 targetWorld)
    {
        GridPoint startGrid = WorldToGrid(startWorld);
        GridPoint targetGrid = WorldToGrid(targetWorld);

        Node start = _nodes[startGrid.X, startGrid.Y];
        Node target = _nodes[targetGrid.X, targetGrid.Y];

        if (!start.Walkable || !target.Walkable)
            return null;

        foreach (var node in _nodes)
        {
            node.GCost = int.MaxValue;
            node.Parent = null;
        }

        var openSet = new PriorityQueue<Node, int>();
        var closedSet = new HashSet<Node>();

        start.GCost = 0;
        start.HCost = GetHeuristicInt(start, target);
        openSet.Enqueue(start, start.FCost);

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            if (current == target)
                return ReconstructAndSmoothPath(start, target);

            closedSet.Add(current);

            foreach (var dir in Directions8)
            {
                int nx = current.GridPos.X + dir.dx;
                int ny = current.GridPos.Y + dir.dy;

                bool inBounds = nx >= 0 && nx < _gridMap.Width && ny >= 0 && ny < _gridMap.Height;
                if (inBounds)
                {
                    Node neighbor = _nodes[nx, ny];
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
                            {
                                openSet.Enqueue(neighbor, neighbor.FCost);
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    private List<Vector2> ReconstructAndSmoothPath(Node start, Node target)
    {
        var rawPath = new List<Node>();
        Node current = target;
        while (current != null)
        {
            rawPath.Add(current);
            current = current.Parent;
        }
        rawPath.Reverse();

        if (rawPath.Count <= 2)
            return rawPath.Select(n => n.WorldPosition).ToList();

        var smoothed = new List<Vector2> { rawPath[0].WorldPosition };
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
                {
                    j--;
                }
            }

            if (!jumpMade)
            {
                smoothed.Add(rawPath[i + 1].WorldPosition);
                i++;
            }
        }

        return smoothed;
    }

    private bool HasLineOfSight(Node from, Node to)
    {
        int x0 = from.GridPos.X, y0 = from.GridPos.Y;
        int x1 = to.GridPos.X, y1 = to.GridPos.Y;

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        bool finished = false;
        bool blocked = false;

        while (!finished && !blocked)
        {
            if (!_gridMap.IsWalkable(x0, y0))
            {
                blocked = true;
            }
            else if (x0 == x1 && y0 == y1)
            {
                finished = true;
            }
            else
            {
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        return !blocked && finished;
    }
}
