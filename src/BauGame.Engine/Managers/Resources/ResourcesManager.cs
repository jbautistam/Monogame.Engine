namespace Bau.BauEngine.Managers.Resources;

/// <summary>
///		Manager para la definición de recursos
/// </summary>
public class ResourcesManager
{
	public ResourcesManager(EngineManager engineManager)
	{
		EngineManager = engineManager;
		TextureManager = new TextureManager(this);
		TextureConfigurationManager = new TextureConfigurationManager(this);
		GlobalContentManager = new ContentDisposableManager(this);
		AnimationManager = new Animations.AnimationManager(this);
        ParticlesResourcesManagers = new ParticlesResourceManager(this);
	}

	/// <summary>
	///		Manager principal del motor
	/// </summary>
	public EngineManager EngineManager { get; }

	/// <summary>
	///		Manager de contenido global
	/// </summary>
	public ContentDisposableManager GlobalContentManager { get; }

	/// <summary>
	///		Manager de definiciones de texturas
	/// </summary>
	public TextureManager TextureManager { get; }

	/// <summary>
	///		Manager para las configuraciones de texturas
	/// </summary>
	public TextureConfigurationManager TextureConfigurationManager { get; }

	/// <summary>
	///		Manager para las definiciones de animaciones
	/// </summary>
	public Animations.AnimationManager AnimationManager { get; }

    /// <summary>
    ///     Manager con las definiciones de los sistemas de partículas
    /// </summary>
    public ParticlesResourceManager ParticlesResourcesManagers { get; }
}