using Microsoft.Xna.Framework;

namespace UI.Components.TypeWriter;

/// <summary>
///		Comando de la máquina de escribir con un texto
/// </summary>
internal class CommandTextTypeWriter : CommandAbstractLine
{
	/// <summary>
	///		Texto que se va a escribir
	/// </summary>
	internal required string Text { get; init; }

	/// <summary>
	///		Color con el que se va a escribir
	/// </summary>
	internal required Color? Color { get; init; }

	/// <summary>
	///		Indica si el texto se va a escribir en negrita
	/// </summary>
	internal required bool Bold { get; init; }

	/// <summary>
	///		Indica si el texto se va a escribir en cursiva
	/// </summary>
	internal required bool Italic { get; init; }
}