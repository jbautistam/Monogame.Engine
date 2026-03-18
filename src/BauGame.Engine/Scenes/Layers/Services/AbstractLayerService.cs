namespace Bau.BauEngine.Scenes.Layers.Services;

/// <summary>
///		Base para los servicios asociados a una capa
/// </summary>
public abstract class AbstractLayerService(string name)
{
	/// <summary>
	///		Inicializa el servicio
	/// </summary>
	public void Initialize(LayerServicesList owner)
	{
		Owner = owner;
	}

	/// <summary>
	///		Arranca el servicio
	/// </summary>
	public void Start()
	{
		// Indica que el servicio está activo
		Enabled = true;
		// Arranca el servicio
		StartService();
	}

	/// <summary>
	///		Arranca el servicio
	/// </summary>
	protected abstract void StartService();

	/// <summary>
	///		Actualiza el servicio
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
		if (Owner is not null && Enabled)
			UpdateService(gameContext);
	}

	/// <summary>
	///		Actualiza el servicio
	/// </summary>
	protected abstract void UpdateService(Managers.GameContext gameContext);

	/// <summary>
	///		Detiene el servicio
	/// </summary>
	public void Stop()
	{
		// Desactiva el servicio
		Enabled = false;
		// Marca el servicio como detenido
		StopService();
	}

	/// <summary>
	///		Detiene el servicio
	/// </summary>
	protected abstract void StopService();

	/// <summary>
	///		Nombre del servicio
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Capa a la que se asocia el servicio
	/// </summary>
	public LayerServicesList? Owner { get; private set; }

	/// <summary>
	///		Indica si el servicio está activo
	/// </summary>
	public bool Enabled { get; set; } = true;
}
