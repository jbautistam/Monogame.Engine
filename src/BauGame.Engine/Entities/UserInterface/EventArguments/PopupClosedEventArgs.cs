namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.EventArguments;

/// <summary>
///		Argumentos del evento de popup
/// </summary>
public class PopupClosedEventArgs(Popups.UiPopupManager.PopupResponse response)
{
	/// <summary>
	///		Respuesta del popup
	/// </summary>
	public Popups.UiPopupManager.PopupResponse Response { get; } = response;
}
