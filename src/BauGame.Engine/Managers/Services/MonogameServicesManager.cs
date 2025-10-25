using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Bau.Libraries.BauGame.Engine.Managers.Services;

/// <summary>
///		Servicios de MonoGame necesarios para el motor
/// </summary>
public class MonogameServicesManager(Game game)
{
    // Eventos públicos
    public event EventHandler? ViewPortChanged;

	/// <summary>
	///		Inicializa los servicios
	/// </summary>
	public void Initialize(Configuration.EngineSettings engineSettings)
	{
        // Asigna los datos iniciales de gráficos
        GraphicsDeviceManager = new GraphicsDeviceManager(Game);
        Content = Game.Content;
        // Asigna la resolución lógica / virtual
        GraphicsDeviceManager.PreferredBackBufferWidth = engineSettings.ScreenBufferWidth;
        GraphicsDeviceManager.PreferredBackBufferHeight = engineSettings.ScreenBufferHeight;
        // Asigna el modo de ventana
        GraphicsDeviceManager.IsFullScreen = engineSettings.FullScreen;
        GraphicsDeviceManager.HardwareModeSwitch = engineSettings.HardwareModeSwitch;
        // Configuración para calidad visual
        GraphicsDeviceManager.PreferMultiSampling = engineSettings.PreferMultiSampling;
        GraphicsDeviceManager.SynchronizeWithVerticalRetrace = engineSettings.SynchronizeWithVerticalRetrace;
        // Aplica la configuración gráfica
        GraphicsDeviceManager.ApplyChanges();
        // Set the root directory for content.
        Content.RootDirectory = engineSettings.ContentRoot;
        // Indica si se muestra el cursor del ratón
        Game.IsMouseVisible = engineSettings.IsMouseVisible;
        // Configura la ventana de la partida
        Game.Window.IsBorderless = engineSettings.WindowBorderless;
        Game.Window.AllowUserResizing = engineSettings.WindowAllowUserResizing;
        Game.Window.Title = engineSettings.WindowTitle;
        // Asigna los manejadores de eventos
        Game.Window.ClientSizeChanged += (sender, args) => ViewPortChanged?.Invoke(this, EventArgs.Empty);
        // Aplica el cambio sobre el manejador de gráficos
        GraphicsDeviceManager.ApplyChanges();
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
