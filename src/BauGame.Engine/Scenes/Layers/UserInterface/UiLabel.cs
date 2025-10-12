using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///     Control de texto para una etiqueta
/// </summary>
public class UiLabel(UserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public enum HorizontalAlignmentType
    {
        None,
        Left,
        Center,
        Right,
        Stretch
    }
    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public enum VerticalAlignmentType
    {
        None,
        Top,
        Center,
        Bottom,
        Stretch
    }
    // Variables privadas
    private bool _isInitialized;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds()
    {
        if (AutoSize && SpriteFont != null && !string.IsNullOrEmpty(Text))
        {
            Vector2 textSize = SpriteFont.MeasureString(Text);

                // Si no está ajustado, se calculan los límites
                if (Position.Dock == UiPosition.DockType.None)
                {
                    Position.ScreenBounds = new Rectangle(Position.ScreenBounds.X, Position.ScreenBounds.Y, (int) textSize.X, (int) textSize.Y);
                    Position.ScreenPaddedBounds = new Rectangle(Position.ScreenBounds.X + (int) Position.Padding.Left,
                                                                Position.ScreenBounds.Y + (int) Position.Padding.Top,
                                                                Math.Max(0, Position.ScreenBounds.Width - (int) (Position.Padding.Left + Position.Padding.Right)),
                                                                Math.Max(0, Position.ScreenBounds.Height - (int) (Position.Padding.Top + Position.Padding.Bottom))
                                                               );
                }
        }
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void Update(GameTime gameTime) 
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
    public override void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
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
    private void DrawSimpleText(Cameras.Camera2D camera, string text, SpriteFont font)
    {
        Vector2 textSize = font.MeasureString(text);
        Vector2 textPosition = new(Position.ScreenPaddedBounds.X, Position.ScreenPaddedBounds.Y);

            // Ajustar alineación horizontal
            switch (HorizontalAlignment)
            {
                case HorizontalAlignmentType.Left:
                        textPosition.X = Position.ScreenPaddedBounds.X;
                    break;
                case HorizontalAlignmentType.Right:
                        textPosition.X = Position.ScreenPaddedBounds.Right - textSize.X;
                    break;
                case HorizontalAlignmentType.Center:
                        textPosition.X = Position.ScreenPaddedBounds.X + (Position.ScreenPaddedBounds.Width - textSize.X) / 2;
                    break;
            }
            // Ajustar alineación vertical
            switch (VerticalAlignment)
            {
                case VerticalAlignmentType.Top:
                        textPosition.Y = Position.ScreenPaddedBounds.Y;
                    break;
                case VerticalAlignmentType.Bottom:
                        textPosition.Y = Position.ScreenPaddedBounds.Bottom - textSize.Y;
                    break;
                case VerticalAlignmentType.Center:
                        textPosition.Y = Position.ScreenPaddedBounds.Y + (Position.ScreenPaddedBounds.Height - textSize.Y) / 2;
                    break;
            }
            // Dibuja el texto
            camera.SpriteBatchController.DrawString(font, text, textPosition, Color * Opacity);
    }

    /// <summary>
    ///     Dibuja el texto en varias líneas
    /// </summary>
    private void DrawWrappedText(Cameras.Camera2D camera, string text, SpriteFont font)
    {
        string[] words = text.Split(' ');
        string line = "";
        float yPosition = Position.ScreenPaddedBounds.Y;

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(line) ? word : line + " " + word;
                Vector2 textSize = font.MeasureString(testLine);

                    if (textSize.X > Position.ScreenPaddedBounds.Width && !string.IsNullOrEmpty(line))
                    {
                        // Dibujar línea actual y empezar nueva
                        Vector2 linePosition = new(Position.ScreenPaddedBounds.X, yPosition);
                        camera.SpriteBatchController.DrawString(font, line, linePosition, Color * Opacity);
                        line = word;
                        yPosition += font.LineSpacing * LineSpacing;
                    }
                    else
                        line = testLine;

                    // Verificar límite vertical
                    if (yPosition > Position.ScreenPaddedBounds.Bottom - font.LineSpacing)
                        break;
            }

            // Dibuja la última línea
            if (!string.IsNullOrEmpty(line) && yPosition <= Position.ScreenPaddedBounds.Bottom - font.LineSpacing)
            {
                Vector2 linePosition = new(Position.ScreenPaddedBounds.X, yPosition);

                    camera.SpriteBatchController.DrawString(font, line, linePosition, Color * Opacity);
            }
    }

    /// <summary>
    ///     Texto
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public HorizontalAlignmentType HorizontalAlignment { get; set; } = HorizontalAlignmentType.None;

    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public VerticalAlignmentType VerticalAlignment { get; set; } = VerticalAlignmentType.None;

    /// <summary>
    ///     Nombre de la fuente
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Fuente del texto
    /// </summary>
    private SpriteFont? SpriteFont { get; set; }

    /// <summary>
    ///     Color
    /// </summary>
    public Color Color { get; set; } = Color.White;

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