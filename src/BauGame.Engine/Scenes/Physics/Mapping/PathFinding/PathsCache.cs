using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Caché de rutas
/// </summary>
public class PathsCache
{
    // Variables privadas
    private Dictionary<PathKey, PathCacheEntry> _cache = [];
    private int _currentFrame = 0;

    public PathsCache(MapManager mapManager, int maxEntries = 100, int maxIdleFrames = 3_000)
    {
        MapManager = mapManager;
        PathfindingService = new PathfindingService(this);
        MaxEntries = maxEntries;
        MaxIdleFrames = maxIdleFrames;
    }

    public void Update(Managers.GameContext gameContext)
    {
        _currentFrame++;
        if (_currentFrame % 60 == 0)
            PruneExpiredEntries();
    }

    /// <summary>
    ///     Elimina las entradas expiradas
    /// </summary>
    private void PruneExpiredEntries()
    {
        List<PathKey> deadKeys = [];

        // Busca las rutas que se deben eliminar por tiempo
        foreach (KeyValuePair<PathKey, PathCacheEntry> keyValue in _cache)
            if (_currentFrame - keyValue.Value.LastAccessFrame > MaxIdleFrames)
                deadKeys.Add(keyValue.Key);
        // Elimina las rutas
        foreach (PathKey key in deadKeys)
            _cache.Remove(key);
        // Elimina las entradas que lleven más tiempo sin utilizarse
        while (_cache.Count > MaxEntries)
        {
			KeyValuePair<PathKey, PathCacheEntry> notUsed = _cache.OrderBy(kvp => kvp.Value.LastAccessFrame)
                                                                  .ThenBy(kvp => kvp.Value.AccessCount)
                                                                  .First();

                // Elimina la ruta
                _cache.Remove(notUsed.Key);
        }
    }

    /// <summary>
    ///     Comprueba si una ruta sigue siendo válida (porque el mapa puede variar)
    /// </summary>
    private bool IsRouteStillValid(List<Point> gridPath)
    {
        // Recorre los puntos comprobando que se pueda seguir pasando sobre ella
        foreach (Point point in gridPath)
            if (!MapManager.GridMap.IsWalkable(point))
                return false;
        // Si ha llegado hasta aquí es porque no se puede utilizar la ruta
        return true;
    }

    public List<Vector2> GetOrCreatePath(Vector2 startWorld, Vector2 endWorld)
    {
        Point startGrid = MapManager.GridMap.ToGrid(startWorld);
        Point endGrid = MapManager.GridMap.ToGrid(endWorld);

        if (startGrid == endGrid)
            return new List<Vector2> { MapManager.GridMap.ToWorld(startGrid) };

        var key = new PathKey(startGrid, endGrid);
        bool isReversed = startGrid == key.End;

        if (_cache.TryGetValue(key, out var entry))
        {
            if (IsRouteStillValid(entry.GridPath))
            {
                entry.Touch(_currentFrame);
                var worldPath = MapManager.GridMap.ToWorld(entry.GridPath);
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
            return MapManager.GridMap.ToWorld(subrouteGrid);
        }

        var fullPathWorld = PathfindingService.FindPath(startWorld, endWorld);
        if (fullPathWorld == null) return null;

        var fullPathGrid = fullPathWorld.Select(wp => MapManager.GridMap.ToGrid(wp)).ToList();
        var newEntry = new PathCacheEntry(fullPathGrid, _currentFrame);
        _cache[key] = newEntry;

        return isReversed 
            ? fullPathWorld.AsEnumerable().Reverse().ToList()
            : new List<Vector2>(fullPathWorld);
    }

    private List<Point> ExtractSubrouteGrid(Point start, Point end)
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
				List<Point> sub = path.GetRange(endIdx, startIdx - endIdx + 1);
                sub.Reverse();
                return sub;
            }
        }

        return null;
    }

    /// <summary>
    ///     Manager de mapas
    /// </summary>
    public MapManager MapManager { get; }

    /// <summary>
    ///     Servicio para generación de rutas
    /// </summary>
    public PathfindingService PathfindingService { get; }

    /// <summary>
    ///     Número máximo de entradas
    /// </summary>
    public int MaxEntries { get; set; }

    /// <summary>
    ///     Número máximo de frames que una ruta en la caché puede estar inactiva
    /// </summary>
    public int MaxIdleFrames { get; set; }
}
