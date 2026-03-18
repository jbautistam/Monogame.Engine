namespace Bau.BauEngine.Actors;

/// <summary>
///		Clase abstracta para la definición de actores
/// </summary>
public abstract class AbstractActor : Entities.Common.Pools.IPoolable, Entities.Common.Collections.ISecureListItem
{
	protected AbstractActor(Scenes.Layers.AbstractLayer layer, int? zOrder)
	{
		// Inicializa los objetos
		Layer = layer;
		RequestedZOrder = zOrder;
		ZOrder = zOrder ?? 0;
		Transform = new Components.Transforms.TransformComponent(this);
		PreviuosTransform = new Components.Transforms.TransformComponent(this);
		// Crea los componentes
		Components = new Components.ComponentCollectionModel(this);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public void Start()
	{
		// Inicializa el actor
		StartSelf();
		// Inicializa los componentes
		Components.Start();
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected abstract void StartSelf();

	/// <summary>
	///		Actualiza el actor y sus componentes
	/// </summary>
    public void Update(Managers.GameContext gameContext)
    {
		// Guarda la transformación actual (actes de actualizar la transformación en el Update)
		PreviuosTransform = Transform.Clone();
		// Primero actualiza el actor
		UpdateSelf(gameContext);
		// y después los componentes
		Components.Update(gameContext);
    }

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected abstract void UpdateSelf(Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	public void End(Managers.GameContext gameContext)
	{
		// Desactiva el actor
		Enabled = false;
		// Detiene los componentes
		Components.End();
		// Finaliza el trabajo con el actor
		EndSelf(gameContext);
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected abstract void EndSelf(Managers.GameContext gameContext);

	/// <summary>
	///		Capa a la que se asocia el actor
	/// </summary>
	public Scenes.Layers.AbstractLayer Layer { get; }

	/// <summary>
	///		Id del elemento
	/// </summary>
	public string Id { get; set; } = Guid.NewGuid().ToString();

	/// <summary>
	///		Orden de dibujo solicitado
	/// </summary>
	public int? RequestedZOrder { get; }

	/// <summary>
	///		Orden de dibujo solicitado
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
	///		Componentes
	/// </summary>
	public Components.ComponentCollectionModel Components { get; }

	/// <summary>
	///		Etiquetas asociadas al actor
	/// </summary>
	public HashSet<string> Tags { get; } = [];
}