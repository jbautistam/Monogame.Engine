using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes;

/// <summary>
///		Clase base para la escena
/// </summary>
public abstract class AbstractScene
{
    protected AbstractScene(SceneManager sceneManager, string name, Entities.Common.WorldDefinitionModel? worldDefinition)
    {
        // Asigna las propiedades
        SceneManager = sceneManager;
        Name = name;
        WorldDefinition = worldDefinition ?? new Entities.Common.WorldDefinitionModel(5_000, 5_000, 10, 10);
        // Asigna los managers
        Camera = new Cameras.Camera2D(this, GetViewPort());
        LayerManager = new Layers.LayerManager(this);
        PhysicsManager = new Physics.PhysicsManager(this);
        MessagesManager = new Messages.MessagesManager(this);
        RenderingManager = new Rendering.RenderingManager(this);
        TimerManager = new Timers.TimerManager(this);
        // Inicializa el manejador de eventos de cambio de tamaño de pantalla
        SceneManager.EngineManager.MonogameServicesManager.ViewPortChanged += (sender, args) => UpdateViewPort();
    }

    /// <summary>
    ///     Carga un asset de la escena
    /// </summary>
    public TypeAsset? LoadSceneAsset<TypeAsset>(string asset) where TypeAsset : class
    {
        return SceneManager.EngineManager.ResourcesManager.GlobalContentManager.LoadAsset<TypeAsset>(asset);
    }

    /// <summary>
    ///     Actualiza la escena
    /// </summary>
    public string? Update(Managers.GameContext gameContext)
    {   
        // Actualiza los datos de la escena
        PhysicsManager.Update(gameContext);
        Camera.Update(gameContext);
        MessagesManager.Update(gameContext);
        // Actualiza la escena
        return UpdateScene(gameContext);
    }

    /// <summary>
    ///     Actualiza la escena
    /// </summary>
    protected abstract string? UpdateScene(Managers.GameContext gameContext);

    /// <summary>
    ///     Cambia la definición del mundo
    /// </summary>
	protected void UpdateWorldDefinition(int width, int height)
	{
		WorldDefinition = new Entities.Common.WorldDefinitionModel(width, height, WorldDefinition.CellWidth, WorldDefinition.CellHeight);
	}

    /// <summary>
    ///     Dibuja la escena
    /// </summary>
    public void Draw(Managers.GameContext gameContext)
    {
		// Comienza el dibujo
        RenderingManager.BeginDrawWorld();
        // Dibuja las capas de fondo / partida
        foreach (Layers.AbstractLayer layer in LayerManager.Layers)
            if (layer.Enabled && layer.Type != Layers.AbstractLayer.LayerType.UserInterface)
                layer.Draw(RenderingManager, gameContext);
		// Dibuja la capa de log
        SceneManager.EngineManager.DebugManager.DrawLogFigures(RenderingManager, gameContext);
		// Comienza el dibujo de la interface de usuario
        RenderingManager.BeginDrawUI();
        // Dibuja las capas de interface de usuario
        foreach (Layers.AbstractLayer layer in LayerManager.Layers)
            if (layer.Enabled && layer.Type == Layers.AbstractLayer.LayerType.UserInterface)
                layer.Draw(RenderingManager, gameContext);
        // Dibuja la capa de log
        SceneManager.EngineManager.DebugManager.DrawLogStrings(RenderingManager, gameContext);
		// Finaliza el dibujo
        RenderingManager.End();
    }

    /// <summary>
    ///     Arranca la escena
    /// </summary>
    public void Start()
    {
        StartScene();
    }

    /// <summary>
    ///     Actualiza el ViewPort cuando se cambia
    /// </summary>
    private void UpdateViewPort()
    {
        Camera?.UpdateViewPort(GetViewPort());
        UpdateViewPortSelf();
    }

    /// <summary>
    ///     Rutina para cambiar los datos de la escena cuando se ha cambiado el ViewPort
    /// </summary>
    protected virtual void UpdateViewPortSelf()
    {
    }

    /// <summary>
    ///     Obtiene el viewport de la escena
    /// </summary>
    public Viewport GetViewPort() => SceneManager.EngineManager.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice.Viewport;

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
        SceneManager?.EngineManager.AudioManager.Stop();
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
    ///     Manager de scenas
    /// </summary>
    public SceneManager SceneManager { get; }

	/// <summary>
	///		Nombre de la escena
	/// </summary>
	public string Name { get; }

    /// <summary>
    ///     Cámara de la escena
    /// </summary>
    public Cameras.Camera2D Camera { get; private set; }

    /// <summary>
    ///     Definición del mundo
    /// </summary>
    public Entities.Common.WorldDefinitionModel WorldDefinition { get; private set; }

    /// <summary>
    ///     Manager de dibujo
    /// </summary>
    public Rendering.RenderingManager RenderingManager { get; private set; }

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