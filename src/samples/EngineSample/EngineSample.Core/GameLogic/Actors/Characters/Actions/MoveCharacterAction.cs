using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para mover un personaje
/// </summary>
public class MoveCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza la acción de mostrar
	/// </summary>
	protected override void UpdateActionSelf(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		Vector2 position = EasingFunctionsHelper.Interpolate(ActorStartPosition, ToWorld(actor, EndPosition), Progress, Easing);

			// Cambia la posición del actor
			actor.Transform.Bounds.X = position.X;
			actor.Transform.Bounds.Y = position.Y;
	}

	/// <summary>
	///		Posición en la que debe terminar de mostrarse el personaje
	/// </summary>
	public required Vector2 EndPosition { get; init; }
}
