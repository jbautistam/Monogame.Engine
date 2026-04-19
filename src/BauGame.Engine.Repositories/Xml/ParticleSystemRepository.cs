using Microsoft.Xna.Framework;
using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;
using Bau.BauEngine.Managers.Resources.ParticlesDefinition;
using Bau.BauEngine.Actors.ParticlesEngine.Emitters;
using Bau.BauEngine.Actors.ParticlesEngine.Modifiers;
using Bau.BauEngine.Tools.MathTools.Easing;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Repositorio del sistema de partículas
/// </summary>
internal class ParticleSystemRepository
{
	// Constantes privadas
	private const string TagRoot = "Particles";
	private const string TagSystem = "System";
	private const string TagName = "Name";
	private const string TagEmitter = "Emitter";
	private const string TagMaximumParticles = "Maximum";
	private const string TagPosition = "Position";
    private const string TagStart = "Start";
    private const string TagDuration = "Duration";
    private const string TagEmissionRate = "EmissionRate";
	private const string TagParticlesPerEmission = "ParticlesPerEmission";
    private const string TagLifetime = "LifeTime";
    private const string TagScale = "Scale";
    private const string TagRotation = "Rotation";
    private const string TagRotationSpeed = "RotationSpeed";
	private const string TagSpeed = "Speed";
    private const string TagColor = "Color";
    private const string TagOpacity = "Opacity";
	private const string TagLocation = "Location";
	private const string TagDirectionMode = "DirectionMode";
	private const string TagFixedDirection = "FixedDirection";
	private const string TagTexture = "Texture";
	private const string TagRegion = "Region";
	private const string TagAnimation = "Animation";
	private const string TagCircle = "Circle";
	private const string TagCone = "Cone";
	private const string TagLine = "Line";
	private const string TagPoint = "Point";
	private const string TagRectangle = "Rectangle";
	private const string TagPath = "Path";
	private const string TagRing = "Ring";
	private const string TagRadius = "Radius";
	private const string TagStartAngle = "StartAngle";
	private const string TagEndAngle = "EndAngle";
	private const string TagLength = "Length";
	private const string TagSpread = "Spread";
	private const string TagWidth = "Width";
	private const string TagHeight = "Height";
	private const string TagInnerRadius = "InnerRadius";
	private const string TagOuterRadius = "OuterRadius";
	private const string TagAlpha = "Alpha";
	private const string TagAttraction = "Attraction";
	private const string TagDrag = "Drag";
	private const string TagGravity = "Gravity";
	private const string TagWind = "Wind";
	private const string TagMinimum = "Minimum";
	private const string TagMaximum = "Maximum";
	private const string TagEasing = "Easing";
	private const string TagForce = "Force";
	private const string TagDistance = "Distance";
	private const string TagCoefficient = "Coefficient";
	private const string TagDirection = "Direction";
	private const string TagStrength = "Strength";
	private const string TagTurbulence = "Turbulence";
	private const string TagAge = "Age";
	// Variables privadas
	private RepositoryXmlHelper _helper = new();

	/// <summary>
	///		Carga los sistemas de partículas de un archivo XML
	/// </summary>
	internal List<ParticleSystemDefinitionModel> Load(string xml)
	{
		List<ParticleSystemDefinitionModel> systems = [];
		MLFile? fileML = new Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			// Carga los datos
			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
						foreach (MLNode nodeML in rootML.Nodes)
							if (nodeML.Name == TagSystem)
								systems.Add(LoadSystem(nodeML));
			// Devuelve los sistemas cargados
			return systems;
	}

	/// <summary>
	///		Carga los datos de un sistema
	/// </summary>
	private ParticleSystemDefinitionModel LoadSystem(MLNode rootML)
	{
		ParticleSystemDefinitionModel system = new(rootML.Attributes[TagName].Value.TrimIgnoreNull());

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
		ParticleSystemEmitterDefinitionModel emitter = new(LoadEmiterShape(rootML));

			// Carga los datos
			emitter.Position = _helper.GetVector(rootML.Attributes[TagPosition].Value.TrimIgnoreNull());
			emitter.Profile.MaximumParticles = rootML.Attributes[TagMaximumParticles].Value.GetInt(1_000);
			emitter.Profile.Start = (float) rootML.Attributes[TagStart].Value.GetDouble(0);
			emitter.Profile.Duration = (float) rootML.Attributes[TagDuration].Value.GetDouble(1);
			emitter.Profile.ParticlesPerEmission = _helper.GetIntInterval(rootML.Attributes[TagParticlesPerEmission].Value, 1);
			emitter.Profile.EmissionRate = _helper.GetFloatInterval(rootML.Attributes[TagEmissionRate].Value, 1);
			emitter.Profile.Lifetime = _helper.GetFloatInterval(rootML.Attributes[TagLifetime].Value, emitter.Profile.Duration);
			emitter.Profile.Scale = _helper.GetFloatInterval(rootML.Attributes[TagScale].Value, 1);
			emitter.Profile.Rotation = _helper.GetFloatInterval(rootML.Attributes[TagRotation].Value, 0);
			emitter.Profile.RotationSpeed = _helper.GetFloatInterval(rootML.Attributes[TagRotationSpeed].Value, 0);
			emitter.Profile.Speed = _helper.GetFloatInterval(rootML.Attributes[TagSpeed].Value, 1);
			emitter.Profile.Color = _helper.GetColorInterval(rootML.Attributes[TagColor].Value, Color.White);
			emitter.Profile.Opacity = _helper.GetFloatInterval(rootML.Attributes[TagOpacity].Value, 1);
			// Crea el sprite
			if (!string.IsNullOrWhiteSpace(rootML.Attributes[TagTexture].Value))
			{
				if (!string.IsNullOrWhiteSpace(rootML.Attributes[TagAnimation].Value))
					emitter.Profile.Sprite = new Entities.Sprites.SpriteAnimatedDefinition(rootML.Attributes[TagTexture].Value.TrimIgnoreNull(),
																						   rootML.Attributes[TagAnimation].Value.TrimIgnoreNull());
				else
					emitter.Profile.Sprite = new Entities.Sprites.SpriteDefinition(rootML.Attributes[TagTexture].Value.TrimIgnoreNull(),
																				   rootML.Attributes[TagRegion].Value.TrimIgnoreNull());
			}
			// Carga los modificadores
			emitter.Modifiers.AddRange(LoadModifiers(rootML));
			// Devuelve los datos del emisor
			return emitter;
	}

	/// <summary>
	///		Carga la figura de un emisor
	/// </summary>
	private ParticleEmitterShape LoadEmiterShape(MLNode rootML)
	{
		ParticleEmitterShape shape = new()
										{
											Shape = LoadShape(rootML)
										};

			// Asigna los datos
			shape.Location = rootML.Attributes[TagLocation].Value.GetEnum(AbstractShapeEmitter.EmissionLocationMode.Surface);
			shape.DirectionMode = rootML.Attributes[TagDirectionMode].Value.GetEnum(AbstractShapeEmitter.EmissionDirectionMode.Outward);
			shape.FixedDirection = _helper.GetVector(rootML.Attributes[TagFixedDirection].Value);
			// Devuelve la figura
			return shape;
	}

	/// <summary>
	///		Carga los datos de la figura
	/// </summary>
	private AbstractShapeEmitter LoadShape(MLNode rootML)
	{
		AbstractShapeEmitter? shape = null;

			// Obtiene la figura de los nodos
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagCircle:
							shape = new CircleShapeEmitter((float) nodeML.Attributes[TagRadius].Value.GetDouble(5));
							AssignShapeAttributes(shape, nodeML);
						break;
					case TagCone:
							shape = new ConeShapeEmitter((float) nodeML.Attributes[TagRadius].Value.GetDouble(5),
														 (float) nodeML.Attributes[TagStartAngle].Value.GetDouble(0),
														 (float) nodeML.Attributes[TagEndAngle].Value.GetDouble(360));
							AssignShapeAttributes(shape, nodeML);
						break;
					case TagLine:
							shape = new LineShapeEmitter((float) nodeML.Attributes[TagLength].Value.GetDouble(5),
														 (float) nodeML.Attributes[TagRotation].Value.GetDouble(0));
							AssignShapeAttributes(shape, nodeML);
						break;
					case TagPath:
							PathEmitter path = new();

								// Carga los puntos
								path.Points.AddRange(GetPoints(nodeML));
								// Asigna la figura
								shape = path;
								// Asigna el resto de atributos
								AssignShapeAttributes(shape, nodeML);
						break;
					case TagPoint:
							shape = new PointShapeEmitter((float) nodeML.Attributes[TagSpread].Value.GetDouble(0));
							AssignShapeAttributes(shape, nodeML);
						break;
					case TagRectangle:
							shape = new RectangleShapeEmitter((float) nodeML.Attributes[TagWidth].Value.GetDouble(5),
															  (float) nodeML.Attributes[TagHeight].Value.GetDouble(5));
							AssignShapeAttributes(shape, nodeML);
						break;
					case TagRing:
							shape = new RingShapeEmitter((float) nodeML.Attributes[TagInnerRadius].Value.GetDouble(2),
														 (float) nodeML.Attributes[TagOuterRadius].Value.GetDouble(5));
							AssignShapeAttributes(shape, nodeML);
						break;
					case TagTexture:
							shape = new TextureShapeEmitter(nodeML.Attributes[TagTexture].Value.TrimIgnoreNull(),
															nodeML.Attributes[TagRegion].Value.TrimIgnoreNull());
							AssignShapeAttributes(shape, nodeML);
						break;
				}
			// Crea la figura predeterminada
			if (shape is null)
				shape = new PointShapeEmitter();
			// Devuelve la figura
			return shape;

		// Asigna los atributos de la figura
		void AssignShapeAttributes(AbstractShapeEmitter shape, MLNode rootML)
		{
			shape.EmissionLocation = rootML.Attributes[TagLocation].Value.GetEnum(AbstractShapeEmitter.EmissionLocationMode.Border);
			shape.EmissionDirection = rootML.Attributes[TagDirectionMode].Value.GetEnum(AbstractShapeEmitter.EmissionDirectionMode.Random);
		}

		// Obtiene una lista de puntos
		List<Vector2> GetPoints(MLNode rootML)
		{
			List<Vector2> points = [];

				// Carga los datos
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagPoint)
						points.Add(_helper.GetVector(nodeML.Value));
				// Devuelve la lista de puntos
				return points;
		}
	}

	/// <summary>
	///		Carga los modificadores asociados a un emisor
	/// </summary>
	private List<AbstractParticleModifier> LoadModifiers(MLNode rootML)
	{
		List<AbstractParticleModifier> modifiers = [];

			// Carga los modificadores
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagAlpha:
							modifiers.Add(new AlphaOverLifetimeModifier(nodeML.Attributes[TagMinimum].Value.GetInt(0),
																		nodeML.Attributes[TagMaximum].Value.GetInt(1),
																		nodeML.Attributes[TagEasing].Value.GetEnum(EasingFunctionsHelper.EasingType.Linear)));
						break;
					case TagAttraction:
							modifiers.Add(new AttractionModifier(_helper.GetVector(nodeML.Attributes[TagPosition].Value),
																 (float) nodeML.Attributes[TagForce].Value.GetDouble(0),
																 (float) nodeML.Attributes[TagDistance].Value.GetDouble(1)));
						break;
					case TagColor:
							ColorOverLifetimeModifier modifier = new(nodeML.Attributes[TagEasing].Value.GetEnum(EasingFunctionsHelper.EasingType.Linear));

								// Añade los colores
								foreach (MLNode childML in nodeML.Nodes)
									if (childML.Name == TagColor)
										modifier.Colors.Add(((float) childML.Attributes[TagAge].Value.GetDouble(1),
															 _helper.GetColor(childML.Attributes[TagColor].Value, Color.White)));
								// Añade el modificador
								modifiers.Add(modifier);
						break;
					case TagDrag:
							modifiers.Add(new DragModifier((float) nodeML.Attributes[TagCoefficient].Value.GetDouble(0.95)));
						break;
					case TagGravity:
							modifiers.Add(new GravityModifier((float) nodeML.Attributes[TagForce].Value.GetDouble(9.8)));
						break;
					case TagRotation:
							modifiers.Add(new RotationOverLifetimeModifier((float) nodeML.Attributes[TagRotationSpeed].Value.GetDouble(1)));
						break;
					case TagScale:
							modifiers.Add(new ScaleOverLifetimeModifier(nodeML.Attributes[TagMinimum].Value.GetInt(0),
																		nodeML.Attributes[TagMaximum].Value.GetInt(1),
																		nodeML.Attributes[TagEasing].Value.GetEnum(EasingFunctionsHelper.EasingType.Linear)));
						break;
					case TagWind:
							modifiers.Add(new WindModifier(_helper.GetVector(nodeML.Attributes[TagDirection].Value, Vector2.Zero),
														   (float) nodeML.Attributes[TagStrength].Value.GetDouble(0),
														   (float) nodeML.Attributes[TagTurbulence].Value.GetDouble(0)));
						break;
				}
			// Devuelve el modificador
			return modifiers;
	}
}