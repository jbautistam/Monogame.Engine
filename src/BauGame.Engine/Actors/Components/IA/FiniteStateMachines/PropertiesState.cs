namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///		Propiedades del estado
/// </summary>
public class PropertiesState
{
	/// <summary>
	///		Grupo de animaciones
	/// </summary>
	public required string BlenderGroup { get; init; }

	/// <summary>
	///		Duración del estado si tiene un tiempo máximo (0 si es infinito)
	/// </summary>
	public float Duration { get; set; }

	/// <summary>
	///		Velocidad máxima de movimiento
	/// </summary>
	public float SpeedMaximum { get; set; } = 1.0f;

	/// <summary>
	///		Siguiente estado predefinido cuando se acaba la duración
	/// </summary>
	public string? NextState { get; set; }
}
