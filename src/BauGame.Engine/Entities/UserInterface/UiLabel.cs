using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

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
    // Variables privadas
    private bool _isInitialized;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
    {
        if (AutoSize && SpriteFont is not null && !string.IsNullOrEmpty(Text))
        {
            Vector2 textSize = SpriteFont.MeasureString(Text);

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
        if (!_isInitialized)
        {
            // Carga la fuente
            if (!string.IsNullOrWhiteSpace(Font))
                SpriteFont = Layer.Scene.LoadSceneAsset<SpriteFont>(Font);
            // Indica que ya está inicializado
            _isInitialized = true;
        }
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        // Dibuja el estilo
        Layer.DrawStyle(camera, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
        // Dibuja el texto
        if (!string.IsNullOrEmpty(Text) && SpriteFont is not null)
        {
            if (WrapText)
                DrawWrappedText(camera, Text, SpriteFont);
            else
                DrawSimpleText(camera, Text, SpriteFont);
        }
    }

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

    /// <summary>
    ///     Prepara los comandos de presentación
    /// </summary>
	public override void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, Managers.GameContext gameContext)
	{
		if (!string.IsNullOrWhiteSpace(Text))
        {
            Styles.UiStyle style = Layer.Styles.GetDefault(Style);

                // Genera los comandos de estilo
                Layer.PrepareStyleRendercommands(builder, Style, Styles.UiStyle.StyleType.Normal, Position.ContentBounds, gameContext);
                // Genera el comando del texto de la etiqueta
                builder.WithCommand(Font, LineSpacing, WrapText)
                        .WithTransform(Position.ContentBounds, Vector2.Zero)
                        .WithAlignment(HorizontalAlignment, VerticalAlignment)
                        .WithText(Text, false, false, style.Color * style.Opacity);
        }
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
    ///     Nombre de la fuente
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Fuente del texto
    /// </summary>
    private SpriteFont? SpriteFont { get; set; }

    /// <summary>
    ///     Indica si se deben ajustar los tamaños
    /// </summary>
    public bool AutoSize { get; set; }

    /// <summary>
    ///     Indica si se debe partir el texto
    /// </summary>
    public bool WrapText { get; set; }

    /// <summary>
    ///     Espaciado de líneas
    /// </summary>
    public float LineSpacing { get; set; } = 1.2f;
}