namespace Bau.Libraries.BauGame.Engine.Managers.Audio.EventArguments;

/// <summary>
///		Argumentos de evento de sincronización
/// </summary>
public class AudioSyncEventArgs(string name) : EventArgs
{
	/// <summary>
	///		Nombre del evento de sincronización
	/// </summary>
	public string Name { get; } = name;
}
