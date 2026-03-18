namespace Bau.BauEngine.Managers.Audio.Sources;

/// <summary>
///		Evento lanzados por el origen de sonido
/// </summary>
public struct ScheduleEvent(string name, float start)
{
	/// <summary>
	///		Nombre del evento
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Momento de inicio
	/// </summary>
	public float Start { get; } = start;
}
