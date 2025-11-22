using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.TilesMap;

public class Tilemap
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TileWidth => _tilesetDef.TileWidth;
    public int TileHeight => _tilesetDef.TileHeight;

    private Texture2D _texture;
    private TilesetDefinition _tilesetDef;
    private MapDefinition _mapDef;
    private Dictionary<string, int[]> _layers = new();
    private Dictionary<int, TilesetTileDefinition> _tileDefsById = new();
    private Dictionary<Point, AnimatedTile> _animatedTiles = new();

    // Cache de colisiones (solo tiles sólidos)
    private bool[,] _collisionGrid;

    //public Tilemap(GraphicsDevice graphicsDevice, string mapPath, string tilesetPath, string texturePath)
    //{
    //    // Cargar textura
    //    using (var stream = TitleContainer.OpenStream($"Content/{texturePath}"))
    //    {
    //        _texture = Texture2D.FromStream(graphicsDevice, stream);
    //    }

    //    // Cargar definiciones
    //    //var tilesetJson = File.ReadAllText($"Content/{tilesetPath}");
    //    //_tilesetDef = JsonConvert.DeserializeObject<TilesetDefinition>(tilesetJson);

    //    //var mapJson = File.ReadAllText($"Content/{mapPath}");
    //    //_mapDef = JsonConvert.DeserializeObject<MapDefinition>(mapJson);

    //    Width = _mapDef.Width;
    //    Height = _mapDef.Height;

    //    // Indexar definiciones por ID
    //    foreach (var def in _tilesetDef.Tiles)
    //    {
    //        _tileDefsById[def.Id] = def;
    //        if (def.IsAnimated)
    //        {
    //            // Inicializar animaciones (una por posición animada)
    //            // Se llenará en Update si es necesario, o al cargar el mapa
    //        }
    //    }

    //    // Cargar capas
    //    foreach (var layer in _mapDef.Layers)
    //    {
    //        if (layer.Data.Length != Width * Height)
    //            throw new Exception($"Capa {layer.Name} no coincide con dimensiones del mapa.");
    //        _layers[layer.Name] = layer.Data;
    //    }

    //    // Construir grid de colisiones
    //    BuildCollisionGrid();
    //}

    private void BuildCollisionGrid()
    {
        _collisionGrid = new bool[Width, Height];
        // Usar la primera capa como base de colisión (o combinar capas)
        var baseLayer = _layers["ground"]; // o cualquier capa que defina sólidos
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int tileId = baseLayer[y * Width + x];
                _collisionGrid[x, y] = tileId > 0 && _tileDefsById.TryGetValue(tileId, out var def) && def.Type == TileType.Solid;
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        // Actualizar animaciones (si decides manejarlas por posición)
        // En este ejemplo, no almacenamos animaciones por tile, sino que las calculamos al dibujar
        // Pero si necesitas estado por tile animado, usa _animatedTiles
    }

    public int GetTileIdAtWorldPosition(Vector2 worldPos, string layerName = "ground")
    {
        int tileX = (int)(worldPos.X / TileWidth);
        int tileY = (int)(worldPos.Y / TileHeight);

        if (tileX < 0 || tileX >= Width || tileY < 0 || tileY >= Height)
            return -1; // fuera del mapa

        if (!_layers.TryGetValue(layerName, out int[] layerData))
            return -1;

        return layerData[tileY * Width + tileX];
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, int viewportWidth, int viewportHeight)
    {
        int tileW = TileWidth;
        int tileH = TileHeight;

        // Límites del viewport en coordenadas de mundo
        float worldLeft = cameraPosition.X;
        float worldRight = cameraPosition.X + viewportWidth;
        float worldTop = cameraPosition.Y;
        float worldBottom = cameraPosition.Y + viewportHeight;

        // Rango de tiles que intersectan el viewport (incluyendo parciales)
        int startX = Math.Max(0, (int)Math.Floor(worldLeft / tileW));
        int startY = Math.Max(0, (int)Math.Floor(worldTop / tileH));
        int endX = Math.Min(Width, (int)Math.Ceiling(worldRight / tileW));
        int endY = Math.Min(Height, (int)Math.Ceiling(worldBottom / tileH));

        foreach (var layer in _mapDef.Layers)
        {
            if (!_layers.TryGetValue(layer.Name, out int[] data))
                continue;

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    int index = y * Width + x;
                    int tileId = data[index];
                    if (tileId <= 0) continue;

                    if (!_tileDefsById.TryGetValue(tileId, out var def))
                        continue;

                    int drawTileId = tileId;

                    if (def.IsAnimated && def.AnimationFrames?.Count > 0)
                    {
                        float timeOffset = (x * 7 + y * 13) * 0.1f;
                        int frameIndex = (int)((Game1.TotalGameTime + timeOffset) / def.FrameDuration) % def.AnimationFrames.Count;
                        drawTileId = def.AnimationFrames[frameIndex];
                    }

                    int tilesPerRow = _texture.Width / tileW;
                    int srcX = (drawTileId % tilesPerRow) * tileW;
                    int srcY = (drawTileId / tilesPerRow) * tileH;
                    var sourceRect = new Rectangle(srcX, srcY, tileW, tileH);

                    // Posición EXACTA en pantalla (con fracciones)
                    Vector2 screenPos = new Vector2(
                        x * tileW - cameraPosition.X,
                        y * tileH - cameraPosition.Y
                    );

                    spriteBatch.Draw(_texture, screenPos, sourceRect, Color.White);
                }
            }
        }
    }

    public (int startX, int endX, int startY, int endY) GetVisibleTileRange(
        Vector2 cameraWorldPosition,
        float zoom,
        int viewportWidth,
        int viewportHeight,
        int mapWidthInTiles,
        int mapHeightInTiles,
        int tileWidth,
        int tileHeight)
    {
        // 1. Calcular tamaño del mundo visible
        float worldVisibleWidth = viewportWidth / zoom;
        float worldVisibleHeight = viewportHeight / zoom;

        // 2. Límites del mundo visible
        float worldLeft = cameraWorldPosition.X - worldVisibleWidth / 2f;
        float worldRight = cameraWorldPosition.X + worldVisibleWidth / 2f;
        float worldTop = cameraWorldPosition.Y - worldVisibleHeight / 2f;
        float worldBottom = cameraWorldPosition.Y + worldVisibleHeight / 2f;

        // 3. Convertir a índices de tile (incluyendo parciales)
        int startX = Math.Max(0, (int)Math.Floor(worldLeft / tileWidth));
        int endX = Math.Min(mapWidthInTiles, (int)Math.Ceiling(worldRight / tileWidth));
        int startY = Math.Max(0, (int)Math.Floor(worldTop / tileHeight));
        int endY = Math.Min(mapHeightInTiles, (int)Math.Ceiling(worldBottom / tileHeight));

        return (startX, endX, startY, endY);
    }

    // === Sistema de colisiones ===
    public bool IsSolid(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return true; // fuera del mapa = sólido
        return _collisionGrid[x, y];
    }

    public bool IsSolidWorld(Vector2 worldPosition)
    {
        int tileX = (int)(worldPosition.X / TileWidth);
        int tileY = (int)(worldPosition.Y / TileHeight);
        return IsSolid(tileX, tileY);
    }

    public Rectangle GetTileBounds(int x, int y)
    {
        return new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
    }

    public Rectangle GetTileBoundsWorld(Vector2 worldPosition)
    {
        int tileX = (int)(worldPosition.X / TileWidth);
        int tileY = (int)(worldPosition.Y / TileHeight);
        return GetTileBounds(tileX, tileY);
    }

    // Para AABB contra el tilemap
    public bool CheckCollision(Rectangle bounds)
    {
        int startX = Math.Max(0, bounds.Left / TileWidth);
        int endX = Math.Min(Width - 1, bounds.Right / TileWidth);
        int startY = Math.Max(0, bounds.Top / TileHeight);
        int endY = Math.Min(Height - 1, bounds.Bottom / TileHeight);

        for (int y = startY; y <= endY; y++)
        {
            for (int x = startX; x <= endX; x++)
            {
                if (IsSolid(x, y))
                    return true;
            }
        }
        return false;
    }
}
