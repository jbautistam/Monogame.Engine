using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering;

/// <summary>
///		Manager para rendering
/// </summary>
public class RenderingManager(AbstractScene scene) : AbstractRenderingManager(scene)
{
	// Variables privadas
	private bool _isDrawing = false;

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
	public override void BeginDrawWorld()
	{
		Clear();
        BeginDraw(Scene.Camera.GetMatrixDrawWorld());
	}

    /// <summary>
    ///     Arranca el dibujo de la UI
    /// </summary>
	public override void BeginDrawUI()
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
	///		Comienza el postproceso
	/// </summary>
    public override void Postprocess()
	{
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public override void End()
	{
		if (SpriteBatch is not null && _isDrawing)
		{
			SpriteBatch.End();
			_isDrawing = false;
		}
	}
}