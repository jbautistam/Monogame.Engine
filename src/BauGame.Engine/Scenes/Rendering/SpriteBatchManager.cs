using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering;

/// <summary>
///		Manager del SpriteBatch
/// </summary>
public class SpriteBatchManager(GraphicsDevice device)
{
	/// <summary>
	///		Limpia la pantalla
	/// </summary>
	public void Clear()
	{
		Device.Clear(ClearOptions.Target, Color.Black, 0, 0);
	}

	/// <summary>
	///		Comienza el dibujo
	/// </summary>
	public void BeginDraw(Matrix? viewMatrix)
	{
		// Inicializa el spritebatch
		if (SpriteBatch is null)
		{
			// Inicia los objetos
			SpriteBatch = new SpriteBatch(Device);
			// Indica que aún no ha comenzado a dibujar
			IsDrawing = false;
		}
		// Finaliza los dibujos anteriores
		End();
		// Arranca el dibujo
		SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
						  RasterizerState.CullCounterClockwise, null, viewMatrix);
		// Indica que está dibujando
		IsDrawing = true;
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public void End()
	{
		if (SpriteBatch is not null && IsDrawing)
		{
			SpriteBatch.End();
			IsDrawing = false;
		}
	}

	/// <summary>
	///		Dispositivo de dibujo
	/// </summary>
	public GraphicsDevice Device { get; } = device;

	/// <summary>
	///		Zona de dibujo
	/// </summary>
	public SpriteBatch? SpriteBatch { get; private set; }

	/// <summary>
	///		Indica si se está dibujando
	/// </summary>
	public bool IsDrawing { get; private set; }
}