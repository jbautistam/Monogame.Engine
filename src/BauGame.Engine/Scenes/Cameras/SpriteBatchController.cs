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
	private Texture2D? _whitePixel = null;
	private bool _isDrawing = false;

	/// <summary>
	///		Prepara el spritebatch
	/// </summary>
	private void Prepare()
	{
		if (_device is null || _spriteBatch is null)
		{
			// Inicia los objetos
			_device = GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice;
			_spriteBatch = new SpriteBatch(_device);
			// Crea una textura de 1x1 píxel blanco
			_whitePixel = new Texture2D(_device, 1, 1);
			_whitePixel.SetData([ Color.White ]);
			// Indica que aún no ha comenzado a dibujar
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
	///		Dibuja un rectángulo sólido de un color
	/// </summary>
	public void DrawRectangle(Rectangle rectangle, Color color)
	{
		if (_whitePixel is not null && _spriteBatch is not null)
			_spriteBatch.Draw(_whitePixel, rectangle, color);
	}

	/// <summary>
	///		Dibuja las líneas de un rectángulo
	/// </summary>
	public void DrawRectangleOutline(Rectangle rectangle, Color color, int thickness = 1)
	{
		if (_whitePixel is not null && _spriteBatch is not null)
		{
			// Arriba
			_spriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
			// Abajo
			_spriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);
			// Izquierda
			_spriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
			// Derecha
			_spriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
		}
	}

	/// <summary>
	///		Dibuja una línea
	/// </summary>
	public void DrawLine(Vector2 start, Vector2 end, Color color, int thickness = 1)
	{
		if (_whitePixel is not null && _spriteBatch is not null)
		{
			Vector2 edge = end - start;
			float angle = (float) Math.Atan2(edge.Y, edge.X);

				// Dibuja la línea como un rectángulo rotado
				_spriteBatch.Draw(_whitePixel, start, null, color, angle, Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0);
		}
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