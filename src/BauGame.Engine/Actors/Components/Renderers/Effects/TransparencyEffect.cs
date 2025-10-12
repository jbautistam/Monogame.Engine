using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.Effects;

/// <summary>
///		Efecto de cambio de transparrencia
/// </summary>
public class TransparencyEffect(RendererComponent renderer, float? duration) : AbstractRendererEffect(renderer, duration)
{
	// Variables privadas
	private float? _rendererOpacity;
	private float _elapsedTime;

	/// <summary>
	///		Actualiza la opacidad
	/// </summary>
	public override void UpdateEffect(GameTime gameTime)
	{
		Tools.Tween.TweenResult<float> result = Tools.Tween.TweenCalculator.CalculateFloat(_elapsedTime, Time, Start, End, GetEasing());

			// Obtiene la opacidad inicial de la representación
			if (_rendererOpacity is null)
				_rendererOpacity = Renderer.Opacity;
			// Cambia la opacidad del componente
			Renderer.Opacity = result.Value;
			// Añade el tiempo pasado
			_elapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
			// Si se ha llegado al final, cambia el inicio y fin
			if (result.IsComplete)
			{
				// Inicializa el tiempo
				_elapsedTime = 0;
				// Si se tiene que hacer un bucle, se cambian inicio y fin
				if (Loop)
					(Start, End) = (End, Start);
			}

		// Obtiene la función del tween
		Tools.Tween.TweenCalculator.EaseType GetEasing()
		{
			if (Start < End)
				return Tools.Tween.TweenCalculator.EaseType.ElasticIn;
			else
				return Tools.Tween.TweenCalculator.EaseType.ElasticOut;
		}
	}

	/// <summary>
	///		Detiene el efecto
	/// </summary>
	public override void StopEffect()
	{
		if (_rendererOpacity is not null)
			Renderer.Opacity = _rendererOpacity ?? 1;
	}

	/// <summary>
	///		Transparencia inicial
	/// </summary>
	public required float Start { get; set; }

	/// <summary>
	///		Transparencia final
	/// </summary>
	public required float End { get; set; }

	/// <summary>
	///		Indica si debe hacer un bucle desde el final al inicio
	/// </summary>
	public bool Loop { get; set; } = true;

	/// <summary>
	///		Tiempo entre cambios
	/// </summary>
	public required float Time { get; init; }
}