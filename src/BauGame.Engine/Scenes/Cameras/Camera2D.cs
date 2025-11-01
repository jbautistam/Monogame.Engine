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
    private Vector2 _position;
    private float _rotation, _zoom;

    public Camera2D(AbstractScene scene, Viewport viewport)
    {
        // Asigna los objectos
        Scene = scene;
        ScreenViewport = viewport;
        Zoom = 1.0f;
        Origin = new Vector2(0.5f * ScreenViewport.Width, 0.5f * ScreenViewport.Height);
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
            Origin = new Vector2(0.5f * ScreenViewport.Width, 0.5f * ScreenViewport.Height);
            // Crea la matriz de transformación
            Matrix transform = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                               Matrix.CreateRotationZ(Rotation) *
                               Matrix.CreateScale(Zoom, Zoom, 1) *
                               Matrix.CreateTranslation(new Vector3(Origin, 0));

                // Obtiene las matrices de salida (utiliza una matriz intermedia porque no se puede hacer un Invert de un Matrix?
                _transformMatrix = transform;
                _inverseMatrix = Matrix.Invert(_transformMatrix ?? new Matrix());
        }
    }

/*
    public void RecalculateMatrices()
    {
        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;

        // Escalado uniforme SIN bandas negras → ajusta al mínimo que cubra toda la pantalla
        float scaleX = (float)screenWidth / DesignWidth;
        float scaleY = (float)screenHeight / DesignHeight;
        _scaleFactor = Math.Max(scaleX, scaleY); // ¡Math.Max elimina bandas negras!

        // Viewport lógico escalado (puede ser más grande que la pantalla)
        int scaledWidth = (int)(DesignWidth * _scaleFactor);
        int scaledHeight = (int)(DesignHeight * _scaleFactor);

        // Matriz para UI/HUD: se dibuja en coordenadas de diseño (1280x720), escalado a pantalla
        UIScaleMatrix = Matrix.CreateScale(_scaleFactor);

        // Centro del mundo en la cámara
        Vector2 cameraScreenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);

        // Matriz de vista del mundo: traslada el mundo para que WorldCameraPosition esté centrado
        WorldViewMatrix =
            Matrix.CreateTranslation(new Vector3(-WorldCameraPosition.X, -WorldCameraPosition.Y, 0)) *
            Matrix.CreateScale(WorldZoom) *
            Matrix.CreateTranslation(new Vector3(cameraScreenCenter, 0));
    }
*/

    /// <summary>
    ///     Actualiza el viewPort (normalmente por un cambio en el tamaño de la pantalla)
    /// </summary>
    private void UpdateViewPort()
    {
        ScreenViewport = GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice.Viewport;
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
    ///     Conversión de coordenadas relativas a coordenadas de patalla
    /// </summary>
    public Vector2 WorldToScreenRelative(Vector2 relativePosition)
    {
        Vector2 topLeft = Position - new Vector2(ScreenViewport.Width * 0.5f, ScreenViewport.Height * 0.5f) / Zoom;
        Vector2 offset = new(relativePosition.X * ScreenViewport.Width / Zoom,
                             relativePosition.Y * ScreenViewport.Height / Zoom);

            // Devuelve la posición
            return WorldToScreen(topLeft + offset);
    }

    /// <summary>
    ///     Convierte coordenadas de pantalla a coordenadas de mundo
    /// </summary>
    public RectangleF ScreenToWorldRect(Rectangle screenRect)
    {
        Matrix inverseTransform = _inverseMatrix ?? new Matrix();
        Vector2 topLeft = Vector2.Transform(new Vector2(screenRect.Left, screenRect.Top), inverseTransform);
        Vector2 topRight = Vector2.Transform(new Vector2(screenRect.Right, screenRect.Top), inverseTransform);
        Vector2 bottomLeft = Vector2.Transform(new Vector2(screenRect.Left, screenRect.Bottom), inverseTransform);
        Vector2 bottomRight = Vector2.Transform(new Vector2(screenRect.Right, screenRect.Bottom), inverseTransform);
        float minX = MathHelper.Min(MathHelper.Min(topLeft.X, topRight.X), MathHelper.Min(bottomLeft.X, bottomRight.X));
        float minY = MathHelper.Min(MathHelper.Min(topLeft.Y, topRight.Y), MathHelper.Min(bottomLeft.Y, bottomRight.Y));
        float maxX = MathHelper.Max(MathHelper.Max(topLeft.X, topRight.X), MathHelper.Max(bottomLeft.X, bottomRight.X));
        float maxY = MathHelper.Max(MathHelper.Max(topLeft.Y, topRight.Y), MathHelper.Max(bottomLeft.Y, bottomRight.Y));

            // Devuelve el rectángulo convertido
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    /// <summary>
    ///     Convierte coordenadas de mundo a coordenadas de pantalla
    /// </summary>
    public Rectangle WorldToScreenRect(RectangleF worldRect)
    {
        Vector2 topLeft = WorldToScreen(new Vector2(worldRect.Left, worldRect.Top));
        Vector2 topRight = WorldToScreen(new Vector2(worldRect.Right, worldRect.Top));
        Vector2 bottomLeft = WorldToScreen(new Vector2(worldRect.Left, worldRect.Bottom));
        Vector2 bottomRight = WorldToScreen(new Vector2(worldRect.Right, worldRect.Bottom));
        float minX = MathHelper.Min(MathHelper.Min(topLeft.X, topRight.X), MathHelper.Min(bottomLeft.X, bottomRight.X));
        float minY = MathHelper.Min(MathHelper.Min(topLeft.Y, topRight.Y), MathHelper.Min(bottomLeft.Y, bottomRight.Y));
        float maxX = MathHelper.Max(MathHelper.Max(topLeft.X, topRight.X), MathHelper.Max(bottomLeft.X, bottomRight.X));
        float maxY = MathHelper.Max(MathHelper.Max(topLeft.Y, topRight.Y), MathHelper.Max(bottomLeft.Y, bottomRight.Y));

            // Devuelve el rectángulo convertido
            return new Rectangle((int) minX, (int) minY, (int) (maxX - minX), (int) (maxY - minY));
    }

    /// <summary>
    ///     Comprueba si un elemento está en la lista
    /// </summary>
    public bool IsInView(RectangleF bounds)
    {
        // Convertimos esquinas del bounding box a pantalla
        Vector2 topLeft = WorldToScreen(new Vector2(bounds.Left, bounds.Top));
        Vector2 bottomRight = WorldToScreen(new Vector2(bounds.Right, bounds.Bottom));
        Rectangle screenRect = new((int) Math.Min(topLeft.X, bottomRight.X), (int) Math.Min(topLeft.Y, bottomRight.Y),
                                   (int) Math.Abs(bottomRight.X - topLeft.X), (int) Math.Abs(bottomRight.Y - topLeft.Y));

            // Comprobamos si intersecta con el viewport
            return screenRect.Intersects(ScreenViewport.Bounds);
    }

    /// <summary>
    ///     Limita un punto a los límites del mundo
    /// </summary>
    private Vector2 Clamp(Vector2 point)
    {
        float zoom = Zoom == 0 ? 1 : Zoom;
        float viewWidth = 0.5f * ScreenViewport.Width / zoom;
        float viewHeight = 0.5f * ScreenViewport.Height / zoom;

        // Opción avanzada (con rotación) — descomenta si necesitas soporte para rotación
        /*
        float diagonal = (float)Math.Sqrt(ScreenViewport.Width * ScreenViewport.Width +
                                            ScreenViewport.Height * ScreenViewport.Height);
        float maxViewRadius = diagonal / (2f * Zoom);
        float viewWidth = maxViewRadius * 2;
        float viewHeight = maxViewRadius * 2;
        */
        float clampedX = MathHelper.Clamp(point.X, Scene.WorldBounds.X + viewWidth, Scene.WorldBounds.Width - viewWidth);
        float clampedY = MathHelper.Clamp(point.Y, Scene.WorldBounds.Y + viewHeight, Scene.WorldBounds.Height - viewHeight);

            // Devuelve el vector limitado
            return new Vector2(clampedX, clampedY);
    }

    /// <summary>
    ///     Actualiza la cámara
    /// </summary>
	public void Update(GameContext gameContext)
	{
        Vector2? target = TargetsManager.LookAt();

            // Si hay algún objetivo definido, actualiza la posición de la cámara
		    if (target is not null)
                Position = Clamp(Vector2.Lerp(Position, target ?? new Vector2(), FollowSpeed));
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
    ///     Origen de la cámara
    /// </summary>
    public Vector2 Origin { get; set; }

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
            Origin = new Vector2(0.5f * _screenViewport.Width, 0.5f * _screenViewport.Height);
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
    ///     Controlador de sprites
    /// </summary>
    public SpriteBatchController SpriteBatchController { get; } = new();
}