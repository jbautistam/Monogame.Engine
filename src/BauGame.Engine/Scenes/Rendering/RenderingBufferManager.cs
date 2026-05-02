using Bau.BauEngine.Scenes.Rendering.Postprocessing;
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
			_renderTarget = new RenderTarget2D(Device, Width, Height, false, SurfaceFormat.Color, DepthFormat.None);
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
			SpriteBatch.Begin();
			// Aplica los efectos o dibuja directamente la textura del buffer
			if (PostprocessingEffects.Count == 0)
				SpriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			else if (_renderTarget is not null)
			{
				_renderTarget = ApplyEffects(_renderTarget);
				SpriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			}
			// Y finaliza el dibujo por lotes
			End();
		}
	}

/*
	// Dibuja TODO el mundo en el render target y luego aplica el efecto
    public void DrawGameWorldAndApplyTransition(System.Action worldDrawingAction)
    {
        // 1. Dibujar el mundo en el render target
        _graphicsDevice.SetRenderTarget(_gameWorldTarget);
        _graphicsDevice.Clear(Color.Transparent); // o Color.CornflowerBlue

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        worldDrawingAction?.Invoke(); // Aquí dibujas tus entidades, tiles, etc.
        _spriteBatch.End();

        // 2. Volver a dibujar en la pantalla principal
        _graphicsDevice.SetRenderTarget(null);
        _graphicsDevice.Clear(Color.Black); // o el color de fondo

        if (_useShaderEffect && _postProcessEffect != null)
        {
            // Dibujar el render target aplicando el shader
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, 
                               SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _postProcessEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(_gameWorldTarget, new Rectangle(0, 0, Width, Height), Color.White);
            _spriteBatch.End();
        }
        else if (_transitionAlpha > 0f)
        {
            // Efecto simple: dibujar el mundo normalmente y encima un rectángulo semitransparente
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _spriteBatch.Draw(_gameWorldTarget, new Rectangle(0, 0, Width, Height), Color.White);
            
            // Textura de 1x1 blanca para el overlay
            Texture2D whitePixel = GetWhitePixel();
            _spriteBatch.Draw(whitePixel, new Rectangle(0, 0, Width, Height), 
                              _transitionColor * _transitionAlpha);
            _spriteBatch.End();
        }
        else
        {
            // Sin efecto: solo dibujar el mundo
            _spriteBatch.Begin();
            _spriteBatch.Draw(_gameWorldTarget, new Rectangle(0, 0, Width, Height), Color.White);
            _spriteBatch.End();
        }
    }
*/

	/// <summary>
	///		Aplica los efectos sobre la textura dibujada
	/// </summary>
    private RenderTarget2D ApplyEffects(RenderTarget2D source)
    {
		RenderTarget2D destination = new(Device, Width, Height);

			// Aplica los efectos
			foreach (AbstractPostProcessingEffect effect in PostprocessingEffects.Enumerate())
			{
				SpriteBatch spriteBatch;

					// Cambia el destino del render
					Device.SetRenderTarget(destination);
					Device.Clear(Color.Black);
					// Aplica el efecto
					spriteBatch = new SpriteBatch(Device);
					spriteBatch.Begin();
					effect.Apply(source, spriteBatch);
					spriteBatch.End();
					// Intercambia el origen y el destino
					(source, destination) = (destination, source);
	        }
			// Devuelve el resultado
			return source;
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
							  RasterizerState.CullCounterClockwise, null, drawMatrix);
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