namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.EventArguments;

/// <summary>
///		Argumentos del evento de click sobre un componente
/// </summary>
public class ClickEventArgs(UiElement component, string? tag = null) : EventArgs
{
	/// <summary>
	///		Componente que lanza el evento
	/// </summary>
	public UiElement Component { get; } = component;

	/// <summary>
	///		Contenido asociado al evento
	/// </summary>
	public string? Tag { get; } = tag;
}