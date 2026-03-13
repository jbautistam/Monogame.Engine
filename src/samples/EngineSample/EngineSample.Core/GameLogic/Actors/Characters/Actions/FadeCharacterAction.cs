using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para cambiar la opacidad de un personaje
/// </summary>
public class FadeCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza la acción de mostrar
	/// </summary>
	protected override void UpdateActionSelf(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		actor.Opacity = EasingFunctionsHelper.Interpolate(ActorStartOpacity, EndOpacity, Progress, Easing);
	}

	/// <summary>
	///		Final de la opacidad
	/// </summary>
	public float EndOpacity { get; set; } = 1;
}
