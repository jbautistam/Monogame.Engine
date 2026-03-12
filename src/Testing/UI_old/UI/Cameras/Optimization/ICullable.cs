using GameEngine.Math;

namespace GameEngine.Optimization;

public interface ICullable
{
    RectangleF GetBounds();
}