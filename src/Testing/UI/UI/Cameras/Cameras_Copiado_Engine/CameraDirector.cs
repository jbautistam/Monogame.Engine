using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Cameras;

/// <summary>
///     Director de cámaras de una escena
/// </summary>
public class CameraDirector(Scene.Scene scene, Viewport viewport)
{
    // Variables privadas
    private Dictionary<AbstractCameraBase, RenderQueue> _renderQueues = [];

    /// <summary>
    ///     Crea una cámara de mundo
    /// </summary>
    public WorldCamera CreateWorldCamera(string name, Vector2 origin, CameraViewport cameraViewport)
    {
        WorldCamera camera = new(this, name, origin, cameraViewport);

            // Añade la cámara
            Cameras.Add(camera);
            // Añade la cola de renderización de la cámara
            _renderQueues[camera] = new RenderQueue();
            // Devuelve la cámara
            return camera;
    }

    /// <summary>
    ///     Crea una cámara relativa con coordenadas 0 a 1
    /// </summary>
    public ScreenCamera CreateScreenCamera(string name, Vector2 origin, CameraViewport cameraViewport)
    {
        ScreenCamera camera = new(this, name, origin, cameraViewport);

            // Añade la cámara
            Cameras.Add(camera);
            // Añade la cámara a la cola de render
            _renderQueues[camera] = new RenderQueue();
            // Devuelve la cámara creada
            return camera;
    }

    /// <summary>
    ///     Crea una cámara híbrida
    /// </summary>
    public HybridCamera CreateHybridCamera(string name, Vector2 origin, float pixelsPerUnit, CameraViewport cameraViewport)
    {
        HybridCamera camera = new(this, name, origin, cameraViewport)
                                    {
                                        PixelsPerUnit = pixelsPerUnit
                                    };

            // Añade la cámara
            Cameras.Add(camera);
            // Añade la cámara a la cola de representación
            _renderQueues[camera] = new RenderQueue();
            // Devuelve la cámara creada
            return camera;
    }

    /// <summary>
    ///     Obtiene la cola asociada a una cámara
    /// </summary>
    public RenderQueue? GetRenderQueue(AbstractCameraBase camera)
    {
        if (_renderQueues.TryGetValue(camera, out var queue))
            return queue;
        else
            return null;
    }

    /// <summary>
    ///     Actualiza las cámaras
    /// </summary>
    public void Update(GameTime gameTime)
    {
        foreach (AbstractCameraBase camera in Cameras)
            camera.Update(gameTime);
    }

    /// <summary>
    ///     Añade un comando a una cámara
    /// </summary>
    public void Submit(AbstractCameraBase camera, RenderCommand command)
    {
        RenderQueue? queue = GetRenderQueue(camera);

            // Encola el comando
            if (queue is not null)
                queue.Enqueue(command);
    }

    /// <summary>
    ///     Renderiza los comandos sobre la cámara
    /// </summary>
    public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        foreach (AbstractCameraBase camera in Cameras)
        {
            RenderQueue? queue = GetRenderQueue(camera);

                if (queue is not null)
                {
                    CameraViewport viewport = camera.CameraViewport;
                    Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;
                
                        // Actualiza el vieport de dibujo
                        spriteBatch.GraphicsDevice.Viewport = viewport.ToViewport(Viewport);
                        // Arranca el spritebatch
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                                          SamplerState.PointClamp, null, null, null,
                                          camera.GetViewMatrix() * camera.GetProjectionMatrix());
                        // Ejecuta los comandos de la cola
                        queue.Execute(spriteBatch, defaultFont);
                        // Finaliza el dibujo
                        spriteBatch.End();
                        // Limpia la cola
                        queue.Clear();
                        // Recupera el viewport inicial
                        spriteBatch.GraphicsDevice.Viewport = oldViewport;
                }
        }
    }

    /// <summary>
    ///     Escena a la que se asocia el director de cámaras
    /// </summary>
    public Scene.Scene Scene { get; } = scene;

    /// <summary>
    ///     Viewport real de la pantalla
    /// </summary>
    public Viewport Viewport { get; set; } = viewport;

    /// <summary>
    ///     Cámaras asociadas
    /// </summary>
    public List<AbstractCameraBase> Cameras { get; } = [];
}
