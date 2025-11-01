
namespace Bau.Libraries.BauGame.Engine.Actors.Components;

/// <summary>
///		Datos base de un componente
/// </summary>
public abstract class AbstractComponent(AbstractActor owner, bool isDrawable)
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
	///		Dibuja los datos del componente
	/// </summary>
	public abstract void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el trabajo con el componente
	/// </summary>
	public abstract void End();

	/// <summary>
	///		Propietario del componente
	/// </summary>
	public AbstractActor Owner { get; } = owner;

	/// <summary>
	///		Indica si el componente está activo
	/// </summary>
	public bool Enabled { get; set; } = true;

	/// <summary>
	///		Indica si el componente se puede dibujar
	/// </summary>
	public bool Drawable { get; } = isDrawable;
}
