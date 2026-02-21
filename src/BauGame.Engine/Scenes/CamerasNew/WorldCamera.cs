using Bau.Libraries.BauGame.Engine.Entities.Common;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew;

/// <summary>
///     Cámara de mundo
/// </summary>
public class WorldCamera : AbstractCameraBase
{
    public WorldCamera(CameraDirector director, string name, Vector2 origin, CameraViewport viewportConfig)
        : base(director, name, CameraType.World, origin, viewportConfig)
    {
    }

    /// <summary>
    ///     Calcula la matriz de la vista
    /// </summary>
    protected override Matrix CalculateViewMatrix() => State.GetViewMatrix();

    /// <summary>
    ///     Obtiene la matriz de proyección
    /// </summary>
    public override Matrix GetProjectionMatrix()
    {
        float targetAspect = CameraViewport.Width / (float) CameraViewport.Height;
        float viewWidth = State.Transform.Origin.X * 2f;
        float viewHeight = State.Transform.Origin.Y * 2f;
        float virtualAspect = viewWidth / viewHeight;
            
            // Cambia el ancho y alto de la vista si se debe mantener el ratio de aspecto            
            if (CameraViewport.MaintainAspectRatio)
            {
                if (targetAspect > virtualAspect)
                    viewWidth = viewHeight * targetAspect;
                else
                    viewHeight = viewWidth / targetAspect;
            }
            // Crea la matriz de proyección
            return Matrix.CreateOrthographicOffCenter(0, viewWidth, viewHeight, 0, 0f, 1f);
    }

    /// <summary>
    ///     Comprueba si un rectángulo está en los límites
    /// </summary>
    public override bool IsInView(RectangleF bounds)
    {
        RectangleF cullingBounds = GetCullingBounds();
            
            // Comprueba si el rectángulo está en los límites
            return !cullingBounds.IsEmpty && cullingBounds.Intersects(bounds);
    }

    /// <summary>
    ///     Comprueba si un punto está en la vista
    /// </summary>
    public override bool IsInView(Vector2 point)
    {
        RectangleF cullingBounds = GetCullingBounds();
            
            // Comprueba si el punto está en el rectángulo
            return !cullingBounds.IsEmpty && cullingBounds.Contains(point);
    }
    
    /// <summary>
    ///     Obtiene el rectángulo en el que intersecciona la vista con el mundo
    /// </summary>
    private RectangleF GetCullingBounds()
    {
        return RectangleF.Intersect(State.GetVisibleBounds(CameraViewport.ToViewport(Director.Scene.GetViewPort())), 
                                    Director.Scene.WorldDefinition.WorldBounds);
    }

    /// <summary>
    ///     Transforma las coordenadas del mundo en coordenadas de pantalla
    /// </summary>
    public override Vector2 WorldToScreen(Vector2 worldPos)
    {
        Vector3 transformed = Vector3.Transform(new Vector3(worldPos, 0f), GetViewMatrix() * GetProjectionMatrix());

            // Obtiene las coordenadas
            return new Vector2(transformed.X, transformed.Y);
    }

    /// <summary>
    ///     Transforma las coordenadas de pantalla en coordenadas de mundo
    /// </summary>
    public override Vector2 ScreenToWorld(Vector2 screenPos)
    {
        Matrix inverse = Matrix.Invert(GetViewMatrix() * GetProjectionMatrix());
        Vector3 transformed = Vector3.Transform(new Vector3(screenPos, 0f), inverse);

            // Obtiene las coordenadas
            return new Vector2(transformed.X, transformed.Y);
    }
}
