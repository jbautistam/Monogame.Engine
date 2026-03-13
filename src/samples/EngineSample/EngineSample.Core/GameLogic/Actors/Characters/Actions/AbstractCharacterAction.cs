using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción asociada a un personaje
/// </summary>
public abstract class AbstractCharacterAction
{
	// Variables privadas
	private bool _isFirst = true;
	private float _elapsed;

	/// <summary>
	///		Actualiza la acción
	/// </summary>
	public bool Update(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		// Obtiene los valores iniciales del actor
		if (_isFirst)
		{
			// Guarda los valores
			ActorStartPosition = actor.Transform.Bounds.TopLeft;
			ActorStartOpacity = actor.Opacity;
			// Indica que ya no es la primera vez
			_isFirst = false;
		}
		// Incrementa el tiempo pasado
		_elapsed += gameContext.DeltaTime;
		if (Duration == 0)
			Duration = 1;
		Progress = _elapsed / Duration;
		// Actualiza la acción
		UpdateActionSelf(actor, gameContext);
		// Indica si ha terminado
		return Progress >= 1;
	}

	/// <summary>
	///		Actualiza la acción
	/// </summary>
	protected abstract void UpdateActionSelf(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext);

	/// <summary>
	///		Transforma un vector a las posiciones de mundo
	/// </summary>
	protected Vector2 ToWorld(CharacterActor actor, Vector2 position)
	{
		return new Vector2(position.X * actor.Layer.Scene.WorldDefinition.WorldBounds.Width, position.Y * actor.Layer.Scene.WorldDefinition.WorldBounds.Height);
	}

	/// <summary>
	///		Posición inicial del personaje
	/// </summary>
	public Vector2 ActorStartPosition { get; private set; }

	/// <summary>
	///		Opacidad inicial del personaje
	/// </summary>
	public float ActorStartOpacity { get; private set; }

	/// <summary>
	///		Duración del efecto
	/// </summary>
	public float Duration { get; set; } = 2;

	/// <summary>
	///		Porcentaje de progreso
	/// </summary>
	public float Progress { get; protected set; }

	/// <summary>
	///		Modo de interpolación
	/// </summary>
	public EasingFunctionsHelper.EasingType Easing { get; set; } = EasingFunctionsHelper.EasingType.Linear;
}