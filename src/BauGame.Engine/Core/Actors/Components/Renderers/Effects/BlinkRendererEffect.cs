using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Components.Renderers.Effects;

/// <summary>
///		Efecto de parpadero
/// </summary>
public class BlinkRendererEffect(RendererComponent renderer, float? duration) : AbstractRendererEffect(renderer, duration)
{
	// Variables privadas
	private Color? _rendererColor;
	private float _elapsedTime;
	private int _actualColor;

	/// <summary>
	///		Actualiza el color
	/// </summary>
	public override void UpdateEffect(GameTime gameTime)
	{
		// Obtiene el color inicial de la representación
		if (_rendererColor is null)
			_rendererColor = Renderer.Color;
		// Ejecuta el efecto
		if (Colors.Count > 0)
		{
			// Añade el tiempo pasado
			_elapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
			// Comprueba si debe cambiar de color
			if (_elapsedTime > TimeBetweenColor)
			{
				_actualColor = (_actualColor + 1) % Colors.Count;
				_elapsedTime = 0;
			}
			// Cambia el color del componente de representación
			Renderer.Color = Colors[_actualColor];
		}
	}

	/// <summary>
	///		Detiene el efecto
	/// </summary>
	public override void StopEffect()
	{
		if (_rendererColor is not null)
			Renderer.Color = _rendererColor ?? Color.White;
	}

	/// <summary>
	///		Colores que forman el parpadeo
	/// </summary>
	public required List<Color> Colors { get; init; }

	/// <summary>
	///		Tiempo entre colores
	/// </summary>
	public required float TimeBetweenColor { get; init; }
}