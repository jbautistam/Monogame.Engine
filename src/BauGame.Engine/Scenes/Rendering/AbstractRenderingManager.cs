using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering;

/// <summary>
///		Manager base para rendering
/// </summary>
public abstract class AbstractRenderingManager
{
	// Variables privadas
	private int _width, _height;

	public AbstractRenderingManager(AbstractScene scene)
	{
		// Inicializa las propoiedades
		Scene = scene;
		Device = Scene.SceneManager.EngineManager.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice;
		Width = Device.PresentationParameters.BackBufferWidth;
		Height = Device.PresentationParameters.BackBufferHeight;
		// Inicializa los objetos
		PostprocessingEffects = new Postprocessing.PostProcessEfectsList(this);
		FiguresRenderer = new Renderers.FiguresRenderer(this);
		SpriteRenderer = new Renderers.SpriteRenderer(this);
		SpriteTextRenderer = new Renderers.SpriteTextRenderer(this);
	}

	/// <summary>
	///		Actualiza el tamaño del viewPort cuando se modifica el tamaño de la ventana
	/// </summary>
	public void UpdateViewPort(Viewport viewport)
	{
		Width = viewport.Width;
		Height = viewport.Height;
	}

    /// <summary>
    ///     Comienza el dibujo del mundo
    /// </summary>
	public abstract void BeginDrawWorld();

	/// <summary>
	///		Arranca el postproceso de la imagen
	/// </summary>
    public abstract void Postprocess();

    /// <summary>
    ///     Arranca el dibujo de la UI
    /// </summary>
	public abstract void BeginDrawUI();

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public abstract void End();

    /// <summary>
    ///		Escena
    /// </summary>
    public AbstractScene Scene { get; }
	
	/// <summary>
	///		Ancho de dibujo
	/// </summary>
	public int Width
	{
		get { return _width; }
		set { IsDirty = Tools.UpdatePropertyFunctions.ChangeProperty(ref _width, value); }
	}

	/// <summary>
	///		Altura de dibujo
	/// </summary>
	public int Height
	{
		get { return _height; }
		set { IsDirty = Tools.UpdatePropertyFunctions.ChangeProperty(ref _height, value); }
	}

	/// <summary>
	///		Indica si se han hecho modificaciones
	/// </summary>
	public bool IsDirty { get; private set; }

	/// <summary>
	///		Dispositivo de dibujo
	/// </summary>
	public GraphicsDevice Device { get; }

	/// <summary>
	///		Lista de efectos de postprocesado
	/// </summary>
	public Postprocessing.PostProcessEfectsList PostprocessingEffects { get; }

	/// <summary>
	///		Batch de sprites
	/// </summary>
	public SpriteBatch? SpriteBatch { get; protected set; }

	/// <summary>
	///		Renderer para figuras
	/// </summary>
	public Renderers.FiguresRenderer FiguresRenderer { get; }

	/// <summary>
	///		Renderer para <see cref="Entities.Sprites.SpriteDefinition"/>
	/// </summary>
	public Renderers.SpriteRenderer SpriteRenderer { get; }

	/// <summary>
	///		Renderer para <see cref="Entities.Sprites.SpriteTextDefinition"/>
	/// </summary>
	public Renderers.SpriteTextRenderer SpriteTextRenderer { get; }
}