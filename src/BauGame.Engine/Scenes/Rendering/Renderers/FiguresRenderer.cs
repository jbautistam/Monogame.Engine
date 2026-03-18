using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Renderers;

/// <summary>
///		Clase de presentación para figuras
/// </summary>
public class FiguresRenderer
{
	// Variables privadas
	private Texture2D _whitePixel = default!;

	public FiguresRenderer(RenderingManager renderingManager)
	{
		// Guarda los objetos
		RenderingManager = renderingManager;
		// Prepara la textura de 1x1 en color blanco
		_whitePixel = new Texture2D(renderingManager.Device, 1, 1);
		_whitePixel.SetData([ Color.White ]);
	}

	/// <summary>
	///		Dibuja un rectángulo sólido de un color
	/// </summary>
	public void DrawRectangle(Rectangle rectangle, Color color)
	{
		if (RenderingManager.SpriteBatch is not null)
			RenderingManager.SpriteBatch.Draw(_whitePixel, rectangle, color);
	}

	/// <summary>
	///		Dibuja las líneas de un rectángulo
	/// </summary>
	public void DrawRectangleOutline(Rectangle rectangle, Color color, int thickness = 1)
	{
		if (RenderingManager.SpriteBatch is not null)
		{
			// Arriba
			RenderingManager.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
			// Abajo
			RenderingManager.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);
			// Izquierda
			RenderingManager.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
			// Derecha
			RenderingManager.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
		}
	}

	/// <summary>
	///		Dibuja una línea
	/// </summary>
	public void DrawLine(Vector2 start, Vector2 end, Color color, int thickness = 1)
	{
		if (RenderingManager.SpriteBatch is not null)
		{
			Vector2 edge = end - start;
			float angle = (float) Math.Atan2(edge.Y, edge.X);

				// Dibuja la línea como un rectángulo rotado
				RenderingManager.SpriteBatch.Draw(_whitePixel, start, null, color, angle, Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0);
		}
	}

	/// <summary>
	///		Manager de presentación
	/// </summary>
	public RenderingManager RenderingManager { get; }
}
