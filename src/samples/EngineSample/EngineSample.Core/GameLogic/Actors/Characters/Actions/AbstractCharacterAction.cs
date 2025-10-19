using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción asociada a un personaje
/// </summary>
public abstract class AbstractCharacterAction
{
	// Variables privadas
	private float _elapsed;
	private bool _isFirst = true;

	/// <summary>
	///		Actualiza la acción
	/// </summary>
	public bool Update(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		// Obtiene los valores iniciales del actor
		if (_isFirst)
		{
			// Guarda los valores
			ActorStartPosition = actor.Transform.WorldBounds.TopLeft;
			ActorStartOpacity = actor.Opacity;
			// Indica que ya no es la primera vez
			_isFirst = false;
		}
		// Incrementa el tiempo pasado
		_elapsed += gameContext.DeltaTime;
		// Actualiza la acción
		return UpdateAction(actor, _elapsed, gameContext);
	}

	/// <summary>
	///		Actualiza la acción
	/// </summary>
	protected abstract bool UpdateAction(CharacterActor actor, float elapsed, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext);

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
}