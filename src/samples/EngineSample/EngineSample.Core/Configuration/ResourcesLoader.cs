using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Managers;

namespace EngineSample.Core.Configuration;

/// <summary>
///		Clases de carga de configuración
/// </summary>
public class ResourcesLoader(EngineManager manager)
{
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
		LoadTexturesSettings("Settings/textures-common.xml");
		LoadTexturesSettings("Settings/textures-graphics-novel.xml");
		LoadTexturesSettings("Settings/textures-sample.xml");
		LoadTexturesSettings("Settings/textures-space.xml");
		LoadTexturesSettings("Settings/textures-tiles.xml");
	}

	/// <summary>
	///		Carga las definiciones de los sistemas de partículas
	/// </summary>
	private void LoadParticlesSystem()
	{
		string? xml = ReadFile("Settings/Particles.xml");
			
			if (!string.IsNullOrWhiteSpace(xml))
				Manager.ResourcesManager.ParticlesResourcesManagers.AddRange(new Repositories.ParticleSystemRepository().Load(xml));
	}

	/// <summary>
	///		Carga los estilos
	/// </summary>
	public UiStylesCollection LoadStyles(AbstractUserInterfaceLayer layer, string fileName)
	{
		string? xml = ReadFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				return new Repositories.StylesRepository().Load(layer, xml);
			else
				return new UiStylesCollection(layer);
	}

	/// <summary>
	///		Carga los datos de una pantalla
	/// </summary>
	public List<UiElement> LoadScreen(AbstractUserInterfaceLayer layer, string fileName)
	{
		string? xml = ReadFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				return new Repositories.UserInterfaceRepository().Load(layer, xml);
			else
				return new List<UiElement>();
	}

	/// <summary>
	///		Carga la configuración de texturas y animación de un archivo
	/// </summary>
	private void LoadTexturesSettings(string fileName)
	{
		string? xml = ReadFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				new Repositories.TexturesRepository().Load(xml, Manager.ResourcesManager);
	}

	/// <summary>
	///		Carga la configuración de texturas y animación de un archivo
	/// </summary>
	private string? ReadFile(string fileName) => Manager.FilesManager.StorageManager.ReadTextFile($"Content/{fileName}");

	/// <summary>
	///		Manager principal del motor
	/// </summary>
	public EngineManager Manager { get; } = manager;
}
