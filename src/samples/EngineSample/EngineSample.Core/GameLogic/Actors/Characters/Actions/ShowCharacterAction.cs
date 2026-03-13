using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para mostrar un personaje
/// </summary>
public class ShowCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza la acción de mostrar
	/// </summary>
	protected override void UpdateActionSelf(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		Vector2 position = EasingFunctionsHelper.Interpolate(ToWorld(actor, StartPosition), ToWorld(actor, EndPosition), Progress, Easing);
		float opacity = EasingFunctionsHelper.Interpolate(StartOpacity, EndOpacity, Progress, Easing);

			// Cambia la definición del personaje
			actor.SetDefinition(DefinitionId);
			// Cambia los estados del actor
			actor.Transform.Bounds.X = position.X;
			actor.Transform.Bounds.Y = position.Y;
			actor.Opacity = opacity;
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
