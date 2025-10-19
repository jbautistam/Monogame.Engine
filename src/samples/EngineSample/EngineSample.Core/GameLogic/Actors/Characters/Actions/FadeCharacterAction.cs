using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.Tween;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para cambiar la opacidad de un personaje
/// </summary>
public class FadeCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza la acción de mostrar
	/// </summary>
	protected override bool UpdateAction(CharacterActor actor, float elapsed, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		TweenResult<float> tweenOpacity = TweenCalculator.CalculateFloat(elapsed + gameContext.DeltaTime, Duration,
																		 ActorStartOpacity, EndOpacity);

			// Cambia los estados del actor
			actor.Opacity = tweenOpacity.Value;
			// Devuelve el valor que indica si ha terminado la acción
			return tweenOpacity.IsComplete;
	}

	/// <summary>
	///		Final de la opacidad
	/// </summary>
	public float EndOpacity { get; set; } = 1;
}
