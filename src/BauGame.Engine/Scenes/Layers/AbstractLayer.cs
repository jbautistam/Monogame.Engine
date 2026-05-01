using Bau.BauEngine.Actors;

namespace Bau.BauEngine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas
/// </summary>
public abstract class AbstractLayer : Entities.Common.Collections.ISecureListItem
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

    public AbstractLayer(AbstractScene scene, string id, LayerType type, int sortOrder)
    {
        // Asigna las propiedades
        Scene = scene;
        Id = id;
        Type = type;
        Actors = new(this);
        SortOrder = sortOrder;
        // Inicializa los objetos
        Services = new Services.LayerServicesList(this);
    }

    /// <summary>
    ///     Arranca la capa
    /// </summary>
    public void Start()
    {
        // Arranca la capa
        StartLayer();
        // Arranca los actores
        Actors.Start();
    }

    /// <summary>
    ///     Arranca el contenido de la capa
    /// </summary>
    protected abstract void StartLayer();

    /// <summary>
    ///     Actualiza los actores de la capa
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        // Actualiza las físicas de la capa
        UpdatePhysicsLayer(gameContext);
        // Actualiza las físicas de los actores (antes de actualizar los actores)
        Actors.UpdatePhysics(gameContext);
        // Actualiza los servicios
        Services.Update(gameContext);
        // Actualiza la capa
        UpdateSelf(gameContext);
        // Actualiza los actores
        Actors.Update(gameContext);
    }

    /// <summary>
    ///     Actualiza la capa
    /// </summary>
    protected abstract void UpdatePhysicsLayer(Managers.GameContext gameContext);

    /// <summary>
    ///     Actualiza la capa
    /// </summary>
    protected abstract void UpdateSelf(Managers.GameContext gameContext);

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	public void Draw(Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext)
	{
        // Dibuja la capa
        DrawSelf(renderingManager, gameContext);
        // Ordena los actores
        Actors.SortByZOrder();
        // Dibuja los actores
        Actors.Draw(renderingManager, gameContext);
	}

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	protected abstract void DrawSelf(Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext);

    /// <summary>
    ///     Finaliza la capa
    /// </summary>
    public void End(Managers.GameContext gameContext)
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
	public AbstractScene Scene { get; }

	/// <summary>
	///		Nombre de la capa
	/// </summary>
	public string Id { get; }

    /// <summary>
    ///     Tipo de capa
    /// </summary>
    public LayerType Type { get; }

    /// <summary>
    ///     Orden de dibujo de la capa
    /// </summary>
    public int SortOrder { get; }

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
    public ActorsList Actors { get; }

    /// <summary>
    ///     Servicios de la capa
    /// </summary>
    public Services.LayerServicesList Services { get; }

    /// <summary>
    ///     Cámaras sobre las que se dibuja la capa
    /// </summary>
    public List<string> Cameras { get; } = [];
}
