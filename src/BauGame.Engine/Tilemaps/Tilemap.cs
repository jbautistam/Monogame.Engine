using Microsoft.Xna.Framework;
using System;

namespace Bau.Monogame.Engine.Domain.Tilemaps;

public class Tilemap
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TileSize { get; private set; }
    public Tileset Tileset { get; private set; }
    public TilemapLayerType Layer { get; set; } = TilemapLayerType.Midground;

    private int[,] _tileIndices;
    private TileType[,] _tileTypes;
    private AnimatedTile[,] _animatedTiles;

    public Tilemap(int width, int height, int tileSize, Tileset tileset, TilemapLayerType layer = TilemapLayerType.Midground)
    {
        Width = width;
        Height = height;
        TileSize = tileSize;
        Tileset = tileset;
        Layer = layer;

        _tileIndices = new int[width, height];
        _tileTypes = new TileType[width, height];
    }

    public void SetTile(int x, int y, int tileIndex, TileType type = TileType.Empty)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return;
        _tileIndices[x, y] = tileIndex;
        _tileTypes[x, y] = type;
    }

    public int GetTileIndex(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return -1;
        return _tileIndices[x, y];
    }

    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return TileType.Empty;
        return _tileTypes[x, y];
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x * TileSize, y * TileSize);
    }

    public Point GetTileAtWorld(Vector2 worldPos)
    {
        int x = (int)Math.Floor(worldPos.X / TileSize);
        int y = (int)Math.Floor(worldPos.Y / TileSize);
        return new Point(x, y);
    }

    public bool IsSolidAt(int x, int y)
    {
        var type = GetTileType(x, y);
        return type == TileType.Wall || type == TileType.Ground || type == TileType.Hazard;
    }

    public bool IsSolidAt(Vector2 worldPos)
    {
        var tile = GetTileAtWorld(worldPos);
        return IsSolidAt(tile.X, tile.Y);
    }

    public Rectangle GetTileBounds(int x, int y)
    {
        return new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
    }

    // Animated Tiles
    public void SetAnimatedTile(int x, int y, AnimatedTile animatedTile)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return;
        if (_animatedTiles == null)
            _animatedTiles = new AnimatedTile[Width, Height];

        _animatedTiles[x, y] = animatedTile;
    }

    public AnimatedTile GetAnimatedTile(int x, int y)
    {
        if (_animatedTiles == null || x < 0 || x >= Width || y < 0 || y >= Height) return null;
        return _animatedTiles[x, y];
    }

    public bool HasAnimationAt(int x, int y)
    {
        return GetAnimatedTile(x, y) != null;
    }
}
