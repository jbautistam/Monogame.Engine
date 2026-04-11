using Microsoft.Xna.Framework;
using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.BauEngine.Entities.UserInterface.Styles;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Repositorio para carga de estilos
/// </summary>
internal class StylesRepository
{
	// Constantes privadas
	private const string TagRoot = "Styles";
	private const string TagStyle = "Style";
	private const string TagName = "Name";
	private const string TagType = "Type";
	private const string TagMargin = "Margin";
	private const string TagPadding = "Padding";
	private const string TagBackground = "Background";
	private const string TagBorder = "Border";
	private const string TagColor = "Color";
	private const string TagOpacity = "Opacity";
	private const string TagTexture = "Texture";
	private const string TagRegion = "Region";
	private const string TagThickness = "Thickness";
	private const string TagRadius = "Radius";
	private const string TagShadowColor = "ShadowColor";
	private const string TagOffset = "Offset";
	private const string TagBlurRadius = "BlurRadius";
	private const string TagText = "Text";
	private const string TagFont = "Font";
	private const string TagRotation = "Rotation";
	private const string TagPresentation = "Presentation";
	// Variables privadas
	private RepositoryXmlHelper _helper = new();

	/// <summary>
	///		Carga los estilos a partir de un texto XML
	/// </summary>
	internal UiStylesCollection Load(Bau.BauEngine.Scenes.Layers.AbstractUserInterfaceLayer layer, string xml)
	{
		UiStylesCollection styles = new(layer);
		MLFile fileML = new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			// Carga los datos
			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
						foreach (MLNode nodeML in rootML.Nodes)
							if (nodeML.Name == TagStyle)
							{
								string name = nodeML.Attributes[TagName].Value.TrimIgnoreNull();

									if (!string.IsNullOrWhiteSpace(name))
										foreach (MLNode childML in nodeML.Nodes)
										{
											UiStyle.StyleType type = childML.Name.TrimIgnoreNull().GetEnum(UiStyle.StyleType.Normal);

												styles.Add(name, LoadStyle(styles, type, childML));
										}
							}
			// Devuelve la colección de estilos
			return styles;
	}

	/// <summary>
	///		Carga los datos de un estilo
	/// </summary>
	private UiStyle LoadStyle(UiStylesCollection parent, UiStyle.StyleType type, MLNode rootML)
	{
		UiStyle style = new(parent, type);

			// Carga los datos
			style.Margin = _helper.GetMargin(rootML.Attributes[TagMargin].Value.TrimIgnoreNull());
			style.Padding = _helper.GetMargin(rootML.Attributes[TagPadding].Value.TrimIgnoreNull());
			// Carga el fondo y el borde
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagBackground:
							LoadBackground(style, nodeML);
						break;
					case TagBorder:
							LoadBorder(style, nodeML);
						break;
					case TagText:
							LoadText(style, nodeML);
						break;
					case TagPresentation:
							LoadPresentation(style, nodeML);
						break;
				}
			// Devuelve el estilo generado
			return style;
	}

	/// <summary>
	///		Carga los datos de un borde
	/// </summary>
	private void LoadBorder(UiStyle style, MLNode rootML)
	{
		// Crea el estilo
		style.StyleBorder = new UiStyleBorder();
		// Asigna la textura
		style.StyleBorder.Texture = rootML.Attributes[TagTexture].Value.TrimIgnoreNull();
		style.StyleBorder.Region = rootML.Attributes[TagRegion].Value.TrimIgnoreNull();
		// Asigna los posibles datos del borde
		style.StyleBorder.CornerRadius = rootML.Attributes[TagRadius].Value.GetInt(0);
		style.StyleBorder.ShadowColor = _helper.GetColor(rootML.Attributes[TagShadowColor].Value, Color.White);
		style.StyleBorder.ShadowOffset = _helper.GetVector(rootML.Attributes[TagOffset].Value);
		style.StyleBorder.ShadowBlurRadius = rootML.Attributes[TagBlurRadius].Value.GetInt(0);
		// Asigna las propiedades comunes
		style.StyleBorder.Thickness = rootML.Attributes[TagThickness].Value.GetInt(1);
		style.StyleBorder.Color = _helper.GetColor(rootML.Attributes[TagColor].Value, Color.White);
		style.StyleBorder.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
	}

	/// <summary>
	///		Carga los datos de un fondo
	/// </summary>
	private void LoadBackground(UiStyle style, MLNode rootML)
	{
		// Crea el estilo de fondo
		style.StyleBackground = new UiStyleBackground();
		// Asigna los datos
		style.StyleBackground.Texture = rootML.Attributes[TagTexture].Value.TrimIgnoreNull();
		style.StyleBackground.Region = rootML.Attributes[TagRegion].Value.TrimIgnoreNull();
		// Asigna los valores comunes
		style.StyleBackground.Color = _helper.GetColor(rootML.Attributes[TagColor].Value.TrimIgnoreNull(), Color.White);
		style.StyleBackground.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
	}

	/// <summary>
	///		Carga el estilo de texto
	/// </summary>
	private void LoadText(UiStyle style, MLNode rootML)
	{
		// Genera el estilo
		style.StyleText = new UiStyleText();
		// Asigna los datos del estilo
		style.StyleText.Font = rootML.Attributes[TagFont].Value.TrimIgnoreNull();
		style.StyleText.Color = _helper.GetColor(rootML.Attributes[TagColor].Value.TrimIgnoreNull(), Color.White);
		style.StyleText.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
		style.StyleText.Rotation = (float) rootML.Attributes[TagRotation].Value.GetDouble(0);
	}

	/// <summary>
	///		Carga el estilo de presentación
	/// </summary>
	private void LoadPresentation(UiStyle style, MLNode rootML)
	{
		style.StylePresentation.Color = _helper.GetColor(rootML.Attributes[TagColor].Value.TrimIgnoreNull(), Color.White);
		style.StylePresentation.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
		style.StylePresentation.Rotation = (float) rootML.Attributes[TagRotation].Value.GetDouble(0);
	}
}