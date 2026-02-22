using Bau.Libraries.BauGame.Engine.Actors;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas
/// </summary>
public abstract class AbstractLayer
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

    public AbstractLayer(AbstractScene scene, string name, LayerType type, int sortOrder)
    {
        // Asigna las propiedades
        Scene = scene;
        Name = name;
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
        UpdateLayer(gameContext);
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
    protected abstract void UpdateLayer(Managers.GameContext gameContext);

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	public void Draw(Cameras.Camera2D camera, Managers.GameContext gameContext)
	{
        // Dibuja la capa
        DrawLayer(camera, gameContext);
        // Ordena los actores
        Actors.SortByZOrder();
        // Dibuja los actores
        Actors.Draw(camera, gameContext);
	}

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	protected abstract void DrawLayer(Cameras.Camera2D camera, Managers.GameContext gameContext);

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
	public AbstractScene Scene { get; }

	/// <summary>
	///		Nombre de la capa
	/// </summary>
	public string Name { get; }

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
}
