using Microsoft.Xna.Framework;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///     Control de texto para una etiqueta
/// </summary>
public class UiLabel(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public enum HorizontalAlignmentType
    {
        /// <summary>Sin alineación</summary>
        None,
        /// <summary>A la izquierda</summary>
        Left,
        /// <summary>Centrada</summary>
        Center,
        /// <summary>A la derecha</summary>
        Right,
        /// <summary>Justificada al tamaño</summary>
        Stretch
    }
    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public enum VerticalAlignmentType
    {
        /// <summary>Sin alineación</summary>
        None,
        /// <summary>En la parte superior</summary>
        Top,
        /// <summary>Centrada</summary>
        Center,
        /// <summary>En la parte inferior</summary>
        Bottom,
        /// <summary>Justificada al tamaño</summary>
        Stretch
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
    {
        if (AutoSize && Font is not null && !string.IsNullOrEmpty(Text))
        {
            Vector2 textSize = Font.MeasureString(Text);

                // Si no está ajustado, se calculan los límites
                if (Position.Dock == UiPosition.DockType.None)
                {
                    Position.Bounds = new Rectangle(Position.Bounds.X, Position.Bounds.Y, (int) textSize.X, (int) textSize.Y);
                    Position.ContentBounds = new Rectangle(Position.Bounds.X, Position.Bounds.Y, Position.Bounds.Width, Position.Bounds.Height);
                    if (Position.Padding is not null)
                        Position.ContentBounds = (Position.Padding ?? new UiMargin(0)).Apply(Position.ContentBounds);
                }
        }
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext) 
    {
        Font?.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        // Dibuja el estilo
        Layer.DrawStyle(renderingManager, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
        // Dibuja el texto
        if (!string.IsNullOrEmpty(Text) && Font is not null)
        {
            if (WrapText)
                DrawWrappedText(renderingManager, Text, Font);
            else
                DrawSimpleText(renderingManager, Text, Font);
        }
    }

    /// <summary>
    ///     Dibuja el texto en una línea
    /// </summary>
    private void DrawSimpleText(Scenes.Rendering.RenderingManager renderingManager, string text, SpriteTextDefinition font)
    {
        Vector2 textSize = font.MeasureString(text);
        Vector2 textPosition = new(Position.ContentBounds.X, Position.ContentBounds.Y);
        Styles.UiStyle style = Layer.Styles.GetDefault(Style);

            // Ajustar alineación horizontal
            switch (HorizontalAlignment)
            {
                case HorizontalAlignmentType.Left:
                        textPosition.X = Position.ContentBounds.X;
                    break;
                case HorizontalAlignmentType.Right:
                        textPosition.X = Position.ContentBounds.Right - textSize.X;
                    break;
                case HorizontalAlignmentType.Center:
                        textPosition.X = Position.ContentBounds.X + (Position.ContentBounds.Width - textSize.X) / 2;
                    break;
            }
            // Ajustar alineación vertical
            switch (VerticalAlignment)
            {
                case VerticalAlignmentType.Top:
                        textPosition.Y = Position.ContentBounds.Y;
                    break;
                case VerticalAlignmentType.Bottom:
                        textPosition.Y = Position.ContentBounds.Bottom - textSize.Y;
                    break;
                case VerticalAlignmentType.Center:
                        textPosition.Y = Position.ContentBounds.Y + (Position.ContentBounds.Height - textSize.Y) / 2;
                    break;
            }
            // Dibuja el texto
            renderingManager.SpriteTextRenderer.DrawString(font, text, textPosition, (style.StyleText?.Color ?? Color.White) * (style.StyleText?.Opacity ?? 1));
    }

    /// <summary>
    ///     Dibuja el texto en varias líneas
    /// </summary>
    private void DrawWrappedText(Scenes.Rendering.RenderingManager renderingManager, string text, SpriteTextDefinition font)
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
                                renderingManager.SpriteTextRenderer.DrawString(font, line, linePosition, (style.StyleText?.Color ?? Color.White) * (style.StyleText?.Opacity ?? 1));
                                // Pasa a la siguiente palabra
                                line = word;
                                // Incrementa la posición y
                                yPosition += font.GetLineSpacing();
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

                    renderingManager.SpriteTextRenderer.DrawString(font, line, linePosition, (style.StyleText?.Color ?? Color.White) * (style.StyleText?.Opacity ?? 1));
            }

        // Comprueba si la coordenada y está fuera de los límites
        bool IsOutLimits(float y) => y > Position.ContentBounds.Bottom - font.GetLineSpacing();
    }

    /// <summary>
    ///     Texto
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public HorizontalAlignmentType HorizontalAlignment { get; set; } = HorizontalAlignmentType.Left;

    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public VerticalAlignmentType VerticalAlignment { get; set; } = VerticalAlignmentType.Top;

    /// <summary>
    ///     Datos del texto
    /// </summary>
    public SpriteTextDefinition? Font { get; set; }

    /// <summary>
    ///     Indica si se deben ajustar los tamaños
    /// </summary>
    public bool AutoSize { get; set; }

    /// <summary>
    ///     Indica si se debe partir el texto
    /// </summary>
    public bool WrapText { get; set; }
}