using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///     Control de texto para una etiqueta
/// </summary>
public class UiBalloonLabel(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Variables privadas
    private bool _isInitialized;
    private string _text = string.Empty, _showText = string.Empty;
    private float _elapsed = 0;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds()
    {
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void Update(GameTime gameTime) 
    {
        // Inicializa el contenido
        if (!_isInitialized)
        {
            // Carga la fuente
            if (!string.IsNullOrWhiteSpace(Font))
                SpriteFont = Layer.Scene.LoadSceneAsset<SpriteFont>(Font);
            // Indica que ya está inicializado
            _isInitialized = true;
        }
        // Añade texto al texto mostrado
        _elapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if (!string.IsNullOrWhiteSpace(Text) && _showText.Length != Text.Length && _elapsed > Speed)
        {
            // Añade el siguiente carácter
            _showText += Text[_showText.Length];
            // Inicializa el temporizador
            _elapsed = 0;
        }
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
        if (!string.IsNullOrEmpty(_showText) && SpriteFont is not null)
            DrawWrappedText(camera, _showText, SpriteFont);
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
    ///     Texto (al asignarle un valor, arranca de nuevo la generación)
    /// </summary>
    public string Text 
    { 
        get { return _text; }
        set 
        {
            _text = value;
            _showText = string.Empty;
        }
    }

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
    ///     Espaciado de líneas
    /// </summary>
    public float LineSpacing { get; set; } = 1.2f;

    /// <summary>
    ///     Velocidad para mostrar los caracteres
    /// </summary>
    public float Speed { get; set; } = 0.01f;
}