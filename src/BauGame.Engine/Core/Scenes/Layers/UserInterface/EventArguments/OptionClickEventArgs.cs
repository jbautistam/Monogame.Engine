namespace Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.UserInterface.EventArguments;

/// <summary>
///		Argumentos del evento de click sobre una opción
/// </summary>
public class OptionClickEventArgs(int selectedOption) : EventArgs
{
	/// <summary>
	///		Opción seleccionada
	/// </summary>
	public int SelectedOption { get; } = selectedOption;
}