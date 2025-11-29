using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Debugger;

/// <summary>
///     Manager para información de depuración
/// </summary>
public class DebugManager(EngineManager manager)
{
    // Variables privadas
    private Queue<(string message, Color? color)> _messages = [];
    private List<(string id, object figure, Color? color)> _figures = [];
    private float _fps;
    private int _frameCount = 0;
    private float _elapsedTime = 0f;
    private bool _initialized;

    /// <summary>
    ///     Log de un mensaje
    /// </summary>
    public void Log(string message, Color? color = null)
    {
        if (Manager.EngineSettings.DebugMode)
        {
            // Encola el mensaje
            _messages.Enqueue(($"[{DateTime.Now:HH:mm:ss.fff}] {message}", color));
            // Elimina los mensajes antiguos
            while (_messages.Count > MaxMessages)
                _messages.Dequeue();
        }
    }

	/// <summary>
	///		Añade un rectángulo a la lista de figuras a visualizar para el log
	/// </summary>
    public void LogRectangle(string id, Models.RectangleF rectangle, Color? color = null)
    {
		AddFigure(id, rectangle, color);
    }

	/// <summary>
	///		Añade un círculo a la lista de figuras a visualizar para el log
	/// </summary>
	public void LogCircle(string id, Models.Circle circle, Color? color = null)
	{
		AddFigure(id, circle, color);
	}

	/// <summary>
	///		Elimina una figura de los comandos de dibujo
	/// </summary>
	public void RemoveFigure(string id)
	{
		for (int index = _figures.Count - 1; index >= 0; index--)
			if (_figures[index].id.Equals(id, StringComparison.CurrentCultureIgnoreCase))
				_figures.RemoveAt(index);
	}

	/// <summary>
	///		Añade un círculo a la lista de figuras a visualizar para el log
	/// </summary>
	private void AddFigure(string id, object figure, Color? color = null)
	{
		if (Manager.EngineSettings.DebugMode)
			_figures.Add((id, figure, color));
	}

    /// <summary>
    ///     Actualiza la información de log
    /// </summary>
    public void Update(GameContext gameContext)
    {
        if (Manager.EngineSettings.DebugMode)
        {
            // Obtiene la fuente
            if (!_initialized && !string.IsNullOrWhiteSpace(Manager.EngineSettings.DebugFont) && DebugFont is null)
            {
                DebugFont = GameEngine.Instance.ResourcesManager.GlobalContentManager.LoadAsset<SpriteFont>(Manager.EngineSettings.DebugFont);
                _initialized = true;
            }
            // Incrementa el tiempo y el número de frames
            _elapsedTime += gameContext.DeltaTime;
            _frameCount++;
            // Calcula el ratio de frames por segundo
            if (_elapsedTime >= 1.0f)
            {
                _fps = _frameCount / _elapsedTime;
                _frameCount = 0;
                _elapsedTime = 0f;
            }
        }
    }

    /// <summary>
    ///     Dibuja las figuras de log
    /// </summary>
    public void DrawLogFigures(GameContext gameContext, Scenes.Cameras.Camera2D camera2D)
    {
        if (Manager.EngineSettings.DebugMode && DebugFont is not null)
        {
		}
	}

    /// <summary>
    ///     Dibuja la información de log
    /// </summary>
    public void DrawLogStrings(GameContext gameContext, Scenes.Cameras.Camera2D camera2D)
    {
        if (Manager.EngineSettings.DebugMode && DebugFont is not null)
        {
            Vector2 position = camera2D.WorldToScreenRelative(LogPosition);

                // Muestra los mensajes
                foreach ((string message, Color? color) in _messages)
                {
                    camera2D.SpriteBatchController.DrawString(DebugFont, message, position, color ?? Manager.EngineSettings.DebugColor);
                    position.Y += DebugFont.LineSpacing;
                }
                // Muestra las estadísticas
                position = camera2D.WorldToScreenRelative(OverlayPosition);
                camera2D.SpriteBatchController.DrawString(DebugFont, $"FPS: {_fps:F1}", position, Manager.EngineSettings.DebugOverlayColor);
                position.Y += DebugFont.LineSpacing;
                camera2D.SpriteBatchController.DrawString(DebugFont, $"TimeScale: {gameContext.TimeScale}", position, Manager.EngineSettings.DebugOverlayColor);
                position.Y += DebugFont.LineSpacing;
                camera2D.SpriteBatchController.DrawString(DebugFont, $"Paused: {gameContext.Paused}", position, Manager.EngineSettings.DebugOverlayColor);
                position.Y += DebugFont.LineSpacing;
        }
    }

    /// <summary>
    ///     Número máximo de mensajes de log
    /// </summary>
    public int MaxMessages { get; set; } = 10;

    /// <summary>
    ///     Posición desde la que se muestra el log
    /// </summary>
    public Vector2 LogPosition { get; set; } = new(0, 0);

    /// <summary>
    ///     Posición desde la que se muestra el log
    /// </summary>
    public Vector2 OverlayPosition { get; set; } = new(0.9f, 0.8f);

    /// <summary>
    ///     Fuente de depuración
    /// </summary>
    public SpriteFont? DebugFont { get; private set; }

    /// <summary>
    ///     Manager principal
    /// </summary>
    public EngineManager Manager { get; } = manager;
}

/*
// Textura blanca de 1x1 para dibujar formas
public static Texture2D Pixel { get; private set; }
// En LoadContent():
Pixel = new Texture2D(GraphicsDevice, 1, 1);
Pixel.SetData(new[] { Color.White });
*/