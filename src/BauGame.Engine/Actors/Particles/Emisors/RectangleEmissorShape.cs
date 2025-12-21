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
    public override Vector2 GetEmissionPosition(Vector2 systemPosition)
    {
        if (EdgeOnly)
            switch (Tools.Randomizer.Random.Next(4))
            {
                case 0: // Borde superior
                    return systemPosition + new Vector2((float) (Tools.Randomizer.Random.NextDouble() * Size.X) - 0.5f * Size.X, -0.5f * Size.Y);
                case 1: // Borde derecho
                    return systemPosition + new Vector2(0.5f * Size.X, (float) (Tools.Randomizer.Random.NextDouble() * Size.Y) - 0.5f * Size.Y);
                case 2: // Borde inferior
                    return systemPosition + new Vector2((float)(Tools.Randomizer.Random.NextDouble() * Size.X) - 0.5f * Size.X, 0.5f * Size.Y);
                default: // Borde izquierdo
                    return systemPosition + new Vector2(-0.5f * Size.X, (float) (Tools.Randomizer.Random.NextDouble() * Size.Y) - 0.5f * Size.Y);
            }
        else // Emite en todo el rectángulo
            return systemPosition + new Vector2((float) (Tools.Randomizer.Random.NextDouble() * Size.X) - 0.5f * Size.X,
                                                (float) (Tools.Randomizer.Random.NextDouble() * Size.Y) - 0.5f * Size.Y);
    }

    /// <summary>
    ///     Tamaño del rectángulo de emisor
    /// </summary>
    public Vector2 Size { get; set; } = new(100, 100);

    /// <summary>
    ///     Indica si sólo se debe emitir sobre los bordes
    /// </summary>
    public bool EdgeOnly = false;
}
