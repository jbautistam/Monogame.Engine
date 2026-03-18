using Microsoft.Xna.Framework;
using Bau.BauEngine.Tools.Extensors;
using Bau.BauEngine.Entities.Common;

namespace Bau.BauEngine.Scenes.Cameras.Behaviors;

/// <summary>
///     Comportamiento de cámara para seguir varios objetivos
/// </summary>
public class MultiTargetFollowBehavior(Definitions.AbstractCameraBase camera) : AbstractCameraBehavior(camera, 0)
{
    // Variables privadas
    private float _zoom;
    private Vector2 _position;

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        if (Targets.Count > 0)
        {
            RectangleF bounds = CalculateTargetsBounds();

                // Inicializa los valores
                if (!IsInitialized)
                {
                    _zoom = Camera.State.Zoom;
                    _position = Camera.State.Transform.Position;
                }
                // Calcula el centro y el zoom actual
                _position = _position.Lerp(bounds.Center + Offset, SmoothSpeed * gameContext.DeltaTime);
                _zoom = MathHelper.Lerp(_zoom, MathHelper.Clamp(CalculateRequiredZoom(bounds), MinZoom, MaxZoom), ZoomSmoothSpeed * gameContext.DeltaTime);
                // Cambia la posición y el zoom de la cámara            
                Camera.SetPosition(_position);
                Camera.SetZoom(_zoom);
        }
    }

    /// <summary>
    ///     Calcula los límites del rectángulo donde se encuentran los objetivos
    /// </summary>
    private RectangleF CalculateTargetsBounds()
    {
        if (Targets.Count == 0) 
            return RectangleF.Empty;
        else
        {
            float minX = Targets[0].X;
            float maxX = Targets[0].X;
            float minY = Targets[0].Y;
            float maxY = Targets[0].Y;
            
                // Calcula las coordenadas mínimas y máximas del rectángulo donde se encuentran todos los objetivos
                for (int index = 1; index < Targets.Count; index++)
                {
                    minX = Math.Min(Targets[index].X, minX);
                    maxX = Math.Max(Targets[index].X, maxX);
                    minY = Math.Min(Targets[index].Y, minY);
                    maxY = Math.Min(Targets[index].Y, maxY);
                }
                // Devuelve el rectángulo calculado
                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }
    }

    /// <summary>
    ///     Calcula el zoom necesario para que todos los objetivos se vean en pantalla
    /// </summary>
    private float CalculateRequiredZoom(RectangleF bounds)
    {
        float contentWidth = bounds.Width + ZoomMargin * 2f;
        float contentHeight = bounds.Height + ZoomMargin * 2f;
            
            // Calcula el zoom
            if (contentWidth <= 0 || contentHeight <= 0) 
                return MaxZoom;
            else
            {
                float zoomX = Camera.CameraViewport.Width / contentWidth;
                float zoomY = Camera.CameraViewport.Height / contentHeight;
            
                    // Devuelve el zoom adecuado
                    return Math.Min(zoomX, zoomY);
            }
    }

    /// <summary>
    ///     Objetivos a los que sigue la cámara
    ///     TODO: Posiblemente sea mejor que aquí hubiera transformaciones y no vectores. Así el cálculo de zoom sería más correcto
    /// </summary>
    public List<Vector2> Targets { get; } = [];

    /// <summary>
    ///     Velocidad de desplazamiento
    /// </summary>
    public float SmoothSpeed { get; set; } = 3f;

    /// <summary>
    ///     Desplazamiento sobre el centro
    /// </summary>
    public Vector2 Offset { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Zoom mínimo
    /// </summary>
    public float MinZoom { get; set; } = 0.5f;

    /// <summary>
    ///     Zoom máximo
    /// </summary>
    public float MaxZoom { get; set; } = 2f;

    /// <summary>
    ///     Margen para el zoom
    /// </summary>
    public float ZoomMargin { get; set; } = 100f;

    /// <summary>
    ///     Velocidad de zoom
    /// </summary>
    public float ZoomSmoothSpeed { get; set; } = 2f;
}
