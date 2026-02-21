using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Optimization;

public class SpatialHash
{
    private readonly float _cellSize;
    private readonly Dictionary<long, List<ICullable>> _cells = new Dictionary<long, List<ICullable>>();

    public SpatialHash(float cellSize)
    {
        _cellSize = cellSize;
    }

    public void Insert(ICullable item)
    {
        var bounds = item.GetBounds();
        var minCell = GetCellCoord(bounds.TopLeft);
        var maxCell = GetCellCoord(bounds.BottomRight);

        for (int x = minCell.X; x <= maxCell.X; x++)
        {
            for (int y = minCell.Y; y <= maxCell.Y; y++)
            {
                long key = GetKey(x, y);
                    
                if (_cells.TryGetValue(key, out var list) == false)
                {
                    list = new List<ICullable>();
                    _cells[key] = list;
                }
                    
                list.Add(item);
            }
        }
    }

    public void Remove(ICullable item)
    {
        var bounds = item.GetBounds();
        var minCell = GetCellCoord(bounds.TopLeft);
        var maxCell = GetCellCoord(bounds.BottomRight);

        for (int x = minCell.X; x <= maxCell.X; x++)
        {
            for (int y = minCell.Y; y <= maxCell.Y; y++)
            {
                long key = GetKey(x, y);
                    
                if (_cells.TryGetValue(key, out var list))
                {
                    list.Remove(item);
                }
            }
        }
    }

    public void Query(RectangleF area, List<ICullable> results)
    {
        results.Clear();
            
        var minCell = GetCellCoord(area.TopLeft);
        var maxCell = GetCellCoord(area.BottomRight);

        var seen = new HashSet<ICullable>();

        for (int x = minCell.X; x <= maxCell.X; x++)
        {
            for (int y = minCell.Y; y <= maxCell.Y; y++)
            {
                long key = GetKey(x, y);
                    
                if (_cells.TryGetValue(key, out var list))
                {
                    foreach (var item in list)
                    {
                        if (seen.Add(item))
                        {
                            if (area.Intersects(item.GetBounds()))
                            {
                                results.Add(item);
                            }
                        }
                    }
                }
            }
        }
    }

    public void Clear()
    {
        _cells.Clear();
    }

    private Point GetCellCoord(Vector2 worldPos)
    {
        return new Point(
            (int)System.MathF.Floor(worldPos.X / _cellSize),
            (int)System.MathF.Floor(worldPos.Y / _cellSize)
        );
    }

    private long GetKey(int x, int y)
    {
        return ((long)x << 32) | (uint)y;
    }
}
