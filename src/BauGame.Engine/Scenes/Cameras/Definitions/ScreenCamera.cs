using Bau.BauEngine.Entities.Common;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Scenes.Cameras.Definitions;

/// <summary>
///     Cámara de interface de usuario o relativa con coordenadas 0 a 1
/// </summary>
public class ScreenCamera : AbstractCameraBase
{
    public ScreenCamera(CameraDirector director, string name, Vector2 origin, CameraViewport viewportConfig, int zIndex)
        : base(director, name, CameraType.Screen, origin, viewportConfig, zIndex)
    {
    }

    /// <summary>
    ///     Calcula la matriz de vista
    /// </summary>
    protected override Matrix CalculateViewMatrix() => Matrix.Identity;

    /// <summary>
    ///     Obtiene la matriz de proyección
    /// </summary>
    public override Matrix GetProjectionMatrix() => Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0f, 1f);

    /// <summary>
    ///     Comprueba si un rectángulo está en el foco de la cámara
    /// </summary>
    public override bool IsInView(RectangleF bounds) => bounds.Left >= 0 && bounds.Right <= 1 && bounds.Top >= 0 && bounds.Bottom <= 1;

    /// <summary>
    ///     Comprueba si un punto está en el foco de la cámara
    /// </summary>
    public override bool IsInView(Vector2 point) => point.X >= 0 && point.X <= 1 && point.Y >= 0 && point.Y <= 1;

    /// <summary>
    ///     Transforma las coordenadas de punto a pantalla
    /// </summary>
    public override Vector2 WorldToScreen(Vector2 worldPos) => worldPos;

    /// <summary>
    ///     Transforma las coordenadas de pantalla a punto
    /// </summary>
    public override Vector2 ScreenToWorld(Vector2 screenPos) => new Vector2(screenPos.X / CameraViewport.Width, screenPos.Y / CameraViewport.Height);
}
