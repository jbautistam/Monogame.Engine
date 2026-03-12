using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Microsoft.Xna.Framework;
using UI.CharactersEngine.ParticlesEngine.Intervals;

namespace UI.ParticlesDefinition.Repositories;

/// <summary>
///		Repositorio del sistema de partículas
/// </summary>
internal class ParticleSystemRepository
{
	// Constantes privadas
	private const string TagRoot = "Particles";
	private const string TagSystem = "System";
	private const string TagName = "Name";
	private const string TagType = "Type";
	private const string TagEmitter = "Emitter";
	private const string TagMaximumParticles = "Maximum";
	private const string TagPosition = "Position";
    private const string TagStart = "Start";
    private const string TagDuration = "Duration";
    private const string TagEmissionRateMin = "EmissionRateMin";
	private const string TagEmissionRateMax = "EmissionRateMax";
    private const string TagLifetimeMin = "LifeTimeMin";
	private const string TagLifetimeMax = "LifeTimeMax";
    private const string TagStartScaleMin = "StartScaleMin";
	private const string TagStartScaleMax = "StartScaleMax";
    private const string TagEndScaleMin = "EndScaleMin";
	private const string TagEndScaleMax = "EndScaleMax";
    private const string TagRotationMin = "RotationMin";
	private const string TagRotationMax = "RotationMax";
    private const string TagRotationSpeedMin = "RotationSpeedMin";
	private const string TagRotationSpeedMax = "RotationSpeedMax";
	private const string TagSpeedMin = "SpeedMin";
	private const string TagSpeedMax = "SpeedMax";
    private const string TagColorMin = "ColorMin";
	private const string TagColorMax = "ColorMax";
	private const string TagLocation = "Location";
	private const string TagDirectionMode = "DirectionMode";
	private const string TagFixedDirection = "FixedDirection";
	private const string TagParameter = "Parameter";
	private const string TagValue = "Value";
	private const string TagModifier = "Modifier";
	// Tipo de parámetro
	private enum ParameterType
	{
		String,
		Decimal,
		Color,
		Vector
	}

    /// <summary>
    ///     Parámetros del emisor
    /// </summary>
    public Dictionary<string, object> Parameters { get; } = new(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    ///     Modificadores
    /// </summary>
    public List<ParticleSystemModifierDefinitionModel> Modifiers { get; } = [];


	/// <summary>
	///		Carga los sistemas de partículas de un archivo XML
	/// </summary>
	internal ParticleSystemDefinitionCollection Load(string xml)
	{
		ParticleSystemDefinitionCollection systems = new();
		MLFile? fileML = new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			// Carga los datos
			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
						foreach (MLNode nodeML in rootML.Nodes)
							if (nodeML.Name == TagSystem)
								systems.Add(nodeML.Attributes[TagName].Value.TrimIgnoreNull(),
											LoadSystem(nodeML));
			// Devuelve los sistemas
			return systems;
	}

	/// <summary>
	///		Carga los datos de un sistema
	/// </summary>
	private ParticleSystemDefinitionModel LoadSystem(MLNode rootML)
	{
		ParticleSystemDefinitionModel system = new();

			// Carga los emisores
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagEmitter)
					system.Emmiters.Add(LoadEmitter(nodeML));
			// Devuelve el sistema
			return system;
	}

	/// <summary>
	///		Carga los datos de un emisor
	/// </summary>
	private ParticleSystemEmitterDefinitionModel LoadEmitter(MLNode rootML)
	{
		ParticleSystemEmitterDefinitionModel emitter = new(rootML.Attributes[TagType].Value.GetEnum(ParticleSystemEmitterDefinitionModel.EmitterShapeType.Point));

			// Carga los datos
			emitter.Position = GetVector(rootML.Attributes[TagPosition].Value.TrimIgnoreNull());
			emitter.MaximumParticles = rootML.Attributes[TagMaximumParticles].Value.GetInt(1_000);
			emitter.Start = (float) rootML.Attributes[TagStart].Value.GetDouble(0);
			emitter.Duration = (float) rootML.Attributes[TagDuration].Value.GetDouble(1);
			emitter.EmissionRate = GetFloatInterval(rootML, TagEmissionRateMin, TagEmissionRateMax, 1);
			emitter.Lifetime = GetFloatInterval(rootML, TagLifetimeMin, TagLifetimeMax, 1);
			emitter.StartScale = GetFloatInterval(rootML, TagStartScaleMin, TagStartScaleMax, 1);
			emitter.EndScale = GetFloatInterval(rootML, TagEndScaleMin, TagEndScaleMax, 1);
			emitter.Rotation = GetFloatInterval(rootML, TagRotationMin, TagRotationMax, 0);
			emitter.RotationSpeed = GetFloatInterval(rootML, TagRotationSpeedMin, TagRotationSpeedMax, 0);
			emitter.Speed = GetFloatInterval(rootML, TagSpeedMin, TagSpeedMax, 1);
			emitter.Color = GetColorInterval(rootML, TagColorMin, TagColorMax, Color.White);
			emitter.Location = rootML.Attributes[TagLocation].Value.GetEnum(CharactersEngine.ParticlesEngine.Shapes.AbstractShapeEmitter.EmissionLocation.Surface);
			emitter.DirectionMode = rootML.Attributes[TagDirectionMode].Value.GetEnum(CharactersEngine.ParticlesEngine.Shapes.AbstractShapeEmitter.EmissionDirectionMode.Outward);
			emitter.FixedDirection = GetVector(rootML.Attributes[TagFixedDirection].Value);
			// Carga los parámetros
			LoadParameters(rootML, emitter.Parameters);
			// Carga los modificadores
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagModifier)
					emitter.Modifiers.Add(LoadModifier(nodeML));
			// Devuelve los datos del emisor
			return emitter;
	}

	/// <summary>
	///		Carga los datos de un modificador
	/// </summary>
	private ParticleSystemModifierDefinitionModel LoadModifier(MLNode rootML)
	{
		ParticleSystemModifierDefinitionModel modifier = new(rootML.Attributes[TagType].Value.GetEnum(ParticleSystemModifierDefinitionModel.ModifierType.Opacity));

			// Carga los parámetros
			LoadParameters(rootML, modifier.Parameters);
			// Devuelve el modificador
			return modifier;
	}

	/// <summary>
	///		Obtiene un intervalo de valores decimales
	/// </summary>
	private FloatRange GetFloatInterval(MLNode rootML, string tagMin, string tagMax, float defaultValue)
	{
		double? min = rootML.Attributes[tagMin].Value.GetDouble();
		double? max = rootML.Attributes[tagMax].Value.GetDouble();

			if (min is null && max is not null)
				return new FloatRange((float) max, (float) max);
			else if (min is not null && max is null)
				return new FloatRange((float) min, (float) min);
			else if (min is not null && max is not null)
				return new FloatRange((float) min, (float) max);
			else
				return new FloatRange(defaultValue, defaultValue);
	}

	/// <summary>
	///		Obtiene un intervalo de colores
	/// </summary>
	private ColorRange GetColorInterval(MLNode rootML, string tagMin, string tagMax, Color defaultColor)
	{
		return new ColorRange(defaultColor, defaultColor);
	}

	/// <summary>
	///		Carga el diccionario de parámetros
	/// </summary>
	private void LoadParameters(MLNode rootML, Dictionary<string, object> parameters)
	{
		foreach (MLNode nodeML in rootML.Nodes)
			if (nodeML.Name == TagParameter)
			{
				string name = rootML.Attributes[TagName].Value.TrimIgnoreNull();

					if (!string.IsNullOrWhiteSpace(name))
					{
						object? value = Convert(rootML.Attributes[TagType].Value.GetEnum(ParameterType.String),
												rootML.Attributes[TagValue].Value.TrimIgnoreNull());

							if (value is not null)
							{
								if (parameters.ContainsKey(name))
									parameters[name] = value;
								else
									parameters.Add(name, value);
							}
					}
			}

		// Convierte una cadena a un objeto
		object? Convert(ParameterType type, string value)
		{
			return type switch
					{
						ParameterType.Decimal => (float?) value.GetDouble(),
						ParameterType.Color => GetColor(value),
						ParameterType.Vector => GetVector(value),
						_ => value
					};
		}
	}

	/// <summary>
	///		Obtiene un vector de una cadena
	/// </summary>
	private Vector2 GetVector(string value) => Vector2.Zero;

	/// <summary>
	///		Obtiene un color de una cadena
	/// </summary>
	private Color GetColor(string value) => Color.White;
}
