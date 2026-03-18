
namespace Bau.BauEngine.Actors.Components;

/// <summary>
///		Datos base de un componente
/// </summary>
public abstract class AbstractComponent(AbstractActorDrawable owner)
{
	/// <summary>
	///		Inicializa el componente
	/// </summary>
	public abstract void Start();

	/// <summary>
	///		Actualiza los datos del componente para las físicas (si es necesario)
	/// </summary>
	public abstract void UpdatePhysics(Managers.GameContext gameContext);

	/// <summary>
	///		Actualiza los datos del componente
	/// </summary>
	public abstract void Update(Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el trabajo con el componente
	/// </summary>
	public abstract void End();

	/// <summary>
	///		Propietario del componente
	/// </summary>
	public AbstractActorDrawable Owner { get; } = owner;

	/// <summary>
	///		Indica si el componente está activo
	/// </summary>
	public bool Enabled { get; set; } = true;
}
