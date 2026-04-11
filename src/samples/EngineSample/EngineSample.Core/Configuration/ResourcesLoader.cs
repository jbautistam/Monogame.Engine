using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Managers;

namespace EngineSample.Core.Configuration;

/// <summary>
///		Clases de carga de configuración
/// </summary>
public class ResourcesLoader
{
	// Variables privadas
	private Bau.BauEngine.Repositories.FilesStandardLoader _filesLoader;

	public ResourcesLoader(EngineManager manager)
	{
		Manager = manager;
		_filesLoader = new Bau.BauEngine.Repositories.FilesStandardLoader(manager);
	}

	/// <summary>
	///		Carga la configuración
	/// </summary>
	public void Load()
	{
		LoadTexturesSettings();
		LoadParticlesSystem();
	}

	/// <summary>
	///		Carga las configuraciones de texturas y animación
	/// </summary>
	private void LoadTexturesSettings()
	{
		_filesLoader.LoadTexturesSettings("Settings/textures-common.xml");
		_filesLoader.LoadTexturesSettings("Settings/textures-graphics-novel.xml");
		_filesLoader.LoadTexturesSettings("Settings/textures-sample.xml");
		_filesLoader.LoadTexturesSettings("Settings/textures-space.xml");
		_filesLoader.LoadTexturesSettings("Settings/textures-tiles.xml");
	}

	/// <summary>
	///		Carga las definiciones de los sistemas de partículas
	/// </summary>
	private void LoadParticlesSystem()
	{
		_filesLoader.LoadParticlesSystem("Settings/Particles.xml");
	}

	/// <summary>
	///		Carga los estilos
	/// </summary>
	public UiStylesCollection LoadStyles(AbstractUserInterfaceLayer layer, string fileName)
	{
		return _filesLoader.LoadStyles(layer, fileName);
	}

	/// <summary>
	///		Carga los datos de una pantalla
	/// </summary>
	public (UiStylesCollection styles, List<UiElement> components) LoadScreen(AbstractUserInterfaceLayer layer, string fileName)
	{
		return _filesLoader.LoadScreen(layer, fileName);
	}

	/// <summary>
	///		Manager principal del motor
	/// </summary>
	public EngineManager Manager { get; }
}
