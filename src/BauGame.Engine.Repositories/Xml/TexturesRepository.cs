using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.BauEngine.Managers.Resources;
using Bau.BauEngine.Managers.Resources.Animations;
using Microsoft.Xna.Framework;
using Bau.BauEngine.Managers.Resources.Textures.Configuration;
using Bau.BauEngine.Entities.UserInterface;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Repositorio de configuraciones de texturas y animaciones
/// </summary>
internal class TexturesRepository
{
	// Constantes privadas
	private const string TagRoot = "SpriteSheets";
	private const string TagTexture = "Texture";
	private const string TagAnimation = "Animation";
	private const string TagName = "Name";
	private const string TagAsset = "Asset";
	private const string TagRows = "Rows";
	private const string TagColumns = "Columns";
	private const string TagFrame = "Frame";
	private const string TagRegion = "Region";
	private const string TagTime = "Time";
	private const string TagAnimationGroup = "AnimationGroup";
	private const string TagLoop = "Loop";
	private const string TagRule = "Rule";
	private const string TagProperty = "Property";
	private const string TagCondition = "Condition";
	private const string TagValue = "Value";
	private const string TagRectangle = "Rectangle";
	private const string TagPadding = "Padding";
	private const string TagLeft = "Left";
	private const string TagTop = "Top";
	private const string TagWidth = "Width";
	private const string TagHeight = "Height";
	private const string TagTopLeftWidth = "TopLeftWidth";
	private const string TagTopLeftHeight = "TopLeftHeight";
	private const string TagTopRightWidth = "TopRightWidth";
	private const string TagTopRightHeight = "TopRightHeight";
	private const string TagBottomLeftWidth = "BottomLeftWidth";
	private const string TagBottomLeftHeight = "BottomLeftHeight";
	private const string TagBottomRightWidth = "BottomRightWidth";
	private const string TagBottomRightHeight = "BottomRightHeight";
	private const string TagFillBackground = "FillBackground";
	private const string TagBackgroundColor = "BackgroundColor";
	private const string TagBackgroundOpacity = "BackgroundOpacity";

	// Variables privadas
	private RepositoryXmlHelper _helper = new();

	/// <summary>
	///		Carga texturas y animaciones de un texto XML
	/// </summary>
	internal void Load(string xml, ResourcesManager resourcesManager)
	{
		MLFile fileML = new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
						foreach (MLNode nodeML in rootML.Nodes)
							switch (nodeML.Name)
							{
								case TagTexture:
										//LoadTexture(nodeML, resourcesManager.TextureManager);
										LoadTexture(nodeML, resourcesManager.TextureConfigurationManager);
									break;
								case TagAnimation:
										LoadAnimation(nodeML, resourcesManager.AnimationManager);
									break;
								case TagAnimationGroup:
										LoadAnimationGroup(nodeML, resourcesManager.AnimationManager);
									break;
							}
	}

	/// <summary>
	///		Carga una textura
	/// </summary>
	private void LoadTexture(MLNode rootML, TextureConfigurationManager texturesManager)
	{
		string name = rootML.Attributes[TagName].Value.TrimIgnoreNull();
		string asset = rootML.Attributes[TagAsset].Value.TrimIgnoreNull();
		int rows = rootML.Attributes[TagRows].Value.GetInt(-1);
		int columns = rootML.Attributes[TagColumns].Value.GetInt(-1);

			// Dependiendo de los datos cargados, crea una textura u otra
			if (rows > 0 && columns > 0)
				texturesManager.Create(name, asset, rows, columns);
			else
			{
				List<(string name, Rectangle region, UiMargin padding, TextureRegionNineSliceConfiguration? nineSlice)> regions = [];

					// Carga las regiones
					foreach (MLNode nodeML in rootML.Nodes)
						if (nodeML.Name == TagRegion)
						{
							Rectangle rectangle = _helper.GetRectangle(nodeML.Attributes[TagRectangle].Value.TrimIgnoreNull());
							UiMargin padding = _helper.GetMargin(nodeML.Attributes[TagPadding].Value.TrimIgnoreNull());

								// Carga el rectángulo desde los atributos
								if (rectangle.Width == 0)
									rectangle = new Rectangle(nodeML.Attributes[TagLeft].Value.GetInt(0),
															  nodeML.Attributes[TagTop].Value.GetInt(0),	
															  nodeML.Attributes[TagWidth].Value.GetInt(0),
															  nodeML.Attributes[TagHeight].Value.GetInt(0));
								// Añade la región
								regions.Add((nodeML.Attributes[TagName].Value.TrimIgnoreNull(), rectangle, padding, LoadNineSlice(nodeML)));
						}
					// Crea la textura adecuada
					if (regions.Count == 0)
						texturesManager.Create(name, asset);
					else
					{
						TextureConfiguration textureConfiguration = texturesManager.Create(name, asset);

							// Añade las regiones
							foreach ((string name, Rectangle region, UiMargin padding, TextureRegionNineSliceConfiguration? nineSlice) regionData in regions)
								textureConfiguration.Regions.Add(regionData.name,
																 new TextureRegionRectangleConfiguration(regionData.name)
																		{
																			Region = regionData.region,
																			Padding = regionData.padding,
																			NineSliceConfiguration = regionData.nineSlice
																		}
																 );
					}
			}
	}

	/// <summary>
	///		Carga la configuración de un elemento de tipo NineSlice
	/// </summary>
	private TextureRegionNineSliceConfiguration? LoadNineSlice(MLNode rootML)
	{
		TextureRegionNineSliceConfiguration configuration = new();

			// Carga los datos
			configuration.TopLeftWidth = rootML.Attributes[TagTopLeftWidth].Value.GetInt(-1);
			configuration.TopLeftHeight = rootML.Attributes[TagTopLeftHeight].Value.GetInt(-1);
			configuration.TopRightWidth = rootML.Attributes[TagTopRightWidth].Value.GetInt(-1);
			configuration.TopRightHeight = rootML.Attributes[TagTopRightHeight].Value.GetInt(-1);
			configuration.BottomLeftWidth = rootML.Attributes[TagBottomLeftWidth].Value.GetInt(-1);
			configuration.BottomLeftHeight = rootML.Attributes[TagBottomLeftHeight].Value.GetInt(-1);
			configuration.BottomRightWidth = rootML.Attributes[TagBottomRightWidth].Value.GetInt(-1);
			configuration.BottomRightHeight = rootML.Attributes[TagBottomRightHeight].Value.GetInt(-1);
			configuration.FillBackground = rootML.Attributes[TagFillBackground].Value.GetBool();
			configuration.BackgroundColor = _helper.GetColor(rootML.Attributes[TagBackgroundColor].Value, Color.White);
			configuration.BackgroundOpacity = (float) rootML.Attributes[TagBackgroundOpacity].Value.GetDouble(1);
			// Devuelve los datos
			if (configuration.TopLeftWidth > 0 && configuration.TopLeftHeight > 0 && 
					configuration.TopRightWidth > 0 && configuration.TopRightHeight > 0 && 
					configuration.BottomLeftWidth  > 0 && configuration.BottomLeftHeight > 0 && 
					configuration.BottomRightWidth > 0 && configuration.BottomRightHeight > 0)
				return configuration;
			else
				return null;
	}

	/// <summary>
	///		Carga una animación
	/// </summary>
	private void LoadAnimation(MLNode rootML, AnimationManager animationManager)
	{
		Animation animation = animationManager.Add(rootML.Attributes[TagName].Value.TrimIgnoreNull(),
												   rootML.Attributes[TagTexture].Value.TrimIgnoreNull(),
												   []);

			// Añade los frames
			if (rootML.Nodes.Count != 0)
			{
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagFrame)
						animation.Frames.Add(new Animation.AnimationFrame(nodeML.Attributes[TagRegion].Value.TrimIgnoreNull(),
																		  (float) nodeML.Attributes[TagTime].Value.GetDouble(0.07f)));
			}
			else
			{
				int rows = rootML.Attributes[TagRows].Value.GetInt(-1);
				int columns = rootML.Attributes[TagColumns].Value.GetInt(-1);
				float time = (float) rootML.Attributes[TagTime].Value.GetDouble(0.07);

					// Crea las regiones
					if (rows > 0 && columns > 0)
						for (int row = 0; row < rows; row++)
							for (int column = 0; column < columns; column++)
								animation.Frames.Add(new Animation.AnimationFrame($"{row.ToString()},{column.ToString()}", time));
			}
	}

	/// <summary>
	///		Carga un grupo de animaciones
	/// </summary>
	private void LoadAnimationGroup(MLNode rootML, AnimationManager animationManager)
	{
		AnimationBlenderGroupModel group = new(rootML.Attributes[TagName].Value.TrimIgnoreNull());

			// Carga las animaciones
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagAnimation)
				{
					AnimationBlenderGroupRuleModel rule = new(nodeML.Attributes[TagName].Value.TrimIgnoreNull(), nodeML.Attributes[TagLoop].Value.GetBool(true));

						// Carga las condiciones
						foreach (MLNode ruleML in nodeML.Nodes)
							if (ruleML.Name == TagRule)
								rule.Rules.Add(new AnimationBlenderGroupRuleModel.Rule
														(ruleML.Attributes[TagProperty].Value.TrimIgnoreNull(),
														 ruleML.Attributes[TagCondition].Value.GetEnum(AnimationBlenderGroupRuleModel.ConditionOperator.Equals),
														 (float) ruleML.Attributes[TagValue].Value.GetDouble(0)));
						// Añade la regla al grupo
						group.GroupRules.Add(rule);
				}
			// Añade el grupo de animaciones
			animationManager.AnimationBlender.Add(group);
	}
}