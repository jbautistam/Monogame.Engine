using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering;

/// <summary>
///		Manager para rendering
/// </summary>
public class RenderingManager
{
	// Variables privadas
	private bool _isDrawing = false;

	public RenderingManager(AbstractScene scene)
	{
		Scene = scene;
		Device = Scene.SceneManager.EngineManager.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice;
		FiguresRenderer = new Renderers.FiguresRenderer(this);
		SpriteRenderer = new Renderers.SpriteRenderer(this);
		SpriteTextRenderer = new Renderers.SpriteTextRenderer(this);
	}

	/// <summary>
	///		Prepara el spritebatch
	/// </summary>
	private void Prepare()
	{
		if (SpriteBatch is null)
		{
			// Inicia los objetos
			SpriteBatch = new SpriteBatch(Device);
			// Indica que aún no ha comenzado a dibujar
			_isDrawing = false;
		}
	}

	/// <summary>
	///		Limpia la pantalla
	/// </summary>
	public void Clear()
	{
		Prepare();
		Device.Clear(ClearOptions.Target, Color.Black, 0, 0);
	}

    /// <summary>
    ///     Comienza el dibujo del mundo
    /// </summary>
	public void BeginDrawWorld()
	{
		Clear();
        BeginDraw(Scene.Camera.GetMatrixDrawWorld());
	}

    /// <summary>
    ///     Arranca el dibujo de la UI
    /// </summary>
	public void BeginDrawUI()
	{
        BeginDraw(null);
	}

	/// <summary>
	///		Comienza el dibujo
	/// </summary>
	private void BeginDraw(Matrix? viewMatrix)
	{
		// Prepara los buffers de dibujo
		Prepare();
		// Arranca el dibujo
		if (SpriteBatch is not null)
		{
			// Finaliza los dibujos anteriores
			End();
			// Arranca el dibujo
			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
							  RasterizerState.CullCounterClockwise, null, viewMatrix);
			// Indica que está dibujando
			_isDrawing = true;
		}
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public void End()
	{
		if (SpriteBatch is not null && _isDrawing)
		{
			SpriteBatch.End();
			_isDrawing = false;
		}
	}

	/// <summary>
	///		Escena
	/// </summary>
	public AbstractScene Scene { get; }

	/// <summary>
	///		Dispositivo de dibujo
	/// </summary>
	public GraphicsDevice Device { get; }

	/// <summary>
	///		Batch de sprites
	/// </summary>
	public SpriteBatch? SpriteBatch { get; private set; }

	/// <summary>
	///		Renderer para figuras
	/// </summary>
	public Renderers.FiguresRenderer FiguresRenderer { get; }

	/// <summary>
	///		Renderer para <see cref="Entities.Common.Sprites.SpriteDefinition"/>
	/// </summary>
	public Renderers.SpriteRenderer SpriteRenderer { get; }

	/// <summary>
	///		Renderer para <see cref="Entities.Common.Sprites.SpriteTextDefinition"/>
	/// </summary>
	public Renderers.SpriteTextRenderer SpriteTextRenderer { get; }
}