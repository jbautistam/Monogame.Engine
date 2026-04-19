using Bau.BauEngine.Managers;
using Bau.BauEngine.Managers.Resources;
using Bau.BauEngine.Managers.Resources.Animations;
using Bau.BauEngine.Tools.Extensors;

namespace Bau.BauEngine.Entities.Sprites;

/// <summary>
///		Definición de textura
/// </summary>
public class SpriteAnimatedDefinition : AbstractSpriteDefinition
{
	// Variables privadas
	private string _animation = string.Empty;
	private int _frameIndex;
	private TextureConfigurationManager.TextureResolved? _textureConfiguration;
	private Animation? _actualAnimation;
	private Animation.AnimationFrame? _frame;
	private bool _isPlaying = true;
	private float _elapsed;

	public SpriteAnimatedDefinition(string asset, string animation) : base(asset, null)
	{
		Animation = animation;
	}

	/// <summary>
	///		Clona el objeto
	/// </summary>
	public override AbstractSpriteDefinition Clone() => new SpriteAnimatedDefinition(Asset, Animation);

	/// <summary>
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	public override TextureConfigurationManager.TextureResolved? LoadAsset(Scenes.AbstractScene scene)
	{
		// Carga el asset si no estaba ya en memoria o se ha modificado
		if (IsDirty)
		{
			// Obtiene la animación actual
			_actualAnimation = scene.SceneManager.EngineManager.ResourcesManager.AnimationManager.Animations.Get(Animation);
			// Marca los datos de inicio
			_frameIndex = 0;
			_elapsed = 0;
			// Indica que se ha cargado con las últimas modificaciones
			IsDirty = false;
		}
		// Obtiene la configuración de la región
		return GetAnimationTexture(scene);
	}

	/// <summary>
	///		Obtiene la textura / región de la animación
	/// </summary>
	private TextureConfigurationManager.TextureResolved? GetAnimationTexture(Scenes.AbstractScene scene)
	{
		// Obtiene la región de la textura animada
		if (_actualAnimation is not null && _frame is not null)
			_textureConfiguration = scene.SceneManager.EngineManager.ResourcesManager.TextureConfigurationManager.GetTextureRegion(scene, Asset, _frame.Region);
		else
			_textureConfiguration = null;
		// Devuelve la configuración
		return _textureConfiguration;
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public override Common.Size GetSize()
	{
		if (_textureConfiguration is null)
			return new Common.Size(0, 0);
		else
			return new Common.Size(_textureConfiguration.Region.Width, _textureConfiguration.Region.Height);
	}

	/// <summary>
	///		Actualiza los datos del sprite
	/// </summary>
	public override void Update(GameContext gameContext)
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
				if (_elapsed >= _frame.Time)
				{
					// Incrementa el índice
					_frameIndex++;
					// Pasa al inicio
					if (_frameIndex == _actualAnimation.Frames.Count)
					{
						// Si no se reproduce en loop, se detiene
						if (!Loop)
						{
							_frameIndex = _actualAnimation.Frames.Count - 1;
							IsPlaying = false;
							HasEndLoop = true;
						}
						else // si estamos en loop pasamos al primer frame
							_frameIndex = 0;
					}
					// Cambia el frame
					_frame = _actualAnimation.GetFrame(_frameIndex);
					_elapsed = 0;
				}
			}
		}
	}

	/// <summary>
	///		Reinicia la animación
	/// </summary>
	public void Reset()
	{
		_actualAnimation = null;
		_frameIndex = 0;
	}

	/// <summary>
	///		Animación
	/// </summary>
	public string Animation 
	{ 
		get { return _animation; } 
		set
		{
			if (!_animation.EqualsIgnoreNull(value, StringComparison.CurrentCultureIgnoreCase))
			{
				_animation = value;
				IsDirty = true;
			}
		}
	}

	/// <summary>
	///		Indica si se está ejecutando la animación
	/// </summary>
	public bool IsPlaying 
	{ 
		get { return _isPlaying; }
		set 
		{
			_isPlaying = value;
			if (_isPlaying)
				HasEndLoop = false;
		}
	}

	/// <summary>
	///		Indica si se está animando en bucle
	/// </summary>
	public bool Loop { get; set; } = true;

	/// <summary>
	///		Indica si ha terminado el bucle de animación
	/// </summary>
	public bool HasEndLoop { get; private set; }
}