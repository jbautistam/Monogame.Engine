using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

/// <summary>
///		Parametros de un texto
/// </summary>
public class SpriteTextParameters
{
	// Variables pivadas
    private SpriteTextDefinition? _lastFont;
	private string? _text;
	private Rectangle _bounds;
	private List<string> _lines = [];
	private bool _isDirty = true;

    /// <summary>
    ///     Divide las líneas
    /// </summary>
    public List<string> GetLines(SpriteTextDefinition font)
    {
        // Si algo ha cambiado desde la última vez, separa las líneas
        if (_lastFont is null || _lastFont != font || _isDirty)
        {
            // Separa las líneas
            _lines = WrapText(font, Bounds.Width);
            // Indica que no ha habido cambiios
            _lastFont = font;
            _isDirty = false;
        }
        // Devuelve la lista de líneas
        return _lines;
    }

    /// <summary>
    ///     Divide el texto de un mensaje para que quepa en el ancho
    /// </summary>
    private List<string> WrapText(SpriteTextDefinition font, int width)
    {
        List<string> lines = [];

            // Si realmente hay un texto
            if (!string.IsNullOrWhiteSpace(Text))
            {
                string currentLine = string.Empty;
            
                    // Simula la escritura de las palabras y las añade a la lista de líneas
                    foreach (string word in Text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                
                            // Si se ha superado el ancho, se pasa a la siguiente línea
                            if (font.MeasureString(testLine).X > width)
                            {
                                // Añade la línea que llevamos hasta ahora
                                lines.Add(currentLine);
                                // Dejamos la siguiente palabra en la línea actual
                                currentLine = word;
                            }
                            else
                                currentLine = testLine;
                    }
                    // Añade el resto de línea
                    if (!string.IsNullOrEmpty(currentLine))
                        lines.Add(currentLine);
               }
               // Devuelve la lista de líneas
               return lines;
    }

    /// <summary>
    ///     Obtiene la altura total de las líneas
    /// </summary>
    public float GetHeight(SpriteTextDefinition font)
    {
        List<string> lines = GetLines(font);
        float contentHeight = 0f;

            // Calcula el tamaño de las líneas            
            foreach (string line in lines)
            {
                Vector2 size = font.MeasureString(line);

                    if (contentHeight < Bounds.Width)
                        contentHeight += size.Y + font.GetLineSpacing();
            }
            // Devuelve la altura
            return contentHeight;
    }

	/// <summary>
	///		Texto de los parámetros
	/// </summary>
	public string? Text 
	{ 
		get { return _text; }
		set { _isDirty = Tools.UpdatePropertyFunctions.ChangeProperty(ref _text, value); }
	}

	/// <summary>
	///		Límites del texto
	/// </summary>
	public Rectangle Bounds
	{
		get { return _bounds; }
		set { _isDirty = Tools.UpdatePropertyFunctions.ChangeProperty(ref _bounds, value); }
	}

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public UserInterface.UiLabel.HorizontalAlignmentType HorizontalAlignment { get; set; } = UserInterface.UiLabel.HorizontalAlignmentType.Left;

    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public UserInterface.UiLabel.VerticalAlignmentType VerticalAlignment { get; set; } = UserInterface.UiLabel.VerticalAlignmentType.Top;
}
