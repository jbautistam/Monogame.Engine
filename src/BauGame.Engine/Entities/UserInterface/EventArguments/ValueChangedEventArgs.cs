namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.EventArguments;

/// <summary>
///		Argumentos de los eventos de modificación de un valor
/// </summary>
public class ValueChangedEventArgs(UiElement component, object? previousValue, object? value) : EventArgs
{
	/// <summary>
	///		Componente que lanza el comando
	/// </summary>
	public UiElement Component { get; } = component;

	/// <summary>
	///		Valor anterior
	/// </summary>
	public object? PreviousValue { get; } = previousValue;

	/// <summary>
	///		Valor actual
	/// </summary>
	public object? Value { get; } = value;
}