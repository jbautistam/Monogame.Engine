using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics;

/// <summary>
///     Datos de colisión cinemática
/// </summary>
public class KinematicCollisionModel
{
    /// <summary>
    ///     Posición de la colisión
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    ///     Normal de la colisión
    /// </summary>
    public Vector2 Normal { get; set; }

    /// <summary>
    ///     Resto del movimiento
    /// </summary>
    public Vector2 Remainder { get; set; }

    /// <summary>
    ///     Movimiento que se puede realizar sin colisiones
    /// </summary>
    public Vector2 SafeMotion { get; set; }

    /// <summary>
    ///     Valor de penetración
    /// </summary>
    public float Penetration { get; set; }

    /// <summary>
    ///     Elemento contra el que ha colisionado
    /// </summary>
    public object? Collider { get; set; }
}