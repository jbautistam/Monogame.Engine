using Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Renderers;

/// <summary>
///		Componente de animación
/// </summary>
internal class AnimatorComponent
{
	// Variables privadas
	private float _elapsed;
	private Animation? _actualAnimation;
	private int _frameIndex, _lastIndex;
	private bool _loop;
	private Animation.AnimationFrame? _frame;
	private TextureRegion? _lastRegion;

	/// <summary>
	///		Cambia la animación actual
	/// </summary>
	internal bool SetAnimation(string animation, bool loop)
	{
		if (_actualAnimation is null || !_actualAnimation.Name.Equals(animation, StringComparison.CurrentCultureIgnoreCase))
		{
			// Obtiene la animación actual
			_actualAnimation = GameEngine.Instance.ResourcesManager.AnimationManager.Animations.Get(animation);
			// Marca los datos de inicio
			_frameIndex = 0;
			_elapsed = 0;
			_loop = loop;
			_lastRegion = null;
			// Indica que se ha asignado
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Actualiza el estado del componente
	/// </summary>
	internal void Update(Managers.GameContext gameContext)
	{
		if (_actualAnimation is null)
			_frame = null;
		else if (IsPlaying)
		{
			// Cambia el frame
			_frame = _actualAnimation.GetFrame(_frameIndex);
			// Si se ha encontrado el frame
			if (_frame is not null)
			{
				// Añade el tiempo pasado
				_elapsed += gameContext.DeltaTime;
				// Pasa al siguiente frame
				if (_elapsed >= _frame.time)
				{
					// Incrementa el índice
					_frameIndex++;
					// Pasa al inicio
					if (_frameIndex == _actualAnimation.Frames.Count)
					{
						// Cambia el índice
						_frameIndex = 0;
						// Si no se reproduce en loop, se detiene
						if (!_loop)
							IsPlaying = false;
					}
					// Cambia el frame
					_frame = _actualAnimation.GetFrame(_frameIndex);
					_elapsed = 0;
				}
			}
		}
	}

	/// <summary>
	///		Obtiene la textura
	/// </summary>
	internal TextureRegion? GetTexture(AbstractTexture texture)
	{
		// Obtiene la textura (si ha cambiado)
		if (_actualAnimation is not null && texture is not null)
		{
			if (_lastIndex != _frameIndex || _lastRegion is null)
			{
				Animation.AnimationFrame? frame = _actualAnimation.GetFrame(_frameIndex);

					if (frame is not null)
					{
						// Guarda la región
						_lastRegion = texture?.GetRegion(frame.Region);
						// y el índice
						_lastIndex = _frameIndex;
					}
			}
		}
		// Devuelve la textura
		return _lastRegion;
	}

	/// <summary>
	///		Obtiene la región predeterminada
	/// </summary>
	internal string? GetDefaultRegion()
	{
		if (_actualAnimation is not null)
			return _actualAnimation.Frames[0].Region;
		else
			return "default";
	}

	/// <summary>
	///		Indica si se está ejecutando la animación
	/// </summary>
	public bool IsPlaying { get; set; }
}