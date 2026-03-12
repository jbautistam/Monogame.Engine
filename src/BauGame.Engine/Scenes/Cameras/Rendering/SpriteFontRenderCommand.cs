using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Comando para dibujar un texto
/// </summary>
public class SpriteFontRenderCommand : AbstractRenderCommand
{
    // Registros privados
    private record LineInfo(string Line, Vector2 Size);

    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public override void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
        if (Texts.Count > 0)
        {
            SpriteFont? spriteFont = GetFont(director, Font);

                if (spriteFont is not null)
                {
                    if (WrapText)
                    {
                        List<LineInfo> linesInfo = GetLinesSize(spriteFont);

                            // Dibuja el texto dividido en líneas
                            DrawText(spriteBatch, spriteFont, GetStartY(spriteFont, linesInfo), linesInfo);
                    }
                    else
                        DrawFullText(spriteBatch, spriteFont);
                }
        }
    }

    /// <summary>
    ///     Dibuja las líneas de texto (lines info sólo se utiliza para el centrado horizontal)
    /// </summary>
    private void DrawText(SpriteBatch spriteBatch, SpriteFont spriteFont, float y, List<LineInfo> linesInfo)
    {
        int actualLine = 0;
        float x = GetStartX(GetLineInfoWidth(actualLine));
        string testLine = string.Empty;

            // Dibuja las líneas
            foreach (SpriteFontTextRenderCommand text in Texts)
                if (!string.IsNullOrWhiteSpace(text.Text))
                    foreach (string word in text.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        Vector2 textSize = spriteFont.MeasureString(AddWord(testLine, word));

                            // Si la palabra cabe en la línea, la añadimos a la línea de prueba de texto e incrementamos la X
                            if (!IsOutOfLine(x, textSize.X))
                            {
                                testLine = AddWord(testLine, word);
                                x += spriteFont.MeasureString(word + " ").X;
                            }
                            else // ... si estamos fuera de la línea
                            {
                                // Escribe el texto
                                DrawText(spriteBatch, spriteFont, x, y, testLine, text.Bold, text.Italic, text.Color);
                                // Vacía la línea y vuelve a la izquierda
                                testLine = word;
                                x = GetStartX(GetLineInfoWidth(++actualLine));
                                y += textSize.Y + spriteFont.LineSpacing * LineSpacing;
                            }
                            //TODO: debería comprobar que no se salta el alto de la página
                    }
            // Dibuja la última línea
            if (!string.IsNullOrWhiteSpace(testLine))
            {
                SpriteFontTextRenderCommand text = Texts[Texts.Count - 1];

                    DrawText(spriteBatch, spriteFont, GetStartX(GetLineInfoWidth(++actualLine)), y, testLine, text.Bold, text.Italic, text.Color);
            }

            // Obtiene el ancho de una línea (comprobando que esté en los límites)
            float GetLineInfoWidth(int line)
            {
                if (line > 0 && line < linesInfo.Count)
                    return linesInfo[line].Size.X;
                else if (linesInfo.Count > 0)
                    return linesInfo[linesInfo.Count - 1].Size.X;
                else
                    return 0;
            }
    }

    /// <summary>
    ///     Dibuja el texto completo sin dividir por líneas
    /// </summary>
    private void DrawFullText(SpriteBatch spriteBatch, SpriteFont spriteFont)
    {
        float y = Transform.Destination.Top;

            for (int line = 0; line < Texts.Count; line++)
            {
                Vector2 textSize = spriteFont.MeasureString(Texts[line].Text);
                float x = GetStartX(textSize.X);

                    // Escribe la línea de texto
                    DrawText(spriteBatch, spriteFont, x, y, Texts[line].Text, Texts[line].Bold, Texts[line].Italic, Texts[line].Color);
                    // Pasa a la siguiente línea
                    y += textSize.Y + spriteFont.LineSpacing * LineSpacing;
            }
    }

    /// <summary>
    ///     Dibuja una línea de texto
    /// </summary>
    private void DrawText(SpriteBatch spriteBatch, SpriteFont spriteFont, float x, float y, string text, bool bold, bool italic, Color color)
    {
        if (!string.IsNullOrWhiteSpace(text))
            spriteBatch.DrawString(spriteFont, text, new Vector2(x, y), color);
    }

    /// <summary>
    ///     Obtiene la coordenada X de inicio de la línea
    /// </summary>
    private float GetStartX(float lineWidth)
    {
        float x = Transform.Destination.X;

            // Calcula la X dependiendo de la alineación horizontal
            switch (HorizontalAlignment)
            {
                case UiLabel.HorizontalAlignmentType.Center:
                case UiLabel.HorizontalAlignmentType.Stretch:
                        x = Transform.Destination.X + 0.5f * (Transform.Destination.Width - lineWidth);
                    break;
                case UiLabel.HorizontalAlignmentType.Right:
                        x = Transform.Destination.Right - lineWidth;
                    break;
            }
            // Devuelve la X
            return x;
    }

    /// <summary>
    ///     Obtiene la coordenada Y de inicio
    /// </summary>
    private float GetStartY(SpriteFont spriteFont, List<LineInfo> linesInfo)
    {
        if (VerticalAlignment == UiLabel.VerticalAlignmentType.None || VerticalAlignment == UiLabel.VerticalAlignmentType.Top)
            return Transform.Destination.Y;
        else
        {
            float height = ComputeHeight(spriteFont, linesInfo);

                switch (VerticalAlignment)
                {
                    case UiLabel.VerticalAlignmentType.Bottom:
                        return Transform.Destination.Height - height;
                    default: // Stretch / centet
                        return Transform.Destination.Height - 0.5f * height;
                }
        }
    }

    /// <summary>
    ///     Calcula la altura máxima de dibujo
    /// </summary>
    private float ComputeHeight(SpriteFont spriteFont, List<LineInfo> linesInfo)
    {
        float height = 0;

            // Calcula la altura
            foreach (LineInfo lineInfo in linesInfo)
                height += lineInfo.Size.Y + spriteFont.LineSpacing * LineSpacing;
            // Devuelve la altura
            return height;
    }

    /// <summary>
    ///     Obtiene las líneas de texto y sus tamaños
    /// </summary>
    private List<LineInfo> GetLinesSize(SpriteFont spriteFont)
    {
        List<LineInfo> linesInfo = [];
        int x = Transform.Destination.Left;
        string testLine = string.Empty;

            // Divide el texto en líneas
            foreach (SpriteFontTextRenderCommand text in Texts)
                if (!string.IsNullOrWhiteSpace(text.Text))
                    foreach (string word in text.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        Vector2 textSize = spriteFont.MeasureString(AddWord(testLine, word));

                            // Comprobamos si la palabra cabe en la línea
                            if (!IsOutOfLine(x, textSize.X))
                                testLine = AddWord(testLine, word);
                            else // ... si estamos fuera de la línea
                            {
                                // Añade el texto a la lista de líneas
                                if (!string.IsNullOrWhiteSpace(testLine))
                                    linesInfo.Add(new LineInfo(testLine, spriteFont.MeasureString(testLine)));
                                // Vacía la línea y vuelve a la izquierda
                                testLine = word;
                                x = Transform.Destination.Left;
                            }
                    }
            // Si queda algo en la línea de pruebas, lo añade a la lista
            if (!string.IsNullOrWhiteSpace(testLine))
                linesInfo.Add(new LineInfo(testLine, spriteFont.MeasureString(testLine)));
            // Devuelve la altura
            return linesInfo;
    }

    /// <summary>
    ///     Añade una palabra a la línea
    /// </summary>
    private string AddWord(string line, string word)
    {
        if (string.IsNullOrWhiteSpace(line))
            return word;
        else
            return line + " " + word;
    }

    /// <summary>
    ///     Comprueba si se supera el ancho designado para el texto
    /// </summary>
    private bool IsOutOfLine(float x, float length) => x + length >= Transform.Destination.Width;

/*
    /// <summary>
    ///     Dibuja el texto en una línea
    /// </summary>
    private void DrawSimpleText(Camera2D camera, string text, SpriteFont font)
    {
        Vector2 textSize = font.MeasureString(text);
        Vector2 textPosition = new(Position.ContentBounds.X, Position.ContentBounds.Y);
        Styles.UiStyle style = Layer.Styles.GetDefault(Style);

            // Ajustar alineación horizontal
            switch (HorizontalAlignment)
            {
                case UiLabel.HorizontalAlignmentType.Left:
                        textPosition.X = Position.ContentBounds.X;
                    break;
                case UiLabel.HorizontalAlignmentType.Right:
                        textPosition.X = Position.ContentBounds.Right - textSize.X;
                    break;
                case UiLabel.HorizontalAlignmentType.Center:
                        textPosition.X = Position.ContentBounds.X + (Position.ContentBounds.Width - textSize.X) / 2;
                    break;
            }
            // Ajustar alineación vertical
            switch (VerticalAlignment)
            {
                case UiLabel.VerticalAlignmentType.Top:
                        textPosition.Y = Position.ContentBounds.Y;
                    break;
                case UiLabel.VerticalAlignmentType.Bottom:
                        textPosition.Y = Position.ContentBounds.Bottom - textSize.Y;
                    break;
                case UiLabel.VerticalAlignmentType.Center:
                        textPosition.Y = Position.ContentBounds.Y + (Position.ContentBounds.Height - textSize.Y) / 2;
                    break;
            }
            // Dibuja el texto
            camera.SpriteBatchController.DrawString(font, text, textPosition, style.Color * style.Opacity);
    }

    /// <summary>
    ///     Dibuja el texto en varias líneas
    /// </summary>
    private void DrawWrappedText(Camera2D camera, string text, SpriteFont font)
    {
        string[] words = text.Split(' ');
        string line = "";
        float yPosition = Position.ContentBounds.Y;
        Styles.UiStyle style = Layer.Styles.GetDefault(Style);
        bool end = false;

            // Dibuja las palabras
            foreach (string word in words)
                if (!end)
                {
                    string testLine = string.IsNullOrEmpty(line) ? word : line + " " + word;
                    Vector2 textSize = font.MeasureString(testLine);

                        // Si no cabe en la misma línea, pasa a la siguiente
                        if (textSize.X > Position.ContentBounds.Width && !string.IsNullOrEmpty(line))
                        {
                            Vector2 linePosition = new(Position.ContentBounds.X, yPosition);

                                // Dibuja la línea actual y empieza una nueva
                                camera.SpriteBatchController.DrawString(font, line, linePosition, style.Color * style.Opacity);
                                // Pasa a la siguiente palabra
                                line = word;
                                // Incrementa la posición y
                                yPosition += font.LineSpacing * LineSpacing;
                        }
                        else
                            line = testLine;
                        // Verifica el límite vertical
                        if (IsOutLimits(yPosition))
                            end = true;
                }
            // Dibuja la última línea
            if (!string.IsNullOrEmpty(line) && !IsOutLimits(yPosition))
            {
                Vector2 linePosition = new(Position.ContentBounds.X, yPosition);

                    camera.SpriteBatchController.DrawString(font, line, linePosition, style.Color * style.Opacity);
            }

        // Comprueba si la coordenada y está fuera de los límites
        bool IsOutLimits(float y) => y > Position.ContentBounds.Bottom - font.LineSpacing;
    }
*/

    /// <summary>
    ///     Obtiene la fuente
    /// </summary>
    private SpriteFont? GetFont(CameraDirector director, string? font)
    {
        SpriteFont? spriteFont = null;
        
            // Carga la fuente especificada
            if (!string.IsNullOrWhiteSpace(font))
                spriteFont = director.Scene.LoadSceneAsset<SpriteFont>(font);
            // Si no ha encontrado ninguna fuente, carga la fuente predeterminada
            if (spriteFont is null)
                spriteFont = director.Scene.LoadSceneAsset<SpriteFont>(GameEngine.Instance.EngineSettings.DefaultFont);
            // Devuelve la fuente
            return spriteFont;
    }

    /// <summary>
    ///     Nombre de la fuente (si está vacío se utiliza la fuente predeterminada)
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Indica si divide el texto
    /// </summary>
    public bool WrapText { get; set; }

    /// <summary>
    ///     Espaciado entre líneas
    /// </summary>
    public float LineSpacing { get; set; }

    /// <summary>
    ///     Transformación
    /// </summary>
    public TransformRenderModel Transform { get; } = new();

    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public UiLabel.HorizontalAlignmentType HorizontalAlignment { get; set; } = UiLabel.HorizontalAlignmentType.Left;

    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public UiLabel.VerticalAlignmentType VerticalAlignment { get; set; } = UiLabel.VerticalAlignmentType.Top;

    /// <summary>
    ///     Textos que se deben presentar
    /// </summary>
    public List<SpriteFontTextRenderCommand> Texts { get; } = [];
}
