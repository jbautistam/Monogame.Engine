using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

/// <summary>
///     Comportamiento para modificar zoom y posición de la cámara
/// </summary>
public class DollyZoomBehavior(AbstractCameraBase camera, float duration) : AbstractCameraBehavior(camera, duration)
{
    // Variables privadas
    private Vector2 _startPosition;
    private float _initialZoom;

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        float t = MathHelper.SmoothStep(0f, 1f, Progress);

            // Recoge los valores de cámara si no se había inicializado
            if (!IsInitialized)
            {
                _startPosition = Camera.State.Transform.Position;
                _initialZoom = Camera.State.Zoom;
            }
            // Cambia posición y zoom
            Camera.SetPosition(GetNewPosition(t));
            Camera.SetZoom(MathHelper.Lerp(_initialZoom, EndZoom, t));

        // Obtiene la nueva posición
        Vector2 GetNewPosition(float t)
        {
            float currentDistance = MathHelper.Lerp(StartDistance, EndDistance, t);
            Vector2 toTarget = Target - _startPosition;

                // Normaliza el vector
                toTarget.Normalize();
                // Devuelve la nueva posición
                return Target - toTarget * currentDistance;
        }
    }

    /// <summary>
    ///     Modifica los datos de cámara cuando finaliza
    /// </summary>
    protected override void OnComplete()
    {
        Camera.SetZoom(EndZoom);
    }

    /// <summary>
    ///     Posición destino
    /// </summary>
    public Vector2 Target { get; set; }

    /// <summary>
    ///     Distancia inicial
    /// </summary>
    public float StartDistance { get; set; } = 300f;

    /// <summary>
    ///     Distancia final
    /// </summary>
    public float EndDistance { get; set; } = 150f;

    /// <summary>
    ///     Zoom final
    /// </summary>
    public float EndZoom { get; set; } = 2f;
}
