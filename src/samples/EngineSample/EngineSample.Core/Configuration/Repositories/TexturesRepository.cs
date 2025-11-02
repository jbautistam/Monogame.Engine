using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.BauGame.Engine.Managers.Resources;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;

namespace EngineSample.Core.Configuration.Repositories;

/// <summary>
///		Repositorio de texturas y animaciones
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
										LoadTexture(nodeML, resourcesManager.TextureManager);
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
	///		Carga una textora
	/// </summary>
	private void LoadTexture(MLNode rootML, TextureManager texturesManager)
	{
		string name = rootML.Attributes[TagName].Value.TrimIgnoreNull();
		string asset = rootML.Attributes[TagAsset].Value.TrimIgnoreNull();
		int? rows = rootML.Attributes[TagRows].Value.GetInt();
		int? columns = rootML.Attributes[TagColumns].Value.GetInt();

			// Crea la textura adecuada
			if (rows is not null && columns is not null)
				texturesManager.Create(name, asset, rows ?? 0, columns ?? 0);
			else
				texturesManager.Create(name, asset);
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
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagFrame)
					animation.Frames.Add(new Animation.AnimationFrame(nodeML.Attributes[TagRegion].Value.TrimIgnoreNull(),
																	  (float) nodeML.Attributes[TagTime].Value.GetDouble(0.07f)));
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