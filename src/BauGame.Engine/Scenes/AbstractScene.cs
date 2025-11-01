﻿using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes;

/// <summary>
///		Clase base para la escena
/// </summary>
public abstract class AbstractScene
{
    protected AbstractScene(string name, Rectangle? worldBounds)
    {
        Name = name;
        WorldBounds = worldBounds ?? new Rectangle();
        LayerManager = new Layers.LayerManager(this);
        AudioManager = new Audio.AudioManager(this);
        PhysicsManager = new Physics.PhysicsManager(this);
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
        AudioManager.Update(gameContext);
        PhysicsManager.Update(gameContext);
        Camera?.Update(gameContext);
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
        if (Camera is not null)
            LayerManager.Draw(Camera, gameContext);
    }

    /// <summary>
    ///     Arranca la escena
    /// </summary>
    public void Start()
    {
        // Inicializa la cámara
        Camera = new Cameras.Camera2D(this, GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice.Viewport);
        // Arranca la escena
        StartScene();
    }

    /// <summary>
    ///     Arranca la escena
    /// </summary>
	protected abstract void StartScene();

    /// <summary>
    ///     Finaliza la escena
    /// </summary>
    public void End()
    {
        // Detiene el audio
        AudioManager.Stop();
        // Finaliza las capas
        LayerManager.End();
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
    ///     Límites del mundo
    /// </summary>
    public Rectangle WorldBounds { get; private set; }

    /// <summary>
    ///     Manager de físicas
    /// </summary>
    public Physics.PhysicsManager PhysicsManager { get; }

    /// <summary>
    ///     Manager de capas de la escena
    /// </summary>
    public Layers.LayerManager LayerManager { get; }

    /// <summary>
    ///     Manager de audio
    /// </summary>
    public Audio.AudioManager AudioManager { get; }

    /// <summary>
    ///     Indica si la escena está detenida
    /// </summary>
    public bool IsPaused { get; set; }
}
