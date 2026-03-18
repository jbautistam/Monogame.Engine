namespace Bau.BauEngine.Entities.UserInterface.TypeWriter.TextItems;

/// <summary>
///		Extrae partes de una cadena
/// </summary>
internal class StringExtractor
{
    // Definición de token
    internal record Token(bool IsCode, string Text);
    // Variables privadas
    private List<Token> _tokens = [];
    private string _lastText = string.Empty;

    /// <summary>
    ///     Interpreta una cadena con colores, por ejemplo:
    ///     Esto es [red]texto en rojo[/red] y esto es [blue]texto azul[/blue]
    /// </summary>
    internal List<Token> Extract(string text, char start, char end)
    {
        // Añade las cadenas
        if (!string.IsNullOrWhiteSpace(text))
        {
            bool isInner = false;

                // Añade los caracteres
                foreach (char chr in text)
                    if (isInner)
                    {
                        if (chr == end)
                        {
                            AddToken(true);
                            isInner = false;
                        }
                        else
                            _lastText += chr;
                    }
                    else if (chr == start)
                    {
                        AddToken(false);
                        isInner = true;
                    }
                    else
                        _lastText += chr;
                // Añade la última cadena
                AddToken(isInner);
        }
        // Devuelve las cadenas separadas
        return _tokens;
    }

    /// <summary>
    ///     Añade el token y vacía el mensaje interno
    /// </summary>
    private void AddToken(bool isCode)
    {
        if (!string.IsNullOrWhiteSpace(_lastText))
        {
            // Quita los saltos de línea \r
            _lastText = _lastText.Replace("\r\n", "\n");
            // Si es un token, no debería tener espacios
            if (isCode)
                _lastText = _lastText.Trim();
            // Añade el token
            _tokens.Add(new Token(isCode, _lastText));
            // Vacía el buffer de cadena
            _lastText = string.Empty;
        }
    }
}