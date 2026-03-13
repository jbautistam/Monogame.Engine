using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Borders;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

namespace EngineSample.Core.Configuration.Repositories;

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
	// Variables privadas
	private RepositoryHelper _helper = new();

	/// <summary>
	///		Carga los estilos a partir de un texto XML
	/// </summary>
	internal UiStylesCollection Load(Bau.Libraries.BauGame.Engine.Scenes.Layers.AbstractUserInterfaceLayer layer, string xml)
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
										styles.Add(name, LoadStyle(styles, nodeML));
							}
			// Devuelve la colección de estilos
			return styles;
	}

	/// <summary>
	///		Carga los datos de un estilo
	/// </summary>
	private UiStyle LoadStyle(UiStylesCollection parent, MLNode rootML)
	{
		UiStyle style = new(parent, rootML.Attributes[TagType].Value.GetEnum(UiStyle.StyleType.Normal));

			// Carga los datos
			style.Color = _helper.GetColor(rootML.Attributes[TagColor].Value.TrimIgnoreNull(), Color.White);
			style.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
			style.Margin = _helper.GetMargin(rootML.Attributes[TagMargin].Value.TrimIgnoreNull());
			style.Padding = _helper.GetMargin(rootML.Attributes[TagPadding].Value.TrimIgnoreNull());
			// Carga el fondo y el borde
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagBackground:
							style.Background = LoadBackground(style, nodeML);
						break;
					case TagBorder:
							style.Border = LoadBorder(style, nodeML);
						break;
				}
			// Devuelve el estilo generado
			return style;
	}

	/// <summary>
	///		Carga los datos de un borde
	/// </summary>
	private UiAbstractBorder? LoadBorder(UiStyle style, MLNode rootML)
	{
		UiAbstractBorder border;
		string texture = rootML.Attributes[TagTexture].Value.TrimIgnoreNull();

			// Carga los datos del borde
			if (!string.IsNullOrWhiteSpace(texture))
			{
				UiTextureBorder real = new(style);

					// Asigna la textura
					real.Sprite = new SpriteDefinition(texture, rootML.Attributes[TagRegion].Value.TrimIgnoreNull());
					// Asigna el borde
					border = real;
			}
			else
			{
				UiSolidBorder real = new(style);

					// Asigna los posibles datos del borde
					real.CornerRadius = rootML.Attributes[TagRadius].Value.GetInt(0);
					real.ShadowColor = _helper.GetColor(rootML.Attributes[TagShadowColor].Value, Color.White);
					real.ShadowOffset = _helper.GetVector(rootML.Attributes[TagOffset].Value);
					real.ShadowBlurRadius = rootML.Attributes[TagBlurRadius].Value.GetInt(0);
					// Asigna el borde
					border = real;
			}
			// Asigna las propiedades
			border.Thickness = rootML.Attributes[TagThickness].Value.GetInt(1);
			border.Color = _helper.GetColor(rootML.Attributes[TagColor].Value, Color.White);
			border.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
			// Devuelve el borde
			return border;
	}

	/// <summary>
	///		Carga los datos de un fondo
	/// </summary>
	private UiAbstractBackground? LoadBackground(UiStyle style, MLNode rootML)
	{
		UiAbstractBackground background;
		string texture = rootML.Attributes[TagTexture].Value.TrimIgnoreNull();

			// Carga los datos del fondo
			if (!string.IsNullOrWhiteSpace(texture))
			{
				UiBackground real = new(style);

					// Asigna los datos
					real.Sprite = new SpriteDefinition(texture, rootML.Attributes[TagRegion].Value.TrimIgnoreNull());
					// Guarda el fondo para cargar el resto de atributos
					background = real;
			}
			else
				background = new UiSolidColorBackground(style);
			// Asigna los valores comunes
			background.Color = _helper.GetColor(rootML.Attributes[TagColor].Value.TrimIgnoreNull(), Color.White);
			background.Opacity = (float) rootML.Attributes[TagOpacity].Value.GetDouble(1);
			// Devuelve el fondo
			return background;
	}
}