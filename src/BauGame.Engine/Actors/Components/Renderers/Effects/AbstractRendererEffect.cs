using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.Effects;

/// <summary>
///		Efectos asociados al dibujo
/// </summary>
public abstract class AbstractRendererEffect(RendererComponent renderer, float? duration)
{
	// Variables privadas
	private float _ellapsed;

	/// <summary>
	///		Actualiza el efecto
	/// </summary>
	public void Update(GameTime gameTime)
	{
		// Actualiza el efecto
		UpdateEffect(gameTime);
		// Detiene el efecto si ha superado la duración
		if (Duration is not null)
		{
			// Incrementa el tiempo pasado
			_ellapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;
			// Detiene el efecto
			if (_ellapsed > Duration)
				Stop();
		}
	}

	/// <summary>
	///		Actualiza el efecto
	/// </summary>
	public abstract void UpdateEffect(GameTime gameTime);

	/// <summary>
	///		Detiene el efecto
	/// </summary>
	public void Stop()
	{
		// Detiene el efecto
		StopEffect();
		// Quita el efecto de la representación
		Renderer.Effects.Remove(this);
	}

	/// <summary>
	///		Detiene el efecto
	/// </summary>
	public abstract void StopEffect();

	/// <summary>
	///		Componente de dibujo
	/// </summary>
	public RendererComponent Renderer { get; } = renderer;

	/// <summary>
	///		Duración del efecto
	/// </summary>
	public float? Duration { get; } = duration;
}