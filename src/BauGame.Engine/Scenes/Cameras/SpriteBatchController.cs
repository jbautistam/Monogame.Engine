using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras;

/// <summary>
///		Controlador del SpriteBatch
/// </summary>
public class SpriteBatchController
{
	// Variables privadas
	private GraphicsDevice? _device;
	private SpriteBatch? _spriteBatch = null;
	private bool _isDrawing = false;

	/// <summary>
	///		Prepara el spritebatch
	/// </summary>
	private void Prepare()
	{
		if (_device is null || _spriteBatch is null)
		{
			_device = GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice;
			_spriteBatch = new SpriteBatch(_device);
			_isDrawing = false;
		}
	}

	/// <summary>
	///		Limpia la pantalla
	/// </summary>
	public void Clear()
	{
		if (_device is not null)
			_device.Clear(ClearOptions.Target, Color.Black, 0, 0);
	}

	/// <summary>
	///		Comienza el dibujo
	/// </summary>
	public void BeginDraw(Matrix? viewMatrix)
	{
		// Prepara los buffers de dibujo
		Prepare();
		// Arranca el dibujo
		if (_spriteBatch is not null)
		{
			// Finaliza los dibujos anteriores
			End();
			// Arranca el dibujo
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
								RasterizerState.CullCounterClockwise, null, viewMatrix);
			// Indica que está dibujando
			_isDrawing = true;
		}
	}

	/// <summary>
	///		Dibuja una textura
	/// </summary>
	public void Draw(Texture2D texture, Vector2 position, Rectangle? source, Vector2 origin, Vector2 scale, SpriteEffects spriteEffect, 
					 Color color, float rotation, int layerDepth = 0)
	{
		if (_spriteBatch is not null)
			_spriteBatch.Draw(texture, position, source, color, rotation, origin, scale, spriteEffect, layerDepth);
	}

	/// <summary>
	///		Dibuja una textura escalada a un rectángulo
	/// </summary>
	public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
	{
		if (_spriteBatch is not null)
			_spriteBatch.Draw(texture, destinationRectangle, color);
	}

	/// <summary>
	///		Dibuja una textura
	/// </summary>
	public void Draw(Texture2D texture, Rectangle destination, Rectangle? source, Vector2 origin, Color color, float rotation, 
					 SpriteEffects spriteEffect, float layerDepth = 0)
	{
		if (_spriteBatch is not null)
			_spriteBatch.Draw(texture, destination, source, color, rotation, origin, spriteEffect, layerDepth);
	}

	/// <summary>
	///		Dibuja una textura en una posición
	/// </summary>
	public void Draw(Texture2D texture, Vector2 position, Color color)
	{
		if (_spriteBatch is not null)
			_spriteBatch.Draw(texture, position, color);
	}

	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
	{
		if (_spriteBatch is not null)
			_spriteBatch.DrawString(font, text, position, color);
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public void End()
	{
		if (_spriteBatch is not null && _isDrawing)
		{
			_spriteBatch.End();
			_isDrawing = false;
		}
	}
}