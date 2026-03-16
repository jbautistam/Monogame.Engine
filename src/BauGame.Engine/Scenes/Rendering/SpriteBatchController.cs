using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering;

/// <summary>
///		Controlador del SpriteBatch
/// </summary>
public class SpriteBatchController
{
	// Variables privadas
	private bool _isDrawing = false;

	public SpriteBatchController(RenderingManager renderingManager, GraphicsDevice device)
	{
		RenderingManager = renderingManager;
		Device = device;
		FiguresRenderer = new Renderers.FiguresRenderer(this);
		TextRenderer = new Renderers.TextRenderer(this);
		TexturesRenderer = new Renderers.TexturesRenderer(this);
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
		Device.Clear(ClearOptions.Target, Color.Black, 0, 0);
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public void End()
	{
	}

	/// <summary>
	///		Dispositivo
	/// </summary>
	public GraphicsDevice Device { get; }

	/// <summary>
	///		Manager de representación
	/// </summary>
	public RenderingManager RenderingManager { get; }

	/// <summary>
	///		Batch de sprites
	/// </summary>
	public SpriteBatch? SpriteBatch { get; private set; }

	/// <summary>
	///		Renderer para figuras
	/// </summary>
	public Renderers.FiguresRenderer FiguresRenderer { get; }

	/// <summary>
	///		Renderer para cadenas de textos
	/// </summary>
	public Renderers.TextRenderer TextRenderer { get; }

	/// <summary>
	///		Renderer para texturas
	/// </summary>
	public Renderers.TexturesRenderer TexturesRenderer { get; }

	/// <summary>
	///		Renderer para <see cref="Entities.Common.Sprites.SpriteDefinition"/>
	/// </summary>
	public Renderers.SpriteRenderer SpriteRenderer { get; }

	/// <summary>
	///		Renderer para <see cref="Entities.Common.Sprites.SpriteTextDefinition"/>
	/// </summary>
	public Renderers.SpriteTextRenderer SpriteTextRenderer { get; }
}