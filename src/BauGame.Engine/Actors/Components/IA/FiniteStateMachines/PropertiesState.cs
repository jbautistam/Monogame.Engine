namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///		Propiedades del estado
/// </summary>
public class PropertiesState
{
	/// <summary>
	///		Textura
	/// </summary>
	public required string Texture { get; init; }

	/// <summary>
	///		Regió dentro de la textura
	/// </summary>
	public required string Region { get; init; }

	/// <summary>
	///		Animación
	/// </summary>
	public string? Animation { get; set; }

	/// <summary>
	///		Indica si se anima en bucle
	/// </summary>
	public bool AnimationLoop { get; set; }

	/// <summary>
	///		Duración del estado si tiene un tiempo máximo (0 si es infinito)
	/// </summary>
	public float Duration { get; set; }

	/// <summary>
	///		Siguiente estado predefinido cuando se acaba la duración
	/// </summary>
	public string? NextState { get; set; }
}
