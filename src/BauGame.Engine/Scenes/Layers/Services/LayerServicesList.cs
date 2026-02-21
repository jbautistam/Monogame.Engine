namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Services;

/// <summary>
///		Colección de servicios de una capa
/// </summary>
public class LayerServicesList(AbstractLayer layer)
{
	// Servicios a añadir
	private List<AbstractLayerService> _servicesToAdd = [];

	/// <summary>
	///		Añade un servicio
	/// </summary>
	public AbstractLayerService Add(AbstractLayerService service)
	{
		// Inicializa el servicio
		service.Initialize(this);
		// Añade el servicio a la lista
		Services.Add(service.Name, service);
		// Devuelve el servicio
		return service;
	}

	/// <summary>
	///		Actualiza los servicios
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
		// Añade los servicios pendientes
		AddPendingServices();
		// Actualiza los servicios
		foreach (KeyValuePair<string, AbstractLayerService> keyValue in Services.Items)
			if (keyValue.Value.Enabled)
				keyValue.Value.Update(gameContext);
	}

	/// <summary>
	///		Añade los servicios pendientes
	/// </summary>
	private void AddPendingServices()
	{
		// Añade los servicios pendientes
		foreach (AbstractLayerService service in _servicesToAdd)
		{
			// Inicializa el servicio
			service.Initialize(this);
			// Lo añade a la lista
			Services.Add(service.Name, service);
		}
		// Limpia la lista de servicios pendientes
		_servicesToAdd.Clear();
	}

	/// <summary>
	///		Arranca un servicio
	/// </summary>
	public void Start(string name)
	{
		AbstractLayerService? service = Get(name);

			if (service is not null)
				service.Start();
	}

	/// <summary>
	///		Detiene un servicio
	/// </summary>
	public void Stop(string name)
	{
		AbstractLayerService? service = Get(name);

			if (service is not null)
				service.Stop();
	}

	/// <summary>
	///		Obtiene un servicio
	/// </summary>
	public AbstractLayerService? Get(string name) => Services.Get(name);

	/// <summary>
	///		Capa a la que se asocia la colección
	/// </summary>
	public AbstractLayer Layer { get; } = layer;

	/// <summary>
	///		Lista de servicios
	/// </summary>
	private Entities.Common.DictionaryModel<AbstractLayerService> Services { get; } = new();
}