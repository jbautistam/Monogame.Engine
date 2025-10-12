using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas
/// </summary>
public abstract class AbstractLayer(AbstractScene scene, string name, AbstractLayer.LayerType type, int sortOrder)
{
    /// <summary>
    ///     Tipo de capa
    /// </summary>
    public enum LayerType
    {
        /// <summary>Capa de fondo</summary>
        Background,
        /// <summary>Capa de partida</summary>
        Game,
        /// <summary>Capa de interface de usuario</summary>
        UserInterface
    }

    /// <summary>
    ///     Arranca la capa
    /// </summary>
    public void Start()
    {
        // Arranca la capa
        StartLayer();
        // Arranca los actores
        foreach (AbstractActor actor in Actors)
            actor.Start();
    }

    /// <summary>
    ///     Arranca el contenido de la capa
    /// </summary>
    protected abstract void StartLayer();

    /// <summary>
    ///     Actualiza los actores de la capa
    /// </summary>
    public void Update(GameTime gameTime)
    {
        // Actualiza la capa
        UpdateLayer(gameTime);
        // Actualiza las físicas de los actores (antes de actualizar los actores)
        foreach (AbstractActor actor in Actors)
            if (actor.Enabled)
                actor.UpdatePhisics(gameTime);
        // Actualiza los actores
        foreach (AbstractActor actor in Actors) 
            if (actor.Enabled) 
                actor.Update(gameTime);
    }

    /// <summary>
    ///     Actualiza la capa
    /// </summary>
    protected abstract void UpdateLayer(GameTime gameTime);

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	public void Draw(Cameras.Camera2D camera, GameTime gameTime)
	{
        // Dibuja la capa
        DrawLayer(camera, gameTime);
        // Dibuja los actores
        foreach (AbstractActor actor in Actors)
            if (actor.Enabled)
                actor.Draw(camera, gameTime);
	}

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	protected abstract void DrawLayer(Cameras.Camera2D camera, GameTime gameTime);

    /// <summary>
    ///     Finaliza la capa
    /// </summary>
    public void End()
    {
        // Limpia los datos
        Actors.Clear();
        // Finaliza la capa
        EndLayer();
    }

    /// <summary>
    ///     Finaliza los datos internos de la capa
    /// </summary>
    protected abstract void EndLayer();

	/// <summary>
	///     Escena padre de la capa
	/// </summary>
	public AbstractScene Scene { get; } = scene;

	/// <summary>
	///		Nombre de la capa
	/// </summary>
	public string Name { get; } = name;

    /// <summary>
    ///     Tipo de capa
    /// </summary>
    public LayerType Type { get; } = type;

    /// <summary>
    ///     Orden de dibujo de la capa
    /// </summary>
    public int SortOrder { get; } = sortOrder;

    /// <summary>
    ///     Orden calculado
    /// </summary>
    public int Order => ((int) Type) * 100 + SortOrder;

    /// <summary>
    ///     Indica si la capa está activa
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     Actores asociados a la capa
    /// </summary>
    public List<AbstractActor> Actors { get; } = [];
}
