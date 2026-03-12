namespace EngineSample.Core.GameLogic.Actors.Entities.ParticlesDefinition;

/// <summary>
///		Definición de un sistema de partículas
/// </summary>
public class ParticleSystemDefinitionModel
{
	/// <summary>
	///		Definiciones de los emisores
	/// </summary>
	public List<ParticleSystemEmitterDefinitionModel> Emmiters { get; } = [];
}