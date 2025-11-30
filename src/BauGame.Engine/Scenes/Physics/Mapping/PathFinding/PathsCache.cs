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
    ///     Obtiene una ruta
    /// </summary>
    public List<Vector2> Get(Vector2 startWorld, Vector2 endWorld)
    {
        Point startGrid = MapManager.GridMap.ToGrid(startWorld);
        Point endGrid = MapManager.GridMap.ToGrid(endWorld);
        List<Vector2> result = [];
        bool isReversed = false;

            // Si estamos en el mismo punto, sólo hay una ruta válida, si no es así, obtenemos la ruta
            if (startGrid == endGrid)
                result = [ MapManager.GridMap.ToWorld(startGrid) ];
            else
            {
                PathKey key = new(startGrid, endGrid);

                    // Indica si se tiene que dar la vuelta al resultado
                    isReversed = startGrid == key.End;
                    // Obtiene la lista de la caché
                    result = GetFromCache(key);
                    // Si no hay nada en la lista, se obtiene la ruta del servicio de creación de rutas
                    if (result.Count == 0)
                        result = PathfindingService.FindPath(startGrid, endGrid);
                    // Se añade la ruta a la caché
                    if (result.Count > 0)
                        _cache[key] = new PathCacheEntry(result, _currentFrame);
            }
            // Se devuelve la ruta (invertida si es necesario
            if (isReversed)
                return result.AsEnumerable().Reverse().ToList();
            else
                return result;
    }

    /// <summary>
    ///     Obtiene una ruta de la caché
    /// </summary>
    private List<Vector2> GetFromCache(PathKey key)
    {
        // Busca el elemento en la caché
        if (_cache.TryGetValue(key, out PathCacheEntry? entry))
        {
            // Si sigue siendo una ruta válida, devuelve la ruta
            if (IsRouteStillValid(entry.Positions))
            {
                // Indica que en este frame, se ha obtenido el dato de la caché
                entry.Touch(_currentFrame);
                // Devuelve la ruta de caché
                return entry.Positions;
            }
            else
                _cache.Remove(key);
        }
        // Si no había ningún elemento en la caché, devuelve una lista vacía
        return new List<Vector2>();
    }

    /// <summary>
    ///     Comprueba si una ruta sigue siendo válida (porque el mapa puede variar)
    /// </summary>
    private bool IsRouteStillValid(List<Vector2> gridPath)
    {
        // Recorre los puntos comprobando que se pueda seguir pasando sobre ella
        foreach (Vector2 point in gridPath)
            if (!MapManager.GridMap.IsWalkable(point))
                return false;
        // Si ha llegado hasta aquí es porque no se puede utilizar la ruta
        return true;
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
