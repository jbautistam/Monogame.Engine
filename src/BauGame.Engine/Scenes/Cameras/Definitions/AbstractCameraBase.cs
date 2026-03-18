using Microsoft.Xna.Framework;
using Bau.BauEngine.Entities.Common;
using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Scenes.Cameras.Definitions;

/// <summary>
///     Clase base para las cámaras
/// </summary>
public abstract class AbstractCameraBase
{
    /// <summary>
    ///     Tipos de cámara
    /// </summary>
    public enum CameraType
    {
        /// <summary>Cámara de mundo</summary>
        World,
        /// <summary>Cámara de interface de usuario (coordenadas relativas 0 a 1)</summary>
        Screen,
        /// <summary>Cámara híbrida</summary>
        Hybrid
    }
    // Variables privadas
    private Matrix _viewMatrix;
    private bool _matrixDirty = true;

    protected AbstractCameraBase(CameraDirector director, string name, CameraType type, Vector2 origin, CameraViewport cameraViewport, int zIndex)
    {
        Name = name;
        Type = type;
        Director = director;
        State = CameraState.Default(origin);
        CameraViewport = cameraViewport;
        ZIndex = zIndex;
    }

    /// <summary>
    ///     Obtiene la matriz de la vista
    /// </summary>
    public Matrix GetViewMatrix()
    {
        // Recalcula la matriz si es necesario
        if (_matrixDirty || State.Transform.IsDirty)
        {
            _viewMatrix = CalculateViewMatrix();
            _matrixDirty = false;
        }
        // Devuelve la matriz
        return _viewMatrix;
    }

    /// <summary>
    ///     Calcula la matríz de vista
    /// </summary>
    protected abstract Matrix CalculateViewMatrix();

    /// <summary>
    ///     Obtiene la matriz de proyección
    /// </summary>
    public abstract Matrix GetProjectionMatrix();
       
    /// <summary>
    ///     Comprueba si un rectángulo está en el punto de vista de la cámara
    /// </summary>
    public abstract bool IsInView(RectangleF bounds);
       
    /// <summary>
    ///     Comprueba si un punto está en el punto de vista de la cámara
    /// </summary>
    public abstract bool IsInView(Vector2 point);
        
    /// <summary>
    ///     Transforma coordenadas de mundo en coordenadas de pantalla
    /// </summary>
    public abstract Vector2 WorldToScreen(Vector2 worldPos);

    /// <summary>
    ///     Transforma coordenadas de pantalla a coordenadas de mundo
    /// </summary>
    public abstract Vector2 ScreenToWorld(Vector2 screenPos);

    /// <summary>
    ///     Actualiza la cámara
    /// </summary>
    public void Update(GameContext gameContext)
    {
        Behaviors.Update(gameContext);
    }

    /// <summary>
    ///     Cambia la posición de la cámara
    /// </summary>
    public void SetPosition(Vector2 position)
    {
        State.Transform.Position = position;
        _matrixDirty = true;
    }

    /// <summary>
    ///     Mueve la cámara
    /// </summary>
    public void Move(Vector2 delta)
    {
        State.Transform.Position += delta;
        _matrixDirty = true;
    }

    /// <summary>
    ///     Cambia la rotación de la cámara
    /// </summary>
    public void SetRotation(float rotation)
    {
        State.Transform.Rotation = rotation;
        _matrixDirty = true;
    }

    /// <summary>
    ///     Rota la cámara
    /// </summary>
    public void Rotate(float deltaRadians)
    {
        State.Transform.Rotation += deltaRadians;
        _matrixDirty = true;
    }

    /// <summary>
    ///     Cambia el zoom de la cámara por un factor
    /// </summary>
    public void ZoomBy(float factor) 
    {
        State.ZoomBy(factor);
        _matrixDirty = true;
    }

    /// <summary>
    ///     Cambia el zoom de la cámara
    /// </summary>
    public void SetZoom(float zoom)
    {
        State.SetZoom(zoom);
        _matrixDirty = true;
    }
        
    /// <summary>
    ///     Nombre de la cámara
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Director al que se asocia la cámara
    /// </summary>
    public CameraDirector Director { get; }

    /// <summary>
    ///     Tipo de la cámara
    /// </summary>
    public CameraType Type { get; }

    /// <summary>
    ///     Estado de la cámara
    /// </summary> 
    public CameraState State { get; }
 
    /// <summary>
    ///     Configuración del viewport
    /// </summary>
    public CameraViewport CameraViewport { get; }

    /// <summary>
    ///     Indice de dibujo
    /// </summary>
    public int ZIndex { get; }

    /// <summary>
    ///     Comportamientos asociados a la cámara
    /// </summary>
    public Behaviors.BehaviorsList Behaviors { get; } = new();

    /// <summary>
    ///     String de la cámara
    /// </summary>
    public override string ToString() => $"Camera '{Name}' ({Type}): {State}";
}