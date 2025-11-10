using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

/// <summary>
///     Emisor sobre un círculo
/// </summary>
public class CircleEmissorShape : AbstractEmissorShape
{
    /// <summary>
    ///     Obtiene la posición de emisión
    /// </summary>
    public override Vector2 GetEmissionPosition(Vector2 systemPosition)
    {
        float angle = (float) (Tools.Randomizer.Random.NextDouble() * Math.PI * 2);
        float radius = EdgeOnly ? Radius : (float) (Tools.Randomizer.Random.NextDouble() * Radius);

            // Devuelve el punto de emisión
            return systemPosition + new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
    }

    /// <summary>
    ///     Radio del círculo
    /// </summary>
    public float Radius = 50f;

    /// <summary>
    ///     Indica si sólo se debe emitir desde el borde
    /// </summary>
    public bool EdgeOnly = false;
}