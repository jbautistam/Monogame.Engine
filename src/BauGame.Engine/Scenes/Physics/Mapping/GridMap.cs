using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping;

/// <summary>
///     Grid de un mapa
/// </summary>
public class GridMap
{
    /// <summary>
    ///     Tipo de celda del mapa
    /// </summary>
    public enum TileType
    {
        Normal,
        Blocked,
        Road,
        Mud,
        Water
    }
    // Variables privadas
    private TileType[ , ] _tiles;

    public GridMap(Models.WorldDefinitionModel worldDefinition)
    {
        // Asigna las propiedades
        TileWidth = worldDefinition.CellWidth;
        TileHeight = worldDefinition.CellHeight;
        Width = worldDefinition.WorldBounds.Width / worldDefinition.CellWidth;
        Height = worldDefinition.WorldBounds.Height / worldDefinition.CellHeight;
        // Crea el array de celdas
        _tiles = new TileType[Width, Height];
    }

    /// <summary>
    ///     Indica si un punto está en el grid
    /// </summary>
    public bool IsAtGrid(int x, int y) => x > 0 && x < Width && y > 0 && y < Width;

    /// <summary>
    ///     Obtiene el tipo de celda de una posición
    /// </summary>
    public TileType GetTile(int x, int y)
    {
        if (!IsAtGrid(x, y))
            return TileType.Blocked;
        else
            return _tiles[x, y];
    }

    /// <summary>
    ///     Asigna el valor de celda de una posición
    /// </summary>
    public void SetTile(int x, int y, TileType type)
    {
        if (IsAtGrid(x, y))
            _tiles[x, y] = type;
    }

    /// <summary>
    ///     Pasa una coordenada de mundo a una coordenada de grid
    /// </summary>
    public Point ToGrid(Vector2 worldPosition)
    {
        int x = (int) (worldPosition.X / TileWidth);
        int y = (int) (worldPosition.Y / TileHeight);

            // Normaliza los valores
            x = Math.Clamp(x, 0, Width - 1);
            y = Math.Clamp(y, 0, Height - 1);
            // Devuelve el punto
            return new Point(x, y);
    }

    /// <summary>
    ///     Pasa una coordenada de grid a una coordenada de mundo
    /// </summary>
    public Vector2 ToWorld(Point point) => ToWorld(point.X, point.Y);

    /// <summary>
    ///     Pasa una coordenada de grid a una coordenada de mundo
    /// </summary>
    public Vector2 ToWorld(int x, int y)
    {
        int worldX = Math.Clamp(x, 0, Width - 1);
        int worldY = Math.Clamp(y, 0, Height - 1);

            // Devuelve las coordenadas del mundo
            return new Vector2(worldX * TileWidth + TileWidth * 0.5f, worldY * TileHeight + TileHeight * 0.5f);
    }

    /// <summary>
    ///     Transforma una serie de coordenadas
    /// </summary>
    public List<Vector2> ToWorld(List<Point> points)
    {
        List<Vector2> worldPath = [];
            
            // Añade los puntos convertidos
            foreach (var p in points)
                worldPath.Add(ToWorld(p));
            // Devuelve los puntos
            return worldPath;
    }

    /// <summary>
    ///     Comprueba si se puede caminar sobre una celda indicando coordenadas de mundo
    /// </summary>
    public bool IsWalkable(Vector2 worldPosition) => IsWalkable(ToGrid(worldPosition));

    /// <summary>
    ///     Comprueba si se puede caminar sobre una celda indicando coordenadas de grid
    /// </summary>
    public bool IsWalkable(Point point) => IsWalkable(point.X, point.Y);

    /// <summary>
    ///     Comprueba si se puede caminar sobre una celda indicando coordenadas de grid
    /// </summary>
    public bool IsWalkable(int x, int y) => GetTile(x, y) != TileType.Blocked;

    /// <summary>
    ///     Ancho de las celdas
    /// </summary>
    public int TileWidth;

    /// <summary>
    ///     Alto de las celdas del grid
    /// </summary>
    public int TileHeight;

    /// <summary>
    ///     Ancho del grid
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Alto del grid
    /// </summary>
    public int Height { get; }
}