using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Behaviors;

/// <summary>
///     Comportamiento de cámara para seguir un objetivo
/// </summary>
public class FollowBehavior(Definitions.AbstractCameraBase camera) : AbstractCameraBehavior(camera, 0)
{
    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        Vector2 targetPos = Target + Offset;
        Vector2 currentPos = Camera.State.Transform.Position;

            // Calcula los datos de la zona muerta            
            if (UseDeadZone)
            {
                Vector2 diff = targetPos - currentPos;

                    // Si está llegando a la coordenada X de la zona muerta                
                    if (Math.Abs(diff.X) < DeadZone.X * 0.5f)
                        targetPos.X = currentPos.X;
                    else
                        targetPos.X -= Math.Sign(diff.X) * DeadZone.X * 0.5f;
                    // Si está llegando a la coordenada Y de la zona muerta
                    if (Math.Abs(diff.Y) < DeadZone.Y * 0.5f)
                        targetPos.Y = currentPos.Y;
                    else
                        targetPos.Y -= Math.Sign(diff.Y) * DeadZone.Y * 0.5f;
            }
            // Cambia la posición de la cámara
            Camera.SetPosition(currentPos.Lerp(targetPos, SmoothSpeed * gameContext.DeltaTime));
    }

    /// <summary>
    ///     Objetivo a seguir
    /// </summary>
    public Vector2 Target { get; set; }

    /// <summary>
    ///     Desplazamiento sobre el objetivo
    /// </summary>
    public Vector2 Offset { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Velocidad de suavizado
    /// </summary>
    public float SmoothSpeed { get; set; } = 5f;

    /// <summary>
    ///     Zona muerta de la pantalla
    /// </summary>
    public Vector2 DeadZone { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Indica si se debe utilizar la zona muerta
    /// </summary>
    public bool UseDeadZone { get; set; } = false;
}
