using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///     Manager para las capas de una escena
/// </summary>
public class LayerManager(AbstractScene scene)
{
    /// <summary>
    ///     Añade una capa
    /// </summary>
    public AbstractLayer AddLayer(AbstractLayer layer)
    {
        // Añade la capa
        Layers.Add(layer);
        // Ordena las capas
        Layers.Sort((first, second) => first.Order.CompareTo(second.Order));
        // y la devuelve
        return layer;
    }

    /// <summary>
    ///     Arranca las capas
    /// </summary>
	public void Start()
	{
        foreach (AbstractLayer layer in Layers) 
            if (layer.Enabled)
                layer.Start();
	}

    /// <summary>
    ///     Limmpia las capas
    /// </summary>
	public void Clear()
	{
		Layers.Clear();
	}

    /// <summary>
    ///     Actualiza los datos de la capa
    /// </summary>
    public void Update(GameContext gameContext)
    {
        foreach (AbstractLayer layer in Layers) 
            if (layer.Enabled)
                layer.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja las capas
    /// </summary>
    public void Draw(Cameras.Camera2D camera, GameContext gameContext)
    {
		// Comienza el dibujo
        camera.BeginDrawWorld();
        // Dibuja las capas de fondo / partida
        foreach (AbstractLayer layer in Layers)
            if (layer.Enabled && layer.Type != AbstractLayer.LayerType.UserInterface)
                layer.Draw(camera, gameContext);
		// Dibuja la capa de log
        GameEngine.Instance.DebugManager.DrawLogFigures(gameContext, camera);
		// Comienza el dibujo de la interface de usuario
        camera.BeginDrawUI();
        // Dibuja las capas de interface de usuario
        foreach (AbstractLayer layer in Layers)
            if (layer.Enabled && layer.Type == AbstractLayer.LayerType.UserInterface)
                layer.Draw(camera, gameContext);
        // Dibuja la capa de log
        GameEngine.Instance.DebugManager.DrawLogStrings(gameContext, camera);
		// Finaliza el dibujo
        camera.EndDraw();
    }

    /// <summary>
    ///     Finaliza las capas
    /// </summary>
	public void End(GameContext gameContext)
	{
	    foreach (AbstractLayer layer in Layers)
            layer.End(gameContext);
	}

	/// <summary>
	///     Escena a la que se asocia la capa
	/// </summary>
	public AbstractScene Scene { get; } = scene;

    /// <summary>
    ///     Capas
    /// </summary>
    public List<AbstractLayer> Layers { get; } = [];
}