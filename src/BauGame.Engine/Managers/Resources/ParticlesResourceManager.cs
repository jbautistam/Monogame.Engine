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
	public void Create(AbstractLayer layer, string name, Vector2 position)
	{
		if (Definitions.TryGetValue(name, out ParticleSystemDefinitionModel? system))
			Create(layer, system, position);
	}

	/// <summary>
	///		Crea un sistema de partículas sobre la capa
	/// </summary>
	public void Create(AbstractLayer layer, ParticleSystemDefinitionModel system, Vector2 position)
	{
		ParticleEngineActor engine = new(layer, position, null);

			// Asigna las emisores
			foreach (ParticleSystemEmitterDefinitionModel emitter in system.Emmiters)
				engine.Emitters.Add(new ParticleEmitter (engine, position, CreateProfile(emitter)));
			// Añade el motor a la capa
			if (engine.Emitters.Count > 0)
				layer.Actors.Add(engine);
	}

	/// <summary>
	///		Crea un perfil de emisión de partículas
	/// </summary>
	private ParticleEmitterProfile CreateProfile(ParticleSystemEmitterDefinitionModel system)
	{
		ParticleEmitterProfile profile = new()
											{
												Shape = CreateShape(system)
											};

			// Asigna las propiedades
			profile.MaximumParticles = system.MaximumParticles;
			profile.Start = system.Start;
			profile.Duration = system.Duration;
			profile.EmissionRate = system.EmissionRate;
			profile.Lifetime = system.Lifetime;
			profile.StartScale = system.StartScale;
			profile.EndScale = system.EndScale;
			profile.Rotation = system.Rotation;
			profile.RotationSpeed = system.RotationSpeed;
			profile.Speed = system.Speed;
			profile.Sprite = system.Sprite;
			profile.Color = system.Color;
			profile.Opacity = system.Opacity;
			profile.Location = system.Location;
			profile.DirectionMode = system.DirectionMode;
			profile.FixedDirection = system.FixedDirection;
			// Asigna los modificadores
			foreach (ParticleSystemModifierDefinitionModel modifier in system.Modifiers)
				profile.Modifiers.Add(CreateModifier(modifier));
			// Devuelve el perfil
			return profile;
	}

	/// <summary>
	///		Genera una figura para el emisor
	/// </summary>
	private AbstractShapeEmitter CreateShape(ParticleSystemEmitterDefinitionModel emitter)
	{
		return emitter.Shape switch
					{
						ParticleSystemEmitterDefinitionModel.EmitterShapeType.Circle => CreateShapeCircle(emitter.Parameters),
						ParticleSystemEmitterDefinitionModel.EmitterShapeType.Cone => CreateShapeCone(emitter.Parameters),
						ParticleSystemEmitterDefinitionModel.EmitterShapeType.Line => CreateShapeLine(emitter.Parameters),
						ParticleSystemEmitterDefinitionModel.EmitterShapeType.Path => CreateShapePath(emitter.Parameters),
						ParticleSystemEmitterDefinitionModel.EmitterShapeType.Rectangle => CreateShapeRectangle(emitter.Parameters),
						ParticleSystemEmitterDefinitionModel.EmitterShapeType.Ring => CreateShapeRing(emitter.Parameters),
						//ParticleSystemEmitterDefinitionModel.EmitterShapeType.Texture => CreateShapeTexture(emitter.Parameters),
						_ => CreateShapePoint(emitter.Parameters)
					};
	}

	/// <summary>
	///		Crea una figura de tipo punto
	/// </summary>
	private AbstractShapeEmitter CreateShapePoint(Dictionary<string, object> parameters)
	{
		return new PointShapeEmitter(GetFloat("Spread", parameters, 0));
	}

	///// <summary>
	/////		Crea una figura de tipo textura
	///// </summary>
	//private AbstractShapeEmitter CreateShapeTexture(Dictionary<string, object> parameters)
	//{
	//	return new TextureShapeEmitter(GetString("Teture", parameters, string.Empty));
	//}

	/// <summary>
	///		Crea una figura de tipo anillo
	/// </summary>
	private AbstractShapeEmitter CreateShapeRing(Dictionary<string, object> parameters)
	{
		return new RingShapeEmitter(GetFloat("InnerRadius", parameters, 0.5f),
									GetFloat("OuterRadius", parameters, 1));
	}

	/// <summary>
	///		Crea una figura de tipo rectángulo
	/// </summary>
	private AbstractShapeEmitter CreateShapeRectangle(Dictionary<string, object> parameters)
	{
		return new RectangleShapeEmitter(GetFloat("Width", parameters, 1),
										 GetFloat("Height", parameters, 1));
	}

	/// <summary>
	///		Crea una figura de tipo ruta
	/// </summary>
	private AbstractShapeEmitter CreateShapePath(Dictionary<string, object> parameters)
	{
	throw new NotImplementedException();
	}

	/// <summary>
	///		Crea una figura de tipo línea
	/// </summary>
	private AbstractShapeEmitter CreateShapeLine(Dictionary<string, object> parameters)
	{
		return new LineShapeEmitter(GetFloat("Length", parameters, 1),
									GetFloat("Rotation", parameters, 0));
	}

	/// <summary>
	///		Crea una figura de tipo cono
	/// </summary>
	private AbstractShapeEmitter CreateShapeCone(Dictionary<string, object> parameters)
	{
		return new ConeShapeEmitter(GetFloat("Radius", parameters, 1),
									GetFloat("Start", parameters, 0),
									GetFloat("End", parameters, 1));
	}

	/// <summary>
	///		Crea una figura de tipo círculo
	/// </summary>
	private AbstractShapeEmitter CreateShapeCircle(Dictionary<string, object> parameters)
	{
		return new CircleShapeEmitter(GetFloat("Radius", parameters, 1));
	}

	/// <summary>
	///		Genera un modificador para el perfil
	/// </summary>
	private AbstractParticleModifier CreateModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return modifier.Type switch
				{
					ParticleSystemModifierDefinitionModel.ModifierType.Attraction => CreateAttractionModifier(modifier),
					ParticleSystemModifierDefinitionModel.ModifierType.Color => CreateColorModifier(modifier),
					ParticleSystemModifierDefinitionModel.ModifierType.Drag => CreateDragModifier(modifier),
					ParticleSystemModifierDefinitionModel.ModifierType.Gravity => CreateGravityModifier(modifier),
					ParticleSystemModifierDefinitionModel.ModifierType.Rotation => CreateRotationModifier(modifier),
					ParticleSystemModifierDefinitionModel.ModifierType.Wind => CreateWindModifier(modifier),
					_ => CreateOpacityModifier(modifier)
				};
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	/// </summary>
	private AbstractParticleModifier CreateOpacityModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new AlphaOverLifetimeModifier(GetInt("Minimum", modifier.Parameters, 0),
											 GetInt("Maximum", modifier.Parameters, 1),
											 EasingFunctionsHelper.EasingType.Linear);
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	/// </summary>
	private AbstractParticleModifier CreateWindModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new WindModifier(GetVector("Direction", modifier.Parameters, Vector2.Zero),
								GetFloat("Strength", modifier.Parameters, 1),
								GetFloat("Turbulence", modifier.Parameters, 0));
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	/// </summary>
	private AbstractParticleModifier CreateRotationModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new RotationOverLifetimeModifier(GetFloat("Speed", modifier.Parameters, 1));
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	/// </summary>
	private AbstractParticleModifier CreateGravityModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new GravityModifier(GetFloat("Gravity", modifier.Parameters, 9.8f));
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	/// </summary>
	private AbstractParticleModifier CreateDragModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new DragModifier(GetFloat("Drag", modifier.Parameters, 0.95f));
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	///		TODO: falta la configuración de la lista de colores y edad (<see cref="ColorOverLifetimeModifier.Colors"/>)
	/// </summary>
	private AbstractParticleModifier CreateColorModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new ColorOverLifetimeModifier(GetString("Easing", modifier.Parameters, string.Empty).GetEnum(EasingFunctionsHelper.EasingType.Linear));
	}

	/// <summary>
	///		Crea un modificador de opacidad de la partícula
	/// </summary>
	private AbstractParticleModifier CreateAttractionModifier(ParticleSystemModifierDefinitionModel modifier)
	{
		return new AttractionModifier(GetVector("Position", modifier.Parameters, Vector2.Zero),
									  GetFloat("Force", modifier.Parameters, 1),
									  GetFloat("MinDistance", modifier.Parameters, 0));
	}

	/// <summary>
	///		Obtiene un valor entero de la lista de valores
	/// </summary>
	private int GetInt(string key, Dictionary<string, object> parameters, int defaultValue)
	{
		// Obtiene el valor de la lista
		if (parameters.TryGetValue(key, out object? value) && value is int integerValue)
			return integerValue;
		// Devuelve el valor predeterminado si ha llegado hasta aquí
		return defaultValue;
	}

	/// <summary>
	///		Obtiene una cadena de la lista de valores
	/// </summary>
	private string GetString(string key, Dictionary<string, object> parameters, string defaultValue)
	{
		// Obtiene el valor de la lista
		if (parameters.TryGetValue(key, out object? value) && value is string stringValue)
			return stringValue;
		// Devuelve el valor predeterminado si ha llegado hasta aquí
		return defaultValue;
	}

	/// <summary>
	///		Obtiene un valor decimal de la lista de valores
	/// </summary>
	private float GetFloat(string key, Dictionary<string, object> parameters, float defaultValue)
	{
		// Obtiene el valor de la lista
		if (parameters.TryGetValue(key, out object? value) && value is float floatValue)
			return floatValue;
		// Devuelve el valor predeterminado si ha llegado hasta aquí
		return defaultValue;
	}

	/// <summary>
	///		Obtiene un color de la lista de valores
	/// </summary>
	private Color GetColor(string key, Dictionary<string, object> parameters, Color defaultValue)
	{
		// Obtiene el valor de la lista
		if (parameters.TryGetValue(key, out object? value) && value is Color colorValue)
			return colorValue;
		// Devuelve el valor predeterminado si ha llegado hasta aquí
		return defaultValue;
	}

	/// <summary>
	///		Obtiene un vector de la lista de valores
	/// </summary>
	private Vector2 GetVector(string key, Dictionary<string, object> parameters, Vector2 defaultValue)
	{
		// Obtiene el valor de la lista
		if (parameters.TryGetValue(key, out object? value) && value is Vector2 vectorValue)
			return vectorValue;
		// Devuelve el valor predeterminado si ha llegado hasta aquí
		return defaultValue;
	}

	/// <summary>
	///		Diccionario de definiciones
	/// </summary>
	private Dictionary<string, ParticleSystemDefinitionModel> Definitions { get; } = new(StringComparer.CurrentCultureIgnoreCase);

	/// <summary>
	///		Manager de recursos
	/// </summary>
	public ResourcesManager ResourcesManager { get; } = resourcesManager;
}