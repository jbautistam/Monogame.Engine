using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter.TextItems;

/// <summary>
///		Generador de comandos para la máquina de escribir
/// </summary>
internal class CommandTypeWriterGenerator
{
	/// <summary>
	///		Tipos de token
	/// </summary>
	private enum TokenType
	{
		/// <summary>Cambio de color</summary>
		Color,
		/// <summary>Texto</summary>
		Text,
		/// <summary>Comando de espera</summary>
		Wait,
		/// <summary>Comando de cambio de velocidad</summary>
		Speed,
		/// <summary>Comando para lanzar un evento</summary>
		Event,
		/// <summary>Indica si se debe poner el texto en negrita hasta este momento</summary>
		Bold,
		/// <summary>Indica si se debe poner el texto en cursiva en este momento</summary>
		Italic
	}
	// Registros privados
	private record TokenExtended(TokenType Type, bool IsEnd, string Text);
	// Variables privadas
	private List<Color> _colors = [];
	private bool _bold, _italic;

	/// <summary>
	///		Genera la lista de comandos
	/// </summary>
	internal List<TextSectionModel> Parse(string text)
	{
		List<StringExtractor.Token> tokens = new StringExtractor().Extract(text, '[', ']');
		List<TextSectionModel> texts = [];

			// Recorre los tokens
			foreach (StringExtractor.Token token in tokens)
				if (token.IsCode)
				{
					TokenExtended extended = ParseToken(token.Text);
					
						switch (extended.Type)
						{
							case TokenType.Color:
									if (extended.IsEnd)
									{
										if (_colors.Count > 0)
											_colors.RemoveAt(_colors.Count - 1);
									}
									else
										_colors.Add(ParseColor(extended.Text));
								break;
							case TokenType.Wait:
									CreateCommand(texts, new CommandWaitTypeWriter()
																{
																	Time = ParseFloat(extended.Text, 0.5f)
																}
												  );
								break;
							case TokenType.Speed:
									CreateCommand(texts, new CommandSpeedTypeWriter()
															{
																Speed = ParseFloat(extended.Text, 0.5f)
															}
												  );
								break;
							case TokenType.Event:
									CreateCommand(texts, new CommandEvetTypeWriter()
															{
																Data = extended.Text
															}
												  );
								break;
							case TokenType.Bold:
									_bold = !extended.IsEnd;
								break;
							case TokenType.Italic:
									_italic = !extended.IsEnd;
								break;
						}
				}
				else
					texts.Add(CreateTextCommand(token.Text));
			// Devuelve la lista de comandos
			return texts;
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void CreateCommand(List<TextSectionModel> texts, CommandAbstractLine command)
	{
		// Crea un texto vacío
		if (texts.Count == 0)
			texts.Add(new TextSectionModel
								{
									Text = string.Empty,
									Bold = false,
									Italic = false,
									Color = Color.White,
								}
					  );
		// Añade el comando al último texto creado
		texts[texts.Count - 1].Commands.Add(command);
	}

	/// <summary>
	///		Interpreta un valor de texto decimal
	/// </summary>
	private float ParseFloat(string text, float defaultValue)
	{
		if (!string.IsNullOrWhiteSpace(text) && float.TryParse(text, System.Globalization.CultureInfo.InvariantCulture, out float value))
			return value;
		else
			return defaultValue;
	}

	/// <summary>
	///		Interpreta un color
	/// </summary>
	private Color ParseColor(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
            if (text.StartsWith("#") && (text.Length == 7 || text.Length == 9))
                try
                {
                    byte a = text.Length == 9 ? Convert.ToByte(text.Substring(1, 2), 16) : (byte) 255;
                    int start = text.Length == 9 ? 3 : 1;
                    byte r = Convert.ToByte(text.Substring(start, 2), 16);
                    byte g = Convert.ToByte(text.Substring(start + 2, 2), 16);
                    byte b = Convert.ToByte(text.Substring(start + 4, 2), 16);

						// Devuelve el color
						return new Color(r, g, b, a);
                }
                catch 
				{ 
					return Color.Black; 
				}
			else
				return text.ToLower() switch
								{
									"red" => Color.Red,
									"green" => Color.Green,
									"blue" => Color.Blue,
									"yellow" => Color.Yellow,
									"white" => Color.White,
									"black" => Color.Black,
									"gray" or "grey" => Color.Gray,
									"cyan" => Color.Cyan,
									"magenta" => Color.Magenta,
									"orange" => Color.Orange,
									"purple" => Color.Purple,
									_ => Color.Black
								};
		}
		else
			return Color.Black;
	}


	/// <summary>
	///		Interpreta un token
	/// </summary>
	private TokenExtended ParseToken(string content)
	{
		bool isEndToken = false;
		TokenType type = TokenType.Text;

			// Interpreta el token
			if (!string.IsNullOrWhiteSpace(content))
			{
				// Quita los espacios
				content = content.Trim();
				// Comprueba si es un inicio o un fin
				if (content.StartsWith("/"))
				{
					// Indica que es un final de token
					isEndToken = true;
					// Quita la barra inicial del texto
					if (content.Length > 1)
						content = content.Substring(1);
					else
						content = string.Empty;
				}
				// Extrae el nombre del token y obtiene el tipo
				(type, content) = ExtractName(content);
				// Obtiene el tipo del token
			}
			// Devuelve la interpretación
			return new TokenExtended(type, isEndToken, content);
	}

	/// <summary>
	///		Extrae el token del texto
	/// </summary>
	private (TokenType type, string part) ExtractName(string text)
	{
		List<(TokenType type, string reserved)> tokens = [ 
															(TokenType.Color, "color"), 
															(TokenType.Color, "c"), 
															(TokenType.Bold, "bold"), 
															(TokenType.Bold, "b"), 
															(TokenType.Wait, "wait"), 
															(TokenType.Speed, "speed"), 
															(TokenType.Event, "event") 
														];
		string extracted = string.Empty;

			// Busca el token y lo quita del contenido
			foreach ((TokenType type, string reserved) in tokens)
				if (text.StartsWith(reserved, StringComparison.CurrentCultureIgnoreCase))
					return (type, Extract(reserved, text));
			// Si ha llegado hasta aquí es porque no ha reconocido el token
			return (TokenType.Text, text);

		// Extrae la palabra reservada del texto
		string Extract(string reserved, string text)
		{
			if (reserved.Length < text.Length)
			{
				// Quita la palabra reservada
				text = text.Substring(reserved.Length);
				// Quita el carácter =
				if (text.StartsWith("="))
				{
					if (text.Length > 1)
						text = text.Substring(1);
				}
				// Devuelve el texto
				return text;
			}
			else
				return string.Empty;
		}
	}

	/// <summary>
	///		Crea un comando de texto
	/// </summary>
	private TextSectionModel CreateTextCommand(string text)
	{
		return new TextSectionModel()
							{
								Text = text,
								Color = GetLastColor(),
								Bold = _bold,
								Italic = _italic
							};

		// Obtiene el último color de la pila
		Color? GetLastColor()
		{
			if (_colors.Count == 0)
				return null;
			else
				return _colors[_colors.Count - 1];
		}
	}
}