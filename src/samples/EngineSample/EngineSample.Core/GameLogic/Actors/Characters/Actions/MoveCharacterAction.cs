using Bau.Libraries.BauGame.Engine.Tools.Tween;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para mover un personaje
/// </summary>
public class MoveCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza la acción de mostrar
	/// </summary>
	protected override bool UpdateAction(CharacterActor actor, float elapsed, GameTime gameTime)
	{
		TweenResult<Vector2> tweenPosition = TweenCalculator.CalculateVector2(elapsed + (float) gameTime.ElapsedGameTime.TotalSeconds, Duration,
																			  ActorStartPosition, EndPosition);

			// Cambia la posición del actor
			actor.Transform.WorldBounds.X = tweenPosition.Value.X;
			actor.Transform.WorldBounds.Y = tweenPosition.Value.Y;
			// Devuelve el valor que indica si ha terminado la acción
			return tweenPosition.IsComplete;
	}

	/// <summary>
	///		Posición en la que debe terminar de mostrarse el personaje
	/// </summary>
	public required Vector2 EndPosition { get; init; }
}
