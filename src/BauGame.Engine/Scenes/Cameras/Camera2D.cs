using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras;

/// <summary>
///     Control de cámara
/// </summary>
public class Camera2D
{
    // Variables privadas
    private Viewport _screenViewport;
    private Matrix? _transformMatrix, _inverseMatrix;
    private Vector2 _position, _desiredPosition, _velocity;
    private float _rotation, _zoom;

    public Camera2D(AbstractScene scene, Viewport viewport)
    {
        // Asigna los objectos
        Scene = scene;
        ScreenViewport = viewport;
        Zoom = 1.0f;
        ViewPortCenter = new Vector2(0.5f * ScreenViewport.Width, 0.5f * ScreenViewport.Height);
        Position = new Vector2(0, 0);
        // Inicializa el manejador de eventos de cambio de tamaño de pantalla
        GameEngine.Instance.MonogameServicesManager.ViewPortChanged += (sender, args) => UpdateViewPort();
    }

    /// <summary>
    ///     Actualiza las matrices
    /// </summary>
    private void UpdateMatrices()
    {
        if (_transformMatrix is null)
        {
            // Actualiza el origen
            ViewPortCenter = new Vector2(0.5f * ScreenViewport.Width, 0.5f * ScreenViewport.Height);
            // Crea la matriz de transformación
            Matrix transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                               Matrix.CreateRotationZ(Rotation) *
                               Matrix.CreateScale(Zoom, Zoom, 1) *
                               Matrix.CreateTranslation(ViewPortCenter.X, ViewPortCenter.Y, 0);

                // Obtiene las matrices de salida (utiliza una matriz intermedia porque no se puede hacer un Invert de un Matrix?)
                _transformMatrix = transform;
                _inverseMatrix = Matrix.Invert(_transformMatrix ?? new Matrix());
        }
    }

    /// <summary>
    ///     Actualiza el viewPort (normalmente por un cambio en el tamaño de la pantalla)
    /// </summary>
    private void UpdateViewPort()
    {
        ScreenViewport = GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice.Viewport;
        _transformMatrix = null;
    }

    /// <summary>
    ///     Conversión de coordenadas de pantalla a coordenadas de mundo
    /// </summary>
    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        // Actualiza las matrices
        UpdateMatrices();
        // Transforma el vector (después del update, ya existe la matriz)
        return Vector2.Transform(screenPosition, _inverseMatrix!.Value);
    }

    /// <summary>
    ///     Conversión de coordenadas de mundo a coordenadas de pantalla
    /// </summary>
    public Vector2 WorldToScreen(Vector2 worldPosition)
    {
        // Actualiza las matrices
        UpdateMatrices();
        // Transforma el vector (después del update, ya existe la matriz)
        return Vector2.Transform(worldPosition, _transformMatrix!.Value);
    }

    /// <summary>
    ///     Convierte coordenadas de mundo a coordenadas de pantalla
    /// </summary>
    public Rectangle WorldToScreenRect(RectangleF worldRect)
    {
        Vector2 origin = WorldToScreen(worldRect.TopLeft);

            // Devuelve el rectángulo convertido
            return new Rectangle((int) origin.X, (int) origin.Y, (int) worldRect.Width, (int) worldRect.Height);
    }

    /// <summary>
    ///     Conversión de coordenadas relativas a coordenadas de patalla
    /// </summary>
    public Vector2 WorldToScreenRelative(Vector2 relativePosition)
    {
        Vector2 topLeft = Position - ViewPortCenter / Zoom;
        Vector2 offset = new(relativePosition.X * ScreenViewport.Width / Zoom, relativePosition.Y * ScreenViewport.Height / Zoom);

            // Devuelve la posición
            return WorldToScreen(topLeft + offset);
    }

    /// <summary>
    ///     Comprueba si un rectángulo está en la vista
    /// </summary>
    public bool IsAtView(RectangleF bounds) => WorldToScreenRect(bounds).Intersects(ScreenViewport.Bounds);

    /// <summary>
    ///     Comprueba si un punto está en la vista
    /// </summary>
    public bool IsAtView(Vector2 worldPoint)
    {
        Vector2 screen = WorldToScreen(worldPoint);

            return new Rectangle(0, 0, ScreenViewport.Width, ScreenViewport.Height).Contains((int) screen.X, (int) screen.Y);
    }

    /// <summary>
    ///     Actualiza la cámara
    /// </summary>
	public void Update(GameContext gameContext)
	{
        Vector2? target = TargetsManager.LookAt();

            // Si hay algún objetivo definido, actualiza la posición de la cámara
		    if (target is null)
                _desiredPosition = Position;
            else if (Scene.WorldDefinition.DeadZone.Width == 0 || Scene.WorldDefinition.DeadZone.Height == 0)
                _desiredPosition = target.Value;
            else
                _desiredPosition = ComputeDeadZone(target.Value, Scene.WorldDefinition.DeadZone);
            // Ejecuta el clamp de la posición deseada
            _desiredPosition = Clamp(_desiredPosition);
            // Mueve hacia la posición deseada
            Position = MoveTo(_desiredPosition, MathHelper.Clamp(gameContext.DeltaTime, 0f, 0.1f));

            GameEngine.Instance.DebugManager.Log($"Camera Position: {Position.X} / {Position.Y}");
	}

    /// <summary>
    ///     Calcula la posición destino teniendo en cuenta la zona muerta definida
    /// </summary>
    private Vector2 ComputeDeadZone(Vector2 target, Rectangle deadZone)
    {
        Vector2 deadCenter = new(deadZone.Center.X, deadZone.Center.Y);
        Vector2 offset = WorldToScreen(target) - deadCenter;

            // Calcula el destino
            if (Math.Abs(offset.X) > deadZone.Width * 0.5f || Math.Abs(offset.Y) > deadZone.Height * 0.5f)
                return target - ScreenToWorld(deadCenter);
            else
                return Position;
    }

    /// <summary>
    ///     Limita un punto a los límites del mundo
    /// </summary>
    private Vector2 Clamp(Vector2 point)
    {
        Vector2 target = new(point.X, point.Y);
        Vector2 halfView = new(ScreenViewport.Width / (2 * Zoom), ScreenViewport.Height / (2 * Zoom));

            // Normaliza el valor X
            if (Scene.WorldDefinition.WorldBounds.Width > ScreenViewport.Width / Zoom)
                target.X = MathHelper.Clamp(target.X, Scene.WorldDefinition.WorldBounds.Left + halfView.X, Scene.WorldDefinition.WorldBounds.Right - halfView.X);
            else
                target.X = Scene.WorldDefinition.WorldBounds.Center.X;
            // Normaliza el valor y
            if (Scene.WorldDefinition.WorldBounds.Height > ScreenViewport.Height / Zoom)
                target.Y = MathHelper.Clamp(target.Y, Scene.WorldDefinition.WorldBounds.Top + halfView.Y, Scene.WorldDefinition.WorldBounds.Bottom - halfView.Y);
            else
                target.Y = Scene.WorldDefinition.WorldBounds.Center.Y;
            // Devuelve el punto calculado
            return target;
    }

    /// <summary>
    ///     Interpola el movimiento de la cámara hacia un punto
    /// </summary>
    private Vector2 MoveTo(Vector2 target, float normalizedDeltaTime)
    {
        Vector2 error = target - Position;
        Vector2 acceleration = error * FollowAcceleration;

            // Calcula la velocidad en que se debe acercar a la posición deseada
            _velocity += acceleration * normalizedDeltaTime;
            _velocity *= MathHelper.Clamp(FollowDamping, 0f, 0.99f);
            // Si ya ha llegado al punto, devuelve el punto destino y detiene la cámara, si no, aplica la velocidad al punto actual
            if (error.LengthSquared() < 0.1f && _velocity.LengthSquared() < 0.01f)
            {
                _velocity = Vector2.Zero;
                return target;
            }
            else
                return Position + _velocity * normalizedDeltaTime;
    }

    /// <summary>
    ///     Aumenta el Zoom
    /// </summary>
    public void ZoomIn(float deltaZoom)
    {
        Zoom *= 1f + deltaZoom;
    }

    /// <summary>
    ///     Disminuye el zoom
    /// </summary>
    public void ZoomOut(float deltaZoom)
    {
        Zoom /= 1f + deltaZoom;
    }

    /// <summary>
    ///     Comienza el dibujo del mundo
    /// </summary>
	public void BeginDrawWorld()
	{
        // Actualiza las matrices
        UpdateMatrices();
        // Limpia la pantalla y arranca el dibujo
        SpriteBatchController.Clear();
        SpriteBatchController.BeginDraw(_transformMatrix);
	}

    /// <summary>
    ///     Arranca el dibujo de la UI
    /// </summary>
	public void BeginDrawUI()
	{
        SpriteBatchController.BeginDraw(null);
	}

    /// <summary>
    ///     Método para finalizar el lote de dibujo
    /// </summary>
    public void EndDraw()
    {
        SpriteBatchController.End();
    }

	/// <summary>
	///     Escena a la que se asocia la cámara
	/// </summary>
	public AbstractScene Scene { get; }

	/// <summary>
	///     Posición de la cámara
	/// </summary>
	public Vector2 Position 
    { 
        get { return _position; } 
        set 
        { 
            _position = Clamp(value);
            _transformMatrix = null;
        }
    }

    /// <summary>
    ///     Centro de la cámara
    /// </summary>
    public Vector2 ViewPortCenter { get; private set; }

    /// <summary>
    ///     Rotación de la cámara
    /// </summary>
    public float Rotation 
    { 
        get { return _rotation; }
        set 
        {
            _rotation = value;
            _transformMatrix = null;
        }
    }

    /// <summary>
    ///     Zoom de la cámara
    /// </summary>
    public float Zoom 
    { 
        get { return _zoom; }
        set
        {
            _zoom = value;
            _transformMatrix = null;
        }
    }

    /// <summary>
    ///     Viewport de la pantalla (en píxeles)
    /// </summary>
    public Viewport ScreenViewport
    { 
        get { return _screenViewport; }
        set
        {
            _screenViewport = value;
            ViewPortCenter = new Vector2(0.5f * _screenViewport.Width, 0.5f * _screenViewport.Height);
            _transformMatrix = null;
        }
    }

    /// <summary>
    ///     Objetivos donde debe mirar la cámara
    /// </summary>
    public CameraTargetsManager TargetsManager { get; } = new();

    /// <summary>
    ///     Velocidad de seguimiento de la cámara
    /// </summary>
    public float FollowSpeed { get; set; } = 0.1f;

    /// <summary>
    ///     Aceleración de seguimiento de la cámara
    /// </summary>
    public float FollowAcceleration { get; set; } = 80f;

    /// <summary>
    ///     Amortiguación a la aceleración de seguimiento de la cámara
    /// </summary>
    public float FollowDamping { get; set; } = 0.88f;

    /// <summary>
    ///     Controlador de sprites
    /// </summary>
    public SpriteBatchController SpriteBatchController { get; } = new();
}