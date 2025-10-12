using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Bau.Libraries.BauGame.Engine.Managers.Services;

/// <summary>
///		Servicios de MonoGame necesarios para el motor
/// </summary>
public class MonogameServicesManager(Game game)
{
	/// <summary>
	///		Inicializa los servicios
	/// </summary>
	public void Initialize(Configuration.EngineSettings engineSettings)
	{
        // Asigna los datos iniciales de gráficos
        GraphicsDeviceManager = new GraphicsDeviceManager(Game);
        Content = Game.Content;
        // Set the graphics defaults.
        GraphicsDeviceManager.PreferredBackBufferWidth = engineSettings.ScreenWidth;
        GraphicsDeviceManager.PreferredBackBufferHeight = engineSettings.ScreenHeight;
        GraphicsDeviceManager.IsFullScreen = engineSettings.FullScreen;
        // Aplica la configuración gráfica
        GraphicsDeviceManager.ApplyChanges();
        // Set the root directory for content.
        Content.RootDirectory = engineSettings.ContentRoot;
        // Indica si se muestra el cursor del ratón
        Game.IsMouseVisible = engineSettings.IsMouseVisible;

/*
protected override void Initialize()
{
    _camera = new Camera2D(GraphicsDevice.Viewport)
    {
        Position = new Vector2(640, 360), // Centrado en mundo 1280x720
        Zoom = 1f,
        WorldBounds = new Rectangle(0, 0, 5000, 5000) // Límites del mundo
    };

    _spriteBatchUI = new SpriteBatch(GraphicsDevice);

    Window.ClientSizeChanged += (s, e) =>
    {
        _camera.Resize(GraphicsDevice.Viewport);
    };

    base.Initialize();
}
*/
	}

    /// <summary>
    ///     Datos de la partida
    /// </summary>
    public Game Game { get; } = game;

	/// <summary>
	///		Manager de gráficos
	/// </summary>
	public GraphicsDeviceManager GraphicsDeviceManager { get; private set; } = default!;

	/// <summary>
	///		Manager de contenidos
	/// </summary>
	public ContentManager Content { get; private set; } = default!;
}
