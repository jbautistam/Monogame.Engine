using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Configuration;

/// <summary>
///     Manager del modo gráfico
///!    Debería depender de la clase del motor pero aquí no la tenemos
/// </summary>
public class GraphicsModeManager(GameWindow window, GraphicsDeviceManager graphicsDeviceManager)
{
    /// <summary>
    ///     Aplica una configuración
    /// </summary>
    public void ApplyConfiguration(GraphicsConfiguration configuration)
    {
        // Resolución
        GraphicsDeviceManager.PreferredBackBufferWidth = configuration.ResolutionWidth;
        GraphicsDeviceManager.PreferredBackBufferHeight = configuration.ResolutionHeight;
        // Fullscreen mode
        GraphicsDeviceManager.IsFullScreen = configuration.FullScreen || configuration.Borderless;
        GraphicsDeviceManager.HardwareModeSwitch = !configuration.Borderless && configuration.FullScreen;
        // Sincronización
        GraphicsDeviceManager.SynchronizeWithVerticalRetrace = configuration.VSync;
        // Calidad
        GraphicsDeviceManager.PreferMultiSampling = configuration.MultiSampling;
        GraphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        // Aplicar
        GraphicsDeviceManager.ApplyChanges();
        // Eventos para manejar resize manual del usuario
        Window.AllowUserResizing = !configuration.FullScreen;
        Window.ClientSizeChanged += OnWindowSizeChanged;
    }
    
    /// <summary>
    ///     Manejador del evento de cambio de tamaño de la ventana
    /// </summary>
    private void OnWindowSizeChanged(object? sender, EventArgs e)
    {
        int newWidth = GraphicsDeviceManager.GraphicsDevice.Viewport.Width;
        int newHeight = GraphicsDeviceManager.GraphicsDevice.Viewport.Height;
        
        //! Debería lanzar un evento para recalcular las matrices de cámara
        // Aquí recalculas tu RenderTarget o matrices de cámara
        // UpdateAspectRatio(newWidth, newHeight);
    }

    /// <summary>
    ///     Ventana
    /// </summary>
    public GameWindow Window { get; } = window;

    /// <summary>
    ///     Manager de los gráficos del dispositivo
    /// </summary>
    public GraphicsDeviceManager GraphicsDeviceManager { get; } = graphicsDeviceManager;
}

/*
// Explorar hardware disponible
void ListGraphicsCapabilities()
{
    // Adaptadores (tarjetas gráficas/monitores)
    foreach (var adapter in GraphicsAdapter.Adapters)
    {
        Console.WriteLine($"Adapter: {adapter.Description}");
        Console.WriteLine($"Monitor: {adapter.CurrentDisplayMode.Width}x{adapter.CurrentDisplayMode.Height}");
        
        // Modos de display soportados
        foreach (var mode in adapter.SupportedDisplayModes)
        {
            Console.WriteLine($"  {mode.Width}x{mode.Height} @ {mode.RefreshRate}Hz - Format: {mode.Format}");
        }
    }
}

// Seleccionar modo específico
void SetSpecificMode(int width, int height, int refreshRate)
{
    var adapter = GraphicsAdapter.DefaultAdapter;
    var targetMode = adapter.SupportedDisplayModes.FirstOrDefault(
        m => m.Width == width && 
             m.Height == height && 
             m.RefreshRate == refreshRate);
    
    if (targetMode != null)
    {
        _graphics.PreferredBackBufferWidth = targetMode.Width;
        _graphics.PreferredBackBufferHeight = targetMode.Height;
        // MonoGame seleccionará automáticamente el modo más cercano
        _graphics.ApplyChanges();
    }
}
*/