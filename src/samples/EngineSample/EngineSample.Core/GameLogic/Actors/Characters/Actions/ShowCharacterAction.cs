using Bau.Libraries.BauGame.Engine.Tools.Tween;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para mostrar un personaje
/// </summary>
public class ShowCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza la acción de mostrar
	/// </summary>
	protected override bool UpdateAction(CharacterActor actor, float elapsed, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		TweenResult<Vector2> tweenPosition = TweenCalculator.CalculateVector2(elapsed + gameContext.DeltaTime, Duration,
																			  StartPosition, EndPosition);
		TweenResult<float> tweenOpacity = TweenCalculator.CalculateFloat(elapsed + gameContext.DeltaTime, Duration,
																		 StartOpacity, EndOpacity);

			// Cambia la definición del personaje
			actor.SetDefinition(DefinitionId);
			// Cambia los estados del actor
			actor.Transform.Bounds.X = tweenPosition.Value.X;
			actor.Transform.Bounds.Y = tweenPosition.Value.Y;
			actor.Opacity = tweenOpacity.Value;
			// Devuelve el valor que indica si ha terminado la acción
			return tweenPosition.IsComplete && tweenOpacity.IsComplete;
	}

	/// <summary>
	///		Id de la definición
	/// </summary>
	public required string DefinitionId { get; init; }

	/// <summary>
	///		Posición en la que debe comenzar a mostrarse el personaje
	/// </summary>
	public required Vector2 StartPosition { get; init; }

	/// <summary>
	///		Posición en la que debe terminar de mostrarse el personaje
	/// </summary>
	public required Vector2 EndPosition { get; init; }

	/// <summary>
	///		Arranque de la opacidad
	/// </summary>
	public float StartOpacity { get; set; } = 1;

	/// <summary>
	///		Final de la opacidad
	/// </summary>
	public float EndOpacity { get; set; } = 1;
}
