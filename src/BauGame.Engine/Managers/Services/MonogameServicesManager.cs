using Bau.BauEngine.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Managers.Services;

/// <summary>
///		Servicios de MonoGame necesarios para el motor
/// </summary>
public class MonogameServicesManager(EngineManager engineManager)
{
    // Eventos públicos
    public event EventHandler? ViewPortChanged;

	/// <summary>
	///		Inicializa los servicios
	/// </summary>
	public void Initialize(EngineSettings engineSettings)
	{
        // Asigna los datos iniciales de gráficos
        GraphicsDeviceManager = new GraphicsDeviceManager(EngineManager.EngineGame);
        Content = EngineManager.EngineGame.Content;
        Content.RootDirectory = "Content";
        // Asigna los manejadores de eventos
        EngineManager.EngineGame.Window.ClientSizeChanged += (sender, args) => ViewPortChanged?.Invoke(this, EventArgs.Empty);
        // Aplica la configuración
        Configure(engineSettings);
	}

    /// <summary>
    ///     Parámetros del motor
    /// </summary>
    public void Configure(EngineSettings engineSettings)
    {
        // Asigna la resolución lógica / virtual
        GraphicsDeviceManager.PreferredBackBufferWidth = engineSettings.ScreenSettings.ScreenBufferWidth;
        GraphicsDeviceManager.PreferredBackBufferHeight = engineSettings.ScreenSettings.ScreenBufferHeight;
        // Asigna el modo de ventana
        GraphicsDeviceManager.IsFullScreen = engineSettings.ScreenSettings.FullScreen || engineSettings.ScreenSettings.Borderless;
        GraphicsDeviceManager.HardwareModeSwitch = !engineSettings.ScreenSettings.Borderless && engineSettings.ScreenSettings.FullScreen;
        // Configuración para calidad visual
        GraphicsDeviceManager.PreferMultiSampling = engineSettings.ScreenSettings.PreferMultiSampling;
        GraphicsDeviceManager.SynchronizeWithVerticalRetrace = engineSettings.ScreenSettings.SynchronizeWithVerticalRetrace;
        // Indica si se muestra el cursor del ratón
        EngineManager.EngineGame.IsMouseVisible = engineSettings.ScreenSettings.IsMouseVisible;
        // Configura la ventana de la partida
        EngineManager.EngineGame.Window.IsBorderless = engineSettings.ScreenSettings.Borderless;
        EngineManager.EngineGame.Window.AllowUserResizing = engineSettings.ScreenSettings.WindowAllowUserResizing;
        EngineManager.EngineGame.Window.Title = engineSettings.WindowTitle;
        // Perfil de gráficos para HLSL
        GraphicsDeviceManager.GraphicsProfile = Convert(engineSettings.ScreenSettings.Profile);
        // Orientación
        GraphicsDeviceManager.SupportedOrientations = Convert(engineSettings.ScreenSettings.DisplayOrientation);
        // Asigna el direcotrio raíz para el contenido
        Content.RootDirectory = engineSettings.ContentRoot;
        // Aplica los cambios sobre el manejador de gráficos
        GraphicsDeviceManager.ApplyChanges();
    }

    /// <summary>
    ///     Convierte el perfil de los gráficos
    /// </summary>
	private GraphicsProfile Convert(ScreenSettings.GraphicsProfile profile)
	{
        return profile switch
                {
                    ScreenSettings.GraphicsProfile.HighDefinition => GraphicsProfile.HiDef,
                    _ => GraphicsProfile.Reach
                };
	}

    /// <summary>
    ///     Convierte la orientación de pantalla
    /// </summary>
    private DisplayOrientation Convert(ScreenSettings.DeviceOrientation orientation)
	{
		return orientation switch
			    {
				    ScreenSettings.DeviceOrientation.LandscapeLeft => DisplayOrientation.LandscapeLeft,
				    ScreenSettings.DeviceOrientation.LandscapeRight => DisplayOrientation.LandscapeRight,
				    ScreenSettings.DeviceOrientation.LandscapeLeftAndRight => DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
				    ScreenSettings.DeviceOrientation.Portrait => DisplayOrientation.Portrait,
				    ScreenSettings.DeviceOrientation.PortraitDown => DisplayOrientation.PortraitDown,
				    _ => DisplayOrientation.Default,
			    };
	}

	/// <summary>
	///     Datos de la partida
	/// </summary>
	public EngineManager EngineManager { get; } = engineManager;

	/// <summary>
	///		Manager de gráficos
	/// </summary>
	public GraphicsDeviceManager GraphicsDeviceManager { get; private set; } = default!;

	/// <summary>
	///		Manager de contenidos
	/// </summary>
	public ContentManager Content { get; private set; } = default!;
}
