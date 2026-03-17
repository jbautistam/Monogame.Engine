using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups.MobileChats;

/// <summary>
///		Herramientas de tratamiento de texto
/// </summary>
public class StringFontHelper
{
    /// <summary>
    ///     Divide el texto de un mensaje para que quepa en el ancho
    /// </summary>
    public List<string> WrapText(SpriteTextDefinition font, string text, float maxLineWidth)
    {
        List<string> lines = [];

            // Si realmente hay un texto
            if (!string.IsNullOrWhiteSpace(text))
            {
                string currentLine = string.Empty;
            
                    // Simula la escritura de las palabras y las añade a la lista de líneas
                    foreach (string word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                
                            // Si se ha superado el ancho, se pasa a la siguiente línea
                            if (font.MeasureString(testLine).X > maxLineWidth)
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
}
