using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

/// <summary>
///     Emisor sobre un rectángulo
/// </summary>
public class RectangleEmissorShape : AbstractEmissorShape
{
    /// <summary>
    ///     Obtiene la posición de emisión
    /// </summary>
    public override Vector2 GetEmissionPosition()
    {
        if (EdgeOnly)
            switch (Tools.Randomizer.Random.Next(4))
            {
                case 0: // Borde superior
                    return Position + new Vector2((float) (Tools.Randomizer.Random.NextDouble() * Size.X) - 0.5f * Size.X, -0.5f * Size.Y);
                case 1: // Borde derecho
                    return Position + new Vector2(0.5f * Size.X, (float) (Tools.Randomizer.Random.NextDouble() * Size.Y) - 0.5f * Size.Y);
                case 2: // Borde inferior
                    return Position + new Vector2((float)(Tools.Randomizer.Random.NextDouble() * Size.X) - 0.5f * Size.X, 0.5f * Size.Y);
                default: // Borde izquierdo
                    return Position + new Vector2(-0.5f * Size.X, (float) (Tools.Randomizer.Random.NextDouble() * Size.Y) - 0.5f * Size.Y);
            }
        else // Emite en todo el rectángulo
            return Position + new Vector2((float) (Tools.Randomizer.Random.NextDouble() * Size.X) - 0.5f * Size.X,
                                          (float) (Tools.Randomizer.Random.NextDouble() * Size.Y) - 0.5f * Size.Y);
    }

    /// <summary>
    ///     Tamaño del rectángulo de emsor
    /// </summary>
    public Vector2 Size { get; set; } = new(100, 100);

    /// <summary>
    ///     Indica si sólo se debe emitir sobre los bordes
    /// </summary>
    public bool EdgeOnly = false;
}
