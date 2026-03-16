using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter.TextItems;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

/// <summary>
///     Control de texto para una etiqueta que escribe como una máquina de escribir
/// </summary>
public class UiTypeWriterLabel(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Modo de escritura
    /// </summary>
    public enum WriteMode
    {
        /// <summary>Todo el texto</summary>
        Full,
        /// <summary>Por palabras</summary>
        Words,
        /// <summary>Por caracteres</summary>
        Characters
    }
    // Eventos públicos
    public event EventHandler<EventArguments.CommandEventArgs>? CommandExecute;
    // Variables privadas
    private bool _isInitialized, _isDirty;
    private string _text = string.Empty;
    private float _elapsed, _wait, _speedMultiplier;
    private List<TextSectionModel> _textSections = [];

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
    {
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext) 
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
        // Interpreta el texto si es necesario
        if (_isDirty)
        {
            // Interpreta el texto
            _textSections = new CommandTypeWriterGenerator().Parse(Text);
            // Indica que ya se ha interpretado e inicializa las variables de control
            _isDirty = false;
            _wait = 0;
            _speedMultiplier = 1;
        }
        // Añade texto al texto mostrado
        _elapsed += gameContext.DeltaTime;
        if (_wait > 0)
            _wait -= gameContext.DeltaTime;
        else if (!string.IsNullOrWhiteSpace(Text) && _elapsed > Speed * _speedMultiplier)
        {
            // Actualiza las secciones
            UpdateSections();
            // Inicializa el temporizador
            _elapsed = 0;
        }
        // Comprueba si está escribiendo
        IsWriting = !IsWritingEnd();
    }

    /// <summary>
    ///     Comprueba si tiene algo pendiente de escribir
    /// </summary>
    private bool IsWritingEnd()
    {
        // Comprueba si alguna sección no ha terminado
        foreach (TextSectionModel section in _textSections)
            if (!section.Completed)
                return false;
        // Si ha llegado hasta aquí es porque ha terminado de escribir
        return true;
    }

    /// <summary>
    ///     Actualiza las secciones de texto
    /// </summary>
    private void UpdateSections()
    {
        bool mustUpdate = true;

            // Actualiza todas las secciones, no lo hace más después de actualizar la primera que encuentra que no ha terminado
            for (int index = 0; index < _textSections.Count; index++)
                if (!_textSections[index].Completed && mustUpdate)
                {
                    // Actualiza la sección con el modo
                    _textSections[index].Update(Mode);
                    // Indica que la siguiente ya no la debe modificar
                    mustUpdate = false;
                    // Ejecuta los comandos y termina si es necesario
                    if (_textSections[index].JustEnd)
                        ExecuteCommands(_textSections[index].Commands);
                }
    }

    /// <summary>
    ///     Ejecuta los comandos asociados a una sección
    /// </summary>
	private void ExecuteCommands(List<CommandAbstractLine> commands)
	{
        foreach (CommandAbstractLine baseCommand in commands)
            switch (baseCommand)
            {
                case CommandWaitTypeWriter waitCommand:
                        _wait = waitCommand.Time;
                    break;
                case CommandSpeedTypeWriter speedCommand:
                        if (speedCommand.Speed > 0)
                            _speedMultiplier = speedCommand.Speed;
                    break;
                case CommandEvetTypeWriter eventCommand:
                        if (!string.IsNullOrWhiteSpace(eventCommand.Data))
                        {
                            CommandExecute?.Invoke(this, new EventArguments.CommandEventArgs(this, eventCommand.Data));
                            Layer.RaiseCommandExecute(new EventArguments.CommandEventArgs(this, eventCommand.Data));
                        }
                    break;
            }
	}

	/// <summary>
	///     Dibuja el contenido
	/// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        // Dibuja el fondo
        Layer.DrawStyle(renderingManager, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
        // Dibuja el texto
        if (SpriteFont is not null)
            DrawWrappedText(renderingManager, SpriteFont);
        // Depuración
		if (GameEngine.Instance.EngineSettings.DebugMode)
            renderingManager.FiguresRenderer.DrawRectangleOutline(Position.ContentBounds, GameEngine.Instance.EngineSettings.DebugOverlayColor, 2);
    }

    /// <summary>
    ///     Dibuja el texto en varias líneas
    /// </summary>
    private void DrawWrappedText(Scenes.Rendering.RenderingManager renderingManager, SpriteFont font)
    {
        Styles.UiStyle style = Layer.Styles.GetDefault(Style);
        float x = Position.ContentBounds.X, y = Position.ContentBounds.Y;
        bool end = false;
        Color color = style.Color;
        bool bold = false, italic = false;

            // Escribe las secciones
            for (int index = 0; index < _textSections.Count && !end; index++)
            {
                string[] words = _textSections[index].TextToShow.Split([ ' ' ], StringSplitOptions.RemoveEmptyEntries);

                    // Cambia color y estilo
                    color = _textSections[index].Color ?? style.Color;
                    bold = _textSections[index].Bold;
                    italic = _textSections[index].Italic;
                    // Escribe cada una de las palabras
                    foreach (string word in words)
                    {
                        (bool isEndOfLine, string wordToWrite) = CheckEndOfLine(word);
                        Vector2 wordSize = font.MeasureString(wordToWrite + " ");
                        
                            // Modifica las coordenadas en los saltos de línea
                            if (x + wordSize.X > Position.ContentBounds.Right)
                                (x, y) = NewLine(y, font);
                            // Dibuja la cadena
                            renderingManager.TextRenderer.DrawString(font, wordToWrite, new Vector2(x, y), color * style.Opacity);
                            // Cambia la X
                            if (isEndOfLine)
                                (x, y) = NewLine(y, font);
                            else
                                x += wordSize.X;
                     }
                    // Comprueba si se debe terminar de dibujar
                    if (!_textSections[index].Completed)
                        end = true;
            }

        // Comprueba si una palabra tiene un salto de línea
        (bool isEndOfLine, string wordToWrite) CheckEndOfLine(string word)
        {
            bool isEndOfLine = false;

                // Quita el salto final si es necesario y apunta que es un salto de línea
                if (!string.IsNullOrWhiteSpace(word) && (word.EndsWith("\r\n ") || word.EndsWith("\r ") || word.EndsWith("\n ") ||
                                                         word.EndsWith("\r\n") || word.EndsWith("\r") || word.EndsWith("\n")))
                    isEndOfLine = true;
                if (!string.IsNullOrWhiteSpace(word))
                    word = word.Trim();
                // Devuelve el valor que indica si es salto de línea yla palabra resultante
                return (isEndOfLine, word);
        }

        // Obtiene las coordenadas para saltar de línea
        (float x, float y) NewLine(float y, SpriteFont font) => (Position.ContentBounds.X, y + font.LineSpacing * LineSpacing);
    }

    /// <summary>
    ///     Texto (al asignarle un valor, arranca de nuevo la generación)
    /// </summary>
    public string Text 
    { 
        get { return _text; }
        set 
        {
            // Asigna el valor al texto
            _text = value;
            // Normaliza la cadena, se añade un espacio a los \r \n para que al partir por palabras, se haga correctamente
            if (!string.IsNullOrWhiteSpace(_text))
            {
                _text = _text.Replace("\r\n", "\n ");
                _text = _text.Replace("\n", "\n ");
            }
            // Indica que se debe interpretar
            _isDirty = true;
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
    ///     Espaciado de líneas
    /// </summary>
    public float LineSpacing { get; set; } = 1f;

    /// <summary>
    ///     Velocidad para mostrar los caracteres
    /// </summary>
    public float Speed { get; set; } = 0.01f;

    /// <summary>
    ///     Modo de escritura
    /// </summary>
    public WriteMode Mode { get; set; } = WriteMode.Characters;

    /// <summary>
    ///     Indica si está escribiendo
    /// </summary>
    public bool IsWriting { get; private set; }
}