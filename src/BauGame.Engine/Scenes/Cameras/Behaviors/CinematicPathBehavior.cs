using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Behaviors;

/// <summary>
///     Comportamiento para seguir una ruta con la cámara
/// </summary>
public class CinematicPathBehavior(Definitions.AbstractCameraBase camera) : AbstractCameraBehavior(camera, 0)
{
    // Registro con los datos de un punto de la ruta
    public record PathPoint(Vector2 Point, float WaitTime);
    // Variable privadas
    private int _currentSegment;
    private float _segmentProgress;
    private float _waitTimer;

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        if (ControlPoints.Count >= 2)
        {   
            // Espera a terminar con el punto actual
            if (_waitTimer > 0)
                _waitTimer -= gameContext.DeltaTime;
            else
            {
                float segmentDuration = MathHelper.Clamp(Duration / (ControlPoints.Count - 1), 0.1f, 1);

                    // Calcula el progreso con el segmento actual
                    _segmentProgress += gameContext.DeltaTime / segmentDuration;
                    // Si ya hemos llegado al final del progreso del segmento, pasamos al siguiente e inicializamos el temporizador
                    if (_segmentProgress >= 1f && _currentSegment < ControlPoints.Count)
                    {
                        _currentSegment++;
                        _waitTimer = ControlPoints[_currentSegment].WaitTime;
                    }
                    // Si se ha llegado al último segmento, vuelve a empezar si es necesario, si no, se detiene
                    if (_currentSegment >= ControlPoints.Count - 1)
                    {
                        if (Loop)
                        {
                            _currentSegment = 0;
                            _segmentProgress = 0f;
                        }
                        else
                            IsComplete = true;
                    }
                    // Si no se ha terminado se calcula la nueva posición
                    if (!IsComplete)
                    {
                        Vector2 p0 = ControlPoints[Math.Max(0, _currentSegment - 1)].Point;
                        Vector2 p1 = ControlPoints[_currentSegment].Point;
                        Vector2 p2 = ControlPoints[Math.Min(ControlPoints.Count - 1, _currentSegment + 1)].Point;
                        Vector2 p3 = ControlPoints[Math.Min(ControlPoints.Count - 1, _currentSegment + 2)].Point;
            
                            // Cambia la posición
                            Camera.SetPosition(CatmullRom(p0, p1, p2, p3, _segmentProgress));
                    }
            }
        }
    }

    /// <summary>
    ///     Calcula el suavizado de los vectores
    /// </summary>
    private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
            
            // Devuelve el vector suavizado
            return 0.5f * ((2f * p1) + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 + (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }

    /// <summary>
    ///     Puntos de control
    /// </summary>
    public List<PathPoint> ControlPoints { get; } = [];

    /// <summary>
    ///     Indica si se debe ejecutar en bucle
    /// </summary>
    public bool Loop { get; set; } = false;
}
