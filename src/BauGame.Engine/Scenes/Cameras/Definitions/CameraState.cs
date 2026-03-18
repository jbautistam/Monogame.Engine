using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Entities.Common;

namespace Bau.BauEngine.Scenes.Cameras.Definitions;

/// <summary>
///     Datos de estado de la cámara
/// </summary>
public class CameraState
{
    // Variables privadas
    private Matrix _viewMatrix = default!;
    private Matrix _inverseViewMatrix = default!;

    public CameraState(Vector2 position, float rotation, float zoom, Vector2 origin)
    {
        Transform = new TransformModel()
                            {
                                Position = position,
                                Rotation = rotation,
                                Scale = new Vector2(zoom, zoom),
                                Origin = origin
                            };
    }

    /// <summary>
    ///     Obtiene un <see cref="CameraState"/> con los datos predeterminados
    /// </summary>
    public static CameraState Default(Vector2 origin) => new(Vector2.Zero, 0f, 1f, origin);

    /// <summary>
    ///     Obtiene la matriz de la vista
    /// </summary>
    public Matrix GetViewMatrix()
    {
        // Recalcula las matrices si es necesario
        ComputeMatrices();
        // Devuelve la matriz
        return _viewMatrix;
    }

    /// <summary>
    ///     Obtiene la matriz inversa
    /// </summary>
    public Matrix GetInverseViewMatrix()
    {
        // Recalcula las matrices si es necesario
        ComputeMatrices();
        // Devuelve la matriz inversa
        return _inverseViewMatrix;
    }

    /// <summary>
    ///     Calcula las matrices
    /// </summary>
    private void ComputeMatrices()
    {
        if (Transform.IsDirty)
        {
            _viewMatrix = ComputeViewMatrix();
            _inverseViewMatrix = ComputeInverseMatrix();
            Transform.IsDirty = false;
        }
    }

    /// <summary>
    ///     Calcula matriz de vista
    /// </summary>
    private Matrix ComputeViewMatrix()
    {
        Matrix translation = Matrix.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0f);
        Matrix rotation = Matrix.CreateRotationZ(-Transform.Rotation);
        Matrix scale = Matrix.CreateScale(Transform.Scale.X, Transform.Scale.Y, 1f);
        Matrix originOffset = Matrix.CreateTranslation(Transform.Origin.X, Transform.Origin.Y, 0f);

            // Devuelve la matriz
            return translation * rotation * scale * originOffset;
    }

    /// <summary>
    ///     Calcula la matriz inversa
    /// </summary>
    private Matrix ComputeInverseMatrix()
    {
        Matrix scale = Matrix.CreateScale(1f / Transform.Scale.X, 1f / Transform.Scale.Y, 1f);
        Matrix rotation = Matrix.CreateRotationZ(Transform.Rotation);
        Matrix translation = Matrix.CreateTranslation(Transform.Position.X, Transform.Position.Y, 0f);
        Matrix originOffset = Matrix.CreateTranslation(-Transform.Origin.X, -Transform.Origin.Y, 0f);

            // Devuelve la matriz calculada
            return originOffset * scale * rotation * translation;
    }

    /// <summary>
    ///     Transforma coordenadas de mundo a coordenadas de pantalla
    /// </summary>
    public Vector2 WorldToScreen(Vector2 worldPosition, Viewport viewport)
    {
        Vector3 transformed = Vector3.Transform(new Vector3(worldPosition, 0f), 
                                                GetViewMatrix() * GetProjectionMatrix(viewport));

            // Devuelve el vector
            return new Vector2(transformed.X, transformed.Y);
    }

    /// <summary>
    ///     Transforma coordenadas de pantalla a coordenadas de mundo
    /// </summary>
    public Vector2 ScreenToWorld(Vector2 screenPosition, Viewport viewport)
    {
        Matrix inverse = Matrix.Invert(GetViewMatrix() * GetProjectionMatrix(viewport));
        Vector3 transformed = Vector3.Transform(new Vector3(screenPosition, 0f), inverse);
            
            // Devuelve el vector
            return new Vector2(transformed.X, transformed.Y);
    }

    /// <summary>
    ///     Obtiene la matriz de proyección ortográfica
    /// </summary>
    private Matrix GetProjectionMatrix(Viewport viewport)
    {
        return Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0f, 1f);
    }

    /// <summary>
    ///     Obtiene un rectángulo con las coordenadas del mundo
    /// </summary>
    public RectangleF GetVisibleBounds(Viewport viewport)
    {
        return RectangleF.FromPoints(ScreenToWorld(Vector2.Zero, viewport), 
                                     ScreenToWorld(new Vector2(viewport.Width, viewport.Height), viewport));
    }
    /// <summary>
    ///     Cambia el zoom de la cámara por un factor
    /// </summary>
    public void ZoomBy(float factor) 
    {
        Transform.Scale *= factor;
        ClampZoom();
    }

    /// <summary>
    ///     Cambia el zoom de la cámara
    /// </summary>
    public void SetZoom(float zoom)
    {
        Transform.Scale = new Vector2(zoom, zoom);
        ClampZoom();
    }

    /// <summary>
    ///     Limita el zoom de la cámara
    /// </summary>
    private void ClampZoom()
    {
        Transform.Scale = new Vector2(MathHelper.Clamp(Transform.Scale.X, 0.001f, 100f),
                                      MathHelper.Clamp(Transform.Scale.X, 0.001f, 100f));
    }

    /// <summary>
    ///     Posición
    /// </summary>
    public TransformModel Transform { get; }

    /// <summary>
    ///     Zoom de la cámara
    /// </summary>
    public float Zoom => Transform.Scale.X;

    /// <summary>
    ///     Pasa el tipo a cadena
    /// </summary>
    public override string ToString() => $"Camera state: {Transform.ToString()}";
}