using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes;

/// <summary>
///		Clase base para la escena
/// </summary>
public abstract class AbstractScene
{
    protected AbstractScene(string name, Entities.Common.WorldDefinitionModel? worldDefinition)
    {
        Name = name;
        WorldDefinition = worldDefinition ?? new Entities.Common.WorldDefinitionModel(5_000, 5_000, 10, 10);
        LayerManager = new Layers.LayerManager(this);
        PhysicsManager = new Physics.PhysicsManager(this);
        MessagesManager = new Messages.MessagesManager(this);
        TimerManager = new Timers.TimerManager(this);
        CameraDirector = new Cameras.CameraDirector(this);
    }

    /// <summary>
    ///     Carga un asset de la escena
    /// </summary>
    public TypeAsset? LoadSceneAsset<TypeAsset>(string asset) where TypeAsset : class
    {
        return GameEngine.Instance.ResourcesManager.GlobalContentManager.LoadAsset<TypeAsset>(asset);
    }

    /// <summary>
    ///     Actualiza la escena
    /// </summary>
    public AbstractScene? Update(Managers.GameContext gameContext)
    {   
        // Actualiza los datos de la escena
        PhysicsManager.Update(gameContext);
        Camera?.Update(gameContext);
        MessagesManager.Update(gameContext);
        // Actualiza la escena
        return UpdateScene(gameContext);
    }

    /// <summary>
    ///     Actualiza la escena
    /// </summary>
    protected abstract AbstractScene? UpdateScene(Managers.GameContext gameContext);

    /// <summary>
    ///     Dibuja la escena
    /// </summary>
    public void Draw(Managers.GameContext gameContext)
    {
        if (CameraDirector.Cameras.Count > 0)
        {
            // Prepara los comandos
            LayerManager.PrepareRenderCommands(gameContext);
            // Dibuja los comandos
            CameraDirector.Render(GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice);
        }
        else if (Camera is not null)
            LayerManager.Draw(Camera, gameContext);
    }

    /// <summary>
    ///     Arranca la escena
    /// </summary>
    public void Start()
    {
        // Inicializa la cámara
        Camera = new Cameras.Camera2D(this, GetViewPort());
        // Arranca la escena
        StartScene();
    }

    /// <summary>
    ///     Obtiene el viewport de la escena
    /// </summary>
    public Viewport GetViewPort() => GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice.Viewport;

    /// <summary>
    ///     Arranca la escena
    /// </summary>
	protected abstract void StartScene();

    /// <summary>
    ///     Finaliza la escena
    /// </summary>
    public void End(Managers.GameContext gameContext)
    {
        // Detiene el audio
        GameEngine.Instance.AudioManager.Stop();
        // Finaliza las capas
        LayerManager.End(gameContext);
        // Finaliza la escena
        EndScene();
    }

    /// <summary>
    ///     Finaliza la escena
    /// </summary>
	protected abstract void EndScene();

	/// <summary>
	///		Nombre de la escena
	/// </summary>
	public string Name { get; }

    /// <summary>
    ///     Cámara de la escena
    /// </summary>
    public Cameras.Camera2D? Camera { get; private set; }

    /// <summary>
    ///     Director de cámaras
    /// </summary>
    public Cameras.CameraDirector CameraDirector { get; }

    /// <summary>
    ///     Definición del mundo
    /// </summary>
    public Entities.Common.WorldDefinitionModel WorldDefinition { get; private set; }

    /// <summary>
    ///     Manager de físicas
    /// </summary>
    public Physics.PhysicsManager PhysicsManager { get; }

    /// <summary>
    ///     Manager de capas de la escena
    /// </summary>
    public Layers.LayerManager LayerManager { get; }

    /// <summary>
    ///     Manager de mensajes
    /// </summary>
    public Messages.MessagesManager MessagesManager { get; }

    /// <summary>
    ///     Manager para temporizadores
    /// </summary>
    public Timers.TimerManager TimerManager { get; }

    /// <summary>
    ///     Indica si la escena está detenida
    /// </summary>
    public bool IsPaused { get; set; }
}