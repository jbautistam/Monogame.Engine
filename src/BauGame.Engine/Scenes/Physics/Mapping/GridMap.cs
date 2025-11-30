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
    ///     Asigna el valor de celda de una posición de mundo
    /// </summary>
    public void SetTileFromWorld(float worldX, float worldY, TileType type)
    {
        Point point = ToGrid(worldX, worldY);

            SetTile(point.X, point.Y, type);
    }

    /// <summary>
    ///     Pasa una coordenada de mundo a una coordenada de grid
    /// </summary>
    public Point ToGrid(Vector2 worldPosition) => ToGrid(worldPosition.X, worldPosition.Y);

    /// <summary>
    ///     Pasa una coordenada de mundo a una coordenada de grid
    /// </summary>
    public Point ToGrid(float worldPositionX, float worldPositionY)
    {
        int x = (int) (worldPositionX / TileWidth);
        int y = (int) (worldPositionY / TileHeight);

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
            foreach (Point point in points)
                worldPath.Add(ToWorld(point));
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
    ///     Comprueba si hay una línea de visión abierta entre dos puntos
    /// </summary>
    public bool HasLineOfSight(Point from, Point to)
    {
        int x0 = from.X, y0 = from.Y;
        int x1 = to.X, y1 = to.Y;
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
                if (!IsWalkable(x0, y0))
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
    ///     Obtiene el coste de movimiento a una posición
    /// </summary>
	public int MoveCost(Point position) => (int) GetTile(position.X, position.Y);

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