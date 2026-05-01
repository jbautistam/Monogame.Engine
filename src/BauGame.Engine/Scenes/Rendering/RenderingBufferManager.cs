using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering;

/// <summary>
///		Manager para rendering
/// </summary>
public class RenderingBufferManager(AbstractScene scene) : AbstractRenderingManager(scene)
{
	// Variables privadas
    private RenderTarget2D? _renderTarget;
	private bool _isDrawing;

    /// <summary>
    ///     Comienza el dibujo del mundo
    /// </summary>
	public override void BeginDrawWorld()
	{
		// Crea la textura de dibujo
		if (IsDirty || _renderTarget is null)
			_renderTarget = new RenderTarget2D(Device, Device.PresentationParameters.BackBufferWidth, 
											   Device.PresentationParameters.BackBufferHeight,
											   false, SurfaceFormat.Color, DepthFormat.None);
		// Configura el dispositivo para pintar sobre la textura intermedia
		Device.SetRenderTarget(_renderTarget);
		Device.Clear(Color.Black);
		// Crea el objeto para el dibujo por lotes
		SpriteBatch = new SpriteBatch(Device);
		// Arranca el dibujo
		BeginDraw(Scene.Camera.GetMatrixDrawWorld());
	}

	/// <summary>
	///		Comienza el postproceso
	/// </summary>
    public override void Postprocess()
	{
		if (SpriteBatch is not null)
		{
			// Finaliza el dibujo sobre el spriteBatch
			End();
			// Indica que está dibujando
			_isDrawing = true;
			// Vuelve a la pantalla y la limpia
			Device.SetRenderTarget(null);
			Device.Clear(Color.Black);
			// Crea el nuevo spritebatch donde aplicar los efectos
			SpriteBatch = new SpriteBatch(Device);
			// Arranca el SpriteBatch y dibuja la textura destino
			SpriteBatch.Begin();
			SpriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			// Y finaliza el dibujo por lotes
			End();
		}
	}

    /// <summary>
    ///     Arranca el dibujo de la UI
    /// </summary>
	public override void BeginDrawUI()
	{
		// Finaliza el dibujo pendiente
		End();
		// Crea el nuevo spritebatch donde dibujar la interface ce usuario
		SpriteBatch = new SpriteBatch(Device);
		// Inicia el SpriteBatch y dibuja la textura destino
		BeginDraw(null);
	}

	/// <summary>
	///		Inicia el dibujo
	/// </summary>
	private void BeginDraw(Matrix? drawMatrix)
	{
		if (SpriteBatch is not null)
		{
			// Inicia el SpriteBatch y dibuja la textura destino
			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
							  RasterizerState.CullCounterClockwise, null, null);
			// ... e indica que está dibujando
			_isDrawing = true;
		}
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public override void End()
	{
		// Cierra el lote de dibujo
		if (SpriteBatch is not null && _isDrawing)
			SpriteBatch.End();
		// Indica que ya no está dibujando
		_isDrawing = false;
	}
}