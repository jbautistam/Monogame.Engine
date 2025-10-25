namespace Bau.Libraries.BauGame.Engine.Actors;

/// <summary>
///		Clase abstracta para la definición de actores
/// </summary>
public abstract class AbstractActor : Pool.IPoolable
{
	protected AbstractActor(Scenes.Layers.AbstractLayer layer, int zOrder)
	{
		Layer = layer;
		ZOrder = zOrder;
		Transform = new Components.Transforms.TransformComponent(this);
		PreviuosTransform = new Components.Transforms.TransformComponent(this);
		Renderer = new Components.Renderers.RendererComponent(this);
		Components = new Components.ComponentCollectionModel(this);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public abstract void Start();

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
    public void UpdatePhysics(Managers.GameContext gameContext)
    {
		// Actualiza el tamaño del actor a partir del tamaño de la textura
		Transform.Bounds.Resize(Renderer.GetSize());
		// Actualiza las físicas
		foreach (Components.AbstractComponent component in Components)
			component.UpdatePhysics(gameContext);
    }

	/// <summary>
	///		Actualiza el actor y sus componentes
	/// </summary>
    public void Update(Managers.GameContext gameContext)
    {
		// Guarda la transformación actual (actes de actualizar la transformación en el Update)
		PreviuosTransform = Transform.Clone();
		// Primero actualiza el actor
		UpdateActor(gameContext);
		// y después los componentes
		Renderer.Update(gameContext);
		Components.Update(gameContext);
    }

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected abstract void UpdateActor(Managers.GameContext gameContext);
    
	/// <summary>
	///		Dibuja el actor y los componentes
	/// </summary>
    public void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
		// Dibuja la textura y los componentes
		Renderer.Draw(camera, gameContext);
		Components.Draw(camera, gameContext);
		// Llama al actor para que se dibuje si es necesario
		DrawActor(camera, gameContext);
    }

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected abstract void DrawActor(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	public void End()
	{
		// Desactiva el actor
		Enabled = false;
		// Detiene los componentes
		foreach (Components.AbstractComponent component in Components)
			component.End();
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected abstract void EndActor();

	/// <summary>
	///		Capa a la que se asocia el actor
	/// </summary>
	public Scenes.Layers.AbstractLayer Layer { get; }

	/// <summary>
	///		Orden de dibujo
	/// </summary>
	public int ZOrder { get; set; }

	/// <summary>
	///		Indica si el actor está activo
	/// </summary>
	public bool Enabled { get; set; } = true;

	/// <summary>
	///		Datos de posición
	/// </summary>
	public Components.Transforms.TransformComponent PreviuosTransform { get; private set; }

	/// <summary>
	///		Datos de posición
	/// </summary>
	public Components.Transforms.TransformComponent Transform { get; }

	/// <summary>
	///		Objeto de representación
	/// </summary>
	public Components.Renderers.RendererComponent Renderer { get; }

	/// <summary>
	///		Componentes
	/// </summary>
	public Components.ComponentCollectionModel Components { get; }

	/// <summary>
	///		Etiquetas asociadas al actor
	/// </summary>
	public HashSet<string> Tags { get; } = [];
}