using Bau.Libraries.BauGame.Engine.Entities.Common;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Definitions;

/// <summary>
///     Cámara híbrida
/// </summary>
public class HybridCamera : AbstractCameraBase
{
    public HybridCamera(CameraDirector director, string name, Vector2 origin, CameraViewport viewportConfig, int zIndex)
        : base(director, name, CameraType.Hybrid, origin, viewportConfig, zIndex)
    {
    }

    /// <summary>
    ///     Calcula la matriz de la vista
    /// </summary>
    protected override Matrix CalculateViewMatrix()
    {
        return Matrix.CreateTranslation(-State.Transform.Position.X, -State.Transform.Position.Y, 0f);
    }

    /// <summary>
    ///     Calcula la matriz de proyección
    /// </summary>
    public override Matrix GetProjectionMatrix()
    {
        (float halfWidth, float halfHeight) = ComputeCenter();
            
            // Obtiene la matriz
            return Matrix.CreateOrthographicOffCenter(-halfWidth, halfWidth, halfHeight, -halfHeight, 0f, 1f);
    }

    /// <summary>
    ///     Comprueba si un rectángulo cae dentro de la cámara
    /// </summary>
    public override bool IsInView(RectangleF bounds)
    {
        (float halfWidth, float halfHeight) = ComputeCenter();
        RectangleF visible = new(State.Transform.Position.X - halfWidth, State.Transform.Position.Y - halfHeight,
                                 halfWidth * 2f, halfHeight * 2f);
            
            // Devuelve el valor que indica si está en la vista
            return !bounds.IsEmpty && visible.Intersects(bounds);
    }

    /// <summary>
    ///     Comprueba si un punto cae dentro de la cámara
    /// </summary>
    public override bool IsInView(Vector2 point)
    {
        (float halfWidth, float halfHeight) = ComputeCenter();
            
            // Devuelve el valor que indica si está en la vista
            return point.X >= State.Transform.Position.X - halfWidth &&
                   point.X <= State.Transform.Position.X + halfWidth &&
                   point.Y >= State.Transform.Position.Y - halfHeight &&
                   point.Y <= State.Transform.Position.Y + halfHeight;
    }

    /// <summary>
    ///     Calcula el centro de la cámara
    /// </summary>
    private (float halfWidth, float halfHeight) ComputeCenter() => (CameraViewport.Width / (2f * PixelsPerUnit), CameraViewport.Height / (2f * PixelsPerUnit));

    /// <summary>
    ///     Transforma las coordenadas de mundo a pantalla
    /// </summary>
    public override Vector2 WorldToScreen(Vector2 worldPos)
    {
        Vector2 offset = worldPos - State.Transform.Position;

            // Devuelve las coordenadas
            return new Vector2(0.5f * CameraViewport.Width + offset.X * PixelsPerUnit,
                               0.5f * CameraViewport.Height + offset.Y * PixelsPerUnit);
    }

    /// <summary>
    ///     Transforma las coordenadas de pantalla a mundo
    /// </summary>
    public override Vector2 ScreenToWorld(Vector2 screenPos)
    {
        float x = (screenPos.X - 0.5f * CameraViewport.Width) / PixelsPerUnit;
        float y = (screenPos.Y - 0.5f * CameraViewport.Height) / PixelsPerUnit;

            // Devuelve las coordenadas
            return State.Transform.Position + new Vector2(x, y);
    }

    /// <summary>
    ///     Relación de pixels por unidades
    /// </summary>
    public float PixelsPerUnit { get; set; } = 100f;
}
