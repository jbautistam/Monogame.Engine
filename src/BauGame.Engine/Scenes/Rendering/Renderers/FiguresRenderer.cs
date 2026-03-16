using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Renderers;

/// <summary>
///		Clase de presentación para figuras
/// </summary>
public class FiguresRenderer
{
	// Variables privadas
	private Texture2D _whitePixel = default!;

	public FiguresRenderer(SpriteBatchController spriteBatchController)
	{
		// Guarda los objetos
		SpriteBatchController = spriteBatchController;
		// Prepara la textura de 1x1 en color blanco
		_whitePixel = new Texture2D(spriteBatchController.Device, 1, 1);
		_whitePixel.SetData([ Color.White ]);
	}

	/// <summary>
	///		Dibuja un rectángulo sólido de un color
	/// </summary>
	public void DrawRectangle(Rectangle rectangle, Color color)
	{
		if (SpriteBatchController.SpriteBatch is not null)
			SpriteBatchController.SpriteBatch.Draw(_whitePixel, rectangle, color);
	}

	/// <summary>
	///		Dibuja las líneas de un rectángulo
	/// </summary>
	public void DrawRectangleOutline(Rectangle rectangle, Color color, int thickness = 1)
	{
		if (SpriteBatchController.SpriteBatch is not null)
		{
			// Arriba
			SpriteBatchController.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
			// Abajo
			SpriteBatchController.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);
			// Izquierda
			SpriteBatchController.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
			// Derecha
			SpriteBatchController.SpriteBatch.Draw(_whitePixel, new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
		}
	}

	/// <summary>
	///		Dibuja una línea
	/// </summary>
	public void DrawLine(Vector2 start, Vector2 end, Color color, int thickness = 1)
	{
		if (SpriteBatchController.SpriteBatch is not null)
		{
			Vector2 edge = end - start;
			float angle = (float) Math.Atan2(edge.Y, edge.X);

				// Dibuja la línea como un rectángulo rotado
				SpriteBatchController.SpriteBatch.Draw(_whitePixel, start, null, color, angle, Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0);
		}
	}

	/// <summary>
	///		Controlador de dibujo principal
	/// </summary>
	public SpriteBatchController SpriteBatchController { get; }
}
