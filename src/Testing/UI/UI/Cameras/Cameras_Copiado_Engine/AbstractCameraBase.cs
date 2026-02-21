using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Cameras;

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
    protected readonly List<AbstractCameraBehaviorBase> _behaviors = [];
    private Matrix _viewMatrix;
    private bool _matrixDirty = true;

    protected AbstractCameraBase(CameraDirector director, string name, CameraType type, Vector2 origin, CameraViewport cameraViewport)
    {
        Name = name;
        Type = type;
        Director = director;
        State = CameraState.Default(origin);
        CameraViewport = cameraViewport;
    }

    /// <summary>
    ///     Obtiene la matriz de la vista
    /// </summary>
    public Matrix GetViewMatrix()
    {
        if (_matrixDirty || State.Transform.IsDirty)
        {
            _viewMatrix = CalculateViewMatrix();
            _matrixDirty = false;
        }
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
    public void Update(GameTime gameTime)
    {
        float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
        for (int i = _behaviors.Count - 1; i >= 0; i--)
        {
            var behavior = _behaviors[i];
                
            if (behavior.IsActive == false) continue;
                
            behavior.Update(delta);
                
            if (behavior.IsComplete && behavior.Duration.HasValue)
                _behaviors.RemoveAt(i);
        }
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

    public void AddBehavior(AbstractCameraBehaviorBase behavior)
    {
        _behaviors.Add(behavior);
    }

    public void RemoveBehavior(AbstractCameraBehaviorBase behavior)
    {
        _behaviors.Remove(behavior);
    }

    public T? GetBehavior<T>() where T : AbstractCameraBehaviorBase
    {
        return _behaviors.OfType<T>().FirstOrDefault();
    }

    public void ClearBehaviors()
    {
        _behaviors.Clear();
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
    ///     String de la cámara
    /// </summary>
    public override string ToString() => $"Camera '{Name}' ({Type}): {State}";
}