using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Scenes.Layers;

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
    ///     Actualiza los datos de la capa
    /// </summary>
    public void Update(GameTime gameTime)
    {
        foreach (AbstractLayer layer in Layers) 
            if (layer.Enabled)
                layer.Update(gameTime);
    }

    /// <summary>
    ///     Dibuja las capas
    /// </summary>
    public void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
		// Comienza el dibujo
        camera.BeginDrawWorld();
        // Dibuja las capas de fondo / partida
        foreach (AbstractLayer layer in Layers)
            if (layer.Enabled && layer.Type != AbstractLayer.LayerType.UserInterface)
                layer.Draw(camera, gameTime);
		// Comienza el dibujo de la interface de usuario
        camera.BeginDrawUI();
        // Dibuja las capas de fondo / partida
        foreach (AbstractLayer layer in Layers)
            if (layer.Enabled && layer.Type == AbstractLayer.LayerType.UserInterface)
                layer.Draw(camera, gameTime);
		// Finaliza el dibujo
        camera.EndDraw();
    }

    /// <summary>
    ///     Finaliza las capas
    /// </summary>
	internal void End()
	{
	    foreach (AbstractLayer layer in Layers)
            layer.End();
	}

    /// <summary>
    ///     Obtiene una capa por su nombre
    /// </summary>
	public TypeData? Get<TypeData>(string layer) where TypeData : AbstractLayer
	{
        return Layers.FirstOrDefault(item => item.Name.Equals(layer, StringComparison.CurrentCultureIgnoreCase)) as TypeData;
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
