using Bau.Libraries.BauGame.Engine.Entities.Common;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

/// <summary>
///     Comportamiento para limitar la cámara a las coordenadas del mundo
/// </summary>
public class BoundsConstraintBehavior(AbstractCameraBase camera) : AbstractCameraBehavior(camera, 0)
{
    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        (float halfVisibleWidth, float halfVisibleHeight) = ComputeVisibleCoords();
        Vector2 targetPos = Camera.State.Transform.Position;
        float minX = Camera.Director.Scene.WorldDefinition.WorldBounds.Left + halfVisibleWidth;
        float maxX = Camera.Director.Scene.WorldDefinition.WorldBounds.Right - halfVisibleWidth;
        float minY = Camera.Director.Scene.WorldDefinition.WorldBounds.Top + halfVisibleHeight;
        float maxY = Camera.Director.Scene.WorldDefinition.WorldBounds.Bottom - halfVisibleHeight;

            // Calcula la nueva posición aplicando las restricciones
            if (SoftConstraint)
            {
                // Limita la posición X
                if (targetPos.X < minX)
                    targetPos.X = minX - (minX - targetPos.X) * Softness;
                else if (targetPos.X > maxX)
                    targetPos.X = maxX + (targetPos.X - maxX) * Softness;
                // Limita la posición Y                
                if (targetPos.Y < minY)
                    targetPos.Y = minY - (minY - targetPos.Y) * Softness;
                else if (targetPos.Y > maxY)
                    targetPos.Y = maxY + (targetPos.Y - maxY) * Softness;
            }
            else
            {
                targetPos.X = MathHelper.Clamp(targetPos.X, minX, maxX);
                targetPos.Y = MathHelper.Clamp(targetPos.Y, minY, maxY);
            }
            // Cambia la posición de la cámara
            Camera.SetPosition(targetPos);

        // Calcula las coordenadas visibles
        (float halfVisibleWidth, float halfVisibleHeight) ComputeVisibleCoords()
        {
            RectangleF visible = Camera.State.GetVisibleBounds(Camera.CameraViewport.ToViewport(Camera.Director.Scene.GetViewPort()));

                return (visible.Width * 0.5f, visible.Height * 0.5f);
        }
    }

    /// <summary>
    ///     Indica si tiene que ser una restricción suave
    /// </summary>
    public bool SoftConstraint { get; set; } = false;

    /// <summary>
    ///     Factor de suavidad para el comportamiento
    /// </summary>
    public float Softness { get; set; } = 0.5f;
}
