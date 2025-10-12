using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Tilemaps;

public class Tileset
{
    public Texture2D Texture { get; private set; }
    public int TileWidth { get; private set; }
    public int TileHeight { get; private set; }
    public int Columns { get; private set; }
    public int Rows { get; private set; }

    public Tileset(Texture2D texture, int tileWidth, int tileHeight)
    {
        Texture = texture;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Columns = texture.Width / tileWidth;
        Rows = texture.Height / tileHeight;
    }

    public Rectangle? GetSourceRect(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= Columns * Rows)
            return null;

        int x = (tileIndex % Columns) * TileWidth;
        int y = (tileIndex / Columns) * TileHeight;

        return new Rectangle(x, y, TileWidth, TileHeight);
    }

    public int GetTileIndex(int column, int row)
    {
        if (column < 0 || column >= Columns || row < 0 || row >= Rows)
            return -1;
        return row * Columns + column;
    }
}
