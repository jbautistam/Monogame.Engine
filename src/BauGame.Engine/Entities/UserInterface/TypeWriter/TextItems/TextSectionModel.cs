using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter.TextItems;

/// <summary>
///		Comando de la máquina de escribir con un texto
/// </summary>
internal class TextSectionModel
{
	/// <summary>
	///		Actualiza la sección
	/// </summary>
	internal void Update(UiTypeWriterLabel.WriteMode mode)
	{
		if (!Completed)
		{
			// Actualiza el texto
			switch (mode)
			{
				case UiTypeWriterLabel.WriteMode.Full:
						ShowText = Text;
					break;
				case UiTypeWriterLabel.WriteMode.Characters:
						AddNextCharacter();
					break;
				case UiTypeWriterLabel.WriteMode.Words:
						AddNextWord();
					break;
			}
			// Indica que se acaba de terminar
			JustEnd = Completed;
		}
		else
			JustEnd = false;
	}

	/// <summary>
	///		Añade el siguiente carácter
	/// </summary>
	private void AddNextCharacter()
	{
		if (ShowText.Length < Text.Length)
			ShowText += Text[ShowText.Length];
	}

	/// <summary>
	///		Añade la siguiente palabra
	/// </summary>
	private void AddNextWord()
	{
		bool end = false;

			// Añade caracteres hasta encontrar un espacio o un salto de línea
			while (ShowText.Length < Text.Length && !end)
			{
				char chr = Text[ShowText.Length];

					// Quita los tabuladores
					if (chr == '\t')
						chr = ' ';
					// Añade el carácter
					ShowText += chr;
					// Si es un salto de línea o un espacio, termina
					if (chr == '\r' || chr == '\n' || chr == ' ')
						end = true;
			}
	}

	/// <summary>
	///		Texto que se va a escribir
	/// </summary>
	internal required string Text { get; init; }

	/// <summary>
	///		Texto que se debe mostrar
	/// </summary>
	internal string ShowText { get; private set; } = string.Empty;

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

	/// <summary>
	///		Indica si se deben ejecutar los comandos
	/// </summary>
	internal bool MustExecuteCommands { get; set; }

	/// <summary>
	///		Indica si se ha terminado la estructura
	/// </summary>
	internal bool Completed => ShowText.Length == Text.Length;

	/// <summary>
	///		Indica si acaba de terminar
	/// </summary>
	internal bool JustEnd { get; private set; }

	/// <summary>
	///		Comandos que se deben ejecutar al terminar con el texto
	/// </summary>
	internal List<CommandAbstractLine> Commands { get; } = [];
}