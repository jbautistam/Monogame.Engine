using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

public class RectangleEmissionShape : AbstractEmissionShape
{
    public Vector2 Size = new Vector2(100, 100);
    public bool EdgeOnly = false;

    public override Vector2 GetEmissionPosition()
    {
        if (EdgeOnly)
        {
            // Emitir solo en los bordes
            int side = Tools.Randomizer.Random.Next(4);
            switch (side)
            {
                case 0: // Top
                    return Position + new Vector2((float)(Tools.Randomizer.Random.NextDouble() * Size.X) - Size.X/2, -Size.Y/2);
                case 1: // Right
                    return Position + new Vector2(Size.X/2, (float)(Tools.Randomizer.Random.NextDouble() * Size.Y) - Size.Y/2);
                case 2: // Bottom
                    return Position + new Vector2((float)(Tools.Randomizer.Random.NextDouble() * Size.X) - Size.X/2, Size.Y/2);
                case 3: // Left
                    return Position + new Vector2(-Size.X/2, (float)(Tools.Randomizer.Random.NextDouble() * Size.Y) - Size.Y/2);
                default:
                    return Position;
            }
        }
        else
        {
            // Emitir en todo el rectángulo
            return Position + new Vector2(
                (float)(Tools.Randomizer.Random.NextDouble() * Size.X) - Size.X/2,
                (float)(Tools.Randomizer.Random.NextDouble() * Size.Y) - Size.Y/2
            );
        }
    }
}
