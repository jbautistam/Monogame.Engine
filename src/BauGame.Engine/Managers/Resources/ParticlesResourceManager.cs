using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors.ParticlesEngine;
using Bau.BauEngine.Actors.ParticlesEngine.Emitters;
using Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;
using Bau.BauEngine.Actors.ParticlesEngine.Modifiers;
using Bau.BauEngine.Managers.Resources.ParticlesDefinition;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Tools.Extensors;
using Bau.BauEngine.Tools.MathTools.Easing;

namespace Bau.BauEngine.Managers.Resources;

/// <summary>
///		Manager de administrador de partículas
/// </summary>
public class ParticlesResourceManager(ResourcesManager resourcesManager)
{
	/// <summary>
	///		Añade una serie de definiciones de sistemas de partículas
	/// </summary>
	public void AddRange(List<ParticleSystemDefinitionModel> particleSystems)
	{
		foreach (ParticleSystemDefinitionModel particleSystem in particleSystems)
			if (particleSystem.Emmiters.Count > 0)
				Add(particleSystem.Name, particleSystem);
	}

	/// <summary>
	///		Añade un sistema de partículas
	/// </summary>
	public void Add(string name, ParticleSystemDefinitionModel system)
	{
		if (Definitions.ContainsKey(name))
			Definitions[name] = system;
		else
			Definitions.Add(name, system);
	}

	/// <summary>
	///		Crea un sistema de partículas sobre la capa
	/// </summary>
	public ParticleEngineActor? Create(AbstractLayer layer, string name, Vector2 position, int? zOrder)
	{
		if (Definitions.TryGetValue(name, out ParticleSystemDefinitionModel? system))
			return Create(layer, system, position, zOrder);
		else
			return null;
	}

	/// <summary>
	///		Crea un sistema de partículas sobre la capa
	/// </summary>
	public ParticleEngineActor Create(AbstractLayer layer, ParticleSystemDefinitionModel system, Vector2 position, int? zOrder)
	{
		ParticleEngineActor engine = new(layer, position, zOrder);

			// Asigna las emisores
			foreach (ParticleSystemEmitterDefinitionModel emitter in system.Emmiters)
			{
				ParticleEmitter translated = new(engine, position, CreateShape(emitter), CreateProfile(emitter));

					// Añade los modificadores
					foreach (AbstractParticleModifier modifier in emitter.Modifiers)
						translated.Modifiers.Add(modifier.Clone());
					// Añade el emisor
					engine.Emitters.Add(translated);
			}
			// Añade el motor a la capa
			if (engine.Emitters.Count > 0)
				layer.Actors.Add(engine);
			// Devuelve el actor creado
			return engine;
	}

	/// <summary>
	///		Crea la figura del emisor
	/// </summary>
	private ParticleEmitterShape CreateShape(ParticleSystemEmitterDefinitionModel emitter) => emitter.Shape.Clone();

	/// <summary>
	///		Crea un perfil de emisión de partículas
	/// </summary>
	private ParticleEmitterProfile CreateProfile(ParticleSystemEmitterDefinitionModel emitter) => emitter.Profile.Clone();

	/// <summary>
	///		Diccionario de definiciones
	/// </summary>
	private Dictionary<string, ParticleSystemDefinitionModel> Definitions { get; } = new(StringComparer.CurrentCultureIgnoreCase);

	/// <summary>
	///		Manager de recursos
	/// </summary>
	public ResourcesManager ResourcesManager { get; } = resourcesManager;
}