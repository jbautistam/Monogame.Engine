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
	public void Update(Managers.GameContext gameContext)
	{
		// Actualiza el efecto
		UpdateEffect(gameContext);
		// Detiene el efecto si ha superado la duración
		if (Duration is not null)
		{
			// Incrementa el tiempo pasado
			_ellapsed += gameContext.DeltaTime;
			// Detiene el efecto
			if (_ellapsed > Duration)
				Stop();
		}
	}

	/// <summary>
	///		Actualiza el efecto
	/// </summary>
	public abstract void UpdateEffect(Managers.GameContext gameContext);

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