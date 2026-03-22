namespace Bau.BauEngine.Managers.Resources.ParticlesDefinition;

/// <summary>
///		Definición de un sistema de partículas
/// </summary>
public class ParticleSystemDefinitionModel(string name)
{
	/// <summary>
	///		Nombre del sistema de partículas
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Definiciones de los emisores
	/// </summary>
	public List<ParticleSystemEmitterDefinitionModel> Emmiters { get; } = [];
}