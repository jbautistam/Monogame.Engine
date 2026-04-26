using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.Sprites;
using Bau.BauEngine.Entities.UserInterface.Grids;
using Bau.BauEngine.Entities.UserInterface.Galleries;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Repositorio para carga de interface de usuario
/// </summary>
public abstract class AbstractUserInterfaceRepository
{
	// Constantes privadas
	private const string TagRoot = "Screen";
	private const string TagId = "Id";
	private const string TagMenu = "Menu";
	private const string TagLabel = "Label";
	private const string TagImage = "Image";
	private const string TagProgressBar = "ProgressBar";
	private const string TagButton = "Button";
	private const string TagSlideBar = "SlideBar";
	private const string TagPosition = "Position";
	private const string TagStyle = "Style";
	private const string TagTag = "Tag";
	private const string TagZIndex = "ZIndex";
	private const string TagOption = "Option";
	private const string TagText = "Text";
	private const string TagFont = "Font";
	private const string TagValue = "Value";
	private const string TagTexture = "Texture";
	private const string TagRegion = "Region";
	private const string TagBarTexture = "BarTexture";
	private const string TagBarRegion = "BarRegion";
	private const string TagStrech = "Strech";
	private const string TagPreserveAspectRatio = "PreserveAspectRatio";
	private const string TagMaximum = "Maximum";
	private const string TagHorizontal = "Horizontal";
	private const string TagVertical = "Vertical";
	private const string TagLineSpacing = "LineSpacing";
	private const string TagTextScale = "TextScale";
	private const string TagVisible = "Visible";
	private const string TagGrid = "Grid";
	private const string TagItem = "Item";
	private const string TagRows = "Rows";
	private const string TagRow = "Row";
	private const string TagRowSpan = "RowSpan";
	private const string TagColumns = "Columns";
	private const string TagColumn = "Column";
	private const string TagColumnSpan = "ColumnSpan";
	private const string TagRowSpacing = "RowSpacing";
	private const string TagColumnSpacing = "ColumnSpacing";
	private const string TagGallery = "Gallery";
	private const string TagVisibleRows = "VisibleRows";
	private const string TagVisibleColumns = "VisibleColumns";
	private const string TagThumb = "Thumb";
	private const string TagLeftBar = "LeftBar";
	private const string TagRightBar = "RightBar";
	private const string TagMinimum = "Minimum";
	private const string TagOrientation = "Orientation";
	private const string TagDrawMode = "DrawMode";
	private const string TagMargin = "Margin";
	private const string TagPadding = "Padding";
	private const string TagCheckbox = "Checkbox";
	private const string TagIsChecked = "IsChecked";
	private const string TagCheckedImage = "CheckedImage";
	private const string TagUncheckedImage = "UncheckedImage";
	private const string TagCheckedLabel = "CheckedLabel";
	private const string TagUncheckedLabel = "UncheckedLabel";
	// Variables privadas
	private RepositoryXmlHelper _helper = new();

	/// <summary>
	///		Carga los estilos a partir de un texto XML
	/// </summary>
	public (string? style, List<UiElement>) Load(AbstractUserInterfaceLayer layer, string xml)
	{
		List<UiElement> items = [];
		string? style = null;
		MLFile fileML = new Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			// Carga los datos
			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
					{
						style = rootML.Attributes[TagStyle].Value.TrimIgnoreNull();
						items.AddRange(LoadItems(layer, rootML.Nodes));
					}
			// Devuelve la colección de opciones
			return (style, items);
	}

	/// <summary>
	///		Carga una lista de elementos
	/// </summary>
	private List<UiElement> LoadItems(AbstractUserInterfaceLayer layer, MLNodesCollection nodesML)
	{
		List<UiElement> items = [];

			// Carga los elementos
			foreach (MLNode nodeML in nodesML)
				switch (nodeML.Name)
				{
					case TagMenu:
							items.Add(LoadMenu(layer, nodeML));
						break;
					case TagLabel:
							items.Add(LoadLabel(layer, nodeML));
						break;
					case TagImage:
							items.Add(LoadImage(layer, nodeML));
						break;
					case TagProgressBar:
							items.Add(LoadProgressBar(layer, nodeML));
						break;
					case TagButton:
							items.Add(LoadButton(layer, nodeML));
						break;
					case TagCheckbox:
							items.Add(LoadCheckbox(layer, nodeML));
						break;
					case TagGrid:
							items.Add(LoadGrid(layer, nodeML));
						break;
					case TagGallery:
							items.Add(LoadGallery(layer, nodeML));
						break;
					case TagSlideBar:
							items.Add(LoadSlideBar(layer, nodeML));
						break;
					default:
							UiElement? item = LoadUiItem(layer, nodeML);

								if (item is not null)
									items.Add(item);
						break;
				}
			// Devuelve la lista de elementos
			return items;
	}

	/// <summary>
	///		Carga un elemento de interface de usuario particular para el proyecto
	/// </summary>
	protected abstract UiElement? LoadUiItem(AbstractUserInterfaceLayer layer, MLNode rootML);

	/// <summary>
	///		Carga los datos de un menú
	/// </summary>
	protected UiMenu LoadMenu(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiMenu menu = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value.TrimIgnoreNull()));

			// Carga los datos
			AssignGeneralAttributes(menu, rootML);
			// Carga las opciones
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagOption)
					menu.AddOption(LoadOption(menu, nodeML));
			// Devuelve el estilo generado
			return menu;
	}

	/// <summary>
	///		Carga los datos de una opción de menú
	/// </summary>
	protected UiMenuOption LoadOption(UiMenu menu, MLNode rootML)
	{
		UiMenuOption option = new(menu, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value.TrimIgnoreNull()), 
								  rootML.Attributes[TagValue].Value.GetInt(0));

			// Asigna las propiedades
			AssignGeneralAttributes(option, rootML);
			option.Text = rootML.Attributes[TagText].Value.TrimIgnoreNull();
			// Devuelve la opción cargada
			return option;
	}

	/// <summary>
	///		Carga los datos de una etiqueta
	/// </summary>
	protected UiLabel LoadLabel(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiLabel label = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(label, rootML);
			label.Font = GetFont(rootML);
			label.Text = rootML.Attributes[TagText].Value.TrimIgnoreNull();
			label.HorizontalAlignment = rootML.Attributes[TagHorizontal].Value.GetEnum(UiLabel.HorizontalAlignmentType.Left);
			label.VerticalAlignment = rootML.Attributes[TagVertical].Value.GetEnum(UiLabel.VerticalAlignmentType.Top);
			// Devuelve la etiqueta
			return label;
	}

	/// <summary>
	///		Carga los datos de una imagen
	/// </summary>
	protected UiImage LoadImage(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiImage image = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(image, rootML);
			image.Sprite = new SpriteDefinition(rootML.Attributes[TagTexture].Value.TrimIgnoreNull(), rootML.Attributes[TagRegion].Value.TrimIgnoreNull());
			image.Stretch = rootML.Attributes[TagStrech].Value.GetBool(true);
			image.PreserveAspectRatio = rootML.Attributes[TagPreserveAspectRatio].Value.GetBool(true);
			image.HorizontalAlignment = rootML.Attributes[TagHorizontal].Value.GetEnum(UiLabel.HorizontalAlignmentType.Left);
			image.VerticalAlignment = rootML.Attributes[TagVertical].Value.GetEnum(UiLabel.VerticalAlignmentType.Top);
			// Devuelve la imagen
			return image;
	}

	/// <summary>
	///		Carga los datos de una barra de progreso
	/// </summary>
	protected UiProgressBar LoadProgressBar(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiProgressBar progressBar = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(progressBar, rootML);
			progressBar.Background = new SpriteDefinition(rootML.Attributes[TagTexture].Value.TrimIgnoreNull(), rootML.Attributes[TagRegion].Value.TrimIgnoreNull());
			progressBar.Bar = new SpriteDefinition(rootML.Attributes[TagBarTexture].Value.TrimIgnoreNull(), rootML.Attributes[TagBarRegion].Value.TrimIgnoreNull());
			progressBar.Orientation = rootML.Attributes[TagOrientation].Value.GetEnum(UiProgressBar.OrientationMode.Horizontal);
			progressBar.Maximum = rootML.Attributes[TagMaximum].Value.GetInt(100);
			progressBar.Value = rootML.Attributes[TagValue].Value.GetInt(0);
			// Devuelve la barra de progreso
			return progressBar;
	}

	/// <summary>
	///		Carga los datos de un botón
	/// </summary>
	protected UiButton LoadButton(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiButton button = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(button, rootML);
			// Carga la etiqueta
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagLabel)
					button.Label = LoadLabel(layer, nodeML);
			// Devuelve el botón
			return button;
	}

	/// <summary>
	///		Carga los datos de un <see cref="UiCheckbox"/>
	/// </summary>
	protected UiCheckbox LoadCheckbox(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiCheckbox checkbox = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(checkbox, rootML);
			// Carga los datos básicos
			checkbox.IsChecked = rootML.Attributes[TagIsChecked].Value.GetBool();
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagCheckedImage:
							checkbox.CheckedImage = LoadImage(layer, nodeML);
						break;
					case TagUncheckedImage:
							checkbox.UncheckedImage = LoadImage(layer, nodeML);
						break;
					case TagCheckedLabel:
							checkbox.CheckedLabel = LoadLabel(layer, nodeML);
						break;
					case TagUncheckedLabel:
							checkbox.UncheckedLabel = LoadLabel(layer, nodeML);
						break;
				}
			// Devuelve el botón
			return checkbox;
	}

	/// <summary>
	///		Carga los datos de una slidebar
	/// </summary>
	protected UiSlideBar LoadSlideBar(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiSlideBar slideBar = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna las propiedades comunes
			AssignGeneralAttributes(slideBar, rootML);
			// Asigna el resto de propiedades
			slideBar.Minimum = (float) rootML.Attributes[TagMinimum].Value.GetDouble(0);
			slideBar.Maximum = (float) rootML.Attributes[TagMaximum].Value.GetDouble(0);
			slideBar.Value = (float) rootML.Attributes[TagValue].Value.GetDouble(0);
			slideBar.Orientation = rootML.Attributes[TagOrientation].Value.GetEnum(UiSlideBar.SliderOrientation.Horizontal);
			slideBar.DrawMode = rootML.Attributes[TagDrawMode].Value.GetEnum(Scenes.Rendering.Renderers.SpriteRenderer.DrawMode.Fill);
			// Carga los sprites
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagThumb:
							slideBar.Thumb = new SpriteDefinition(nodeML.Attributes[TagTexture].Value.TrimIgnoreNull(), 
																  nodeML.Attributes[TagRegion].Value.TrimIgnoreNull());
						break;
					case TagLeftBar:
							slideBar.TrackLeft = new SpriteDefinition(nodeML.Attributes[TagTexture].Value.TrimIgnoreNull(), 
																	  nodeML.Attributes[TagRegion].Value.TrimIgnoreNull());
						break;
					case TagRightBar:
							slideBar.TrackRight = new SpriteDefinition(nodeML.Attributes[TagTexture].Value.TrimIgnoreNull(), 
																	   nodeML.Attributes[TagRegion].Value.TrimIgnoreNull());
						break;
				}
			// Devuelve el control
			return slideBar;
	}

	/// <summary>
	///		Carga los datos de un grid
	/// </summary>
	protected UiGrid LoadGrid(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiGrid grid = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(grid, rootML);
			// Carga las definiciones de filas
			foreach (float height in RepositoryHelper.GetValues(rootML.Attributes[TagRows].Value.TrimIgnoreNull()))
				grid.Definitions.AddRow(height);
			foreach (float width in RepositoryHelper.GetValues(rootML.Attributes[TagColumns].Value.TrimIgnoreNull()))
				grid.Definitions.AddColumn(width);
			grid.RowSpacing = rootML.Attributes[TagRowSpacing].Value.GetInt(0);
			grid.ColumnSpacing = rootML.Attributes[TagColumnSpacing].Value.GetInt(0);
			// Carga los elementos
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagItem)
					foreach (UiElement item in LoadItems(layer, nodeML.Nodes))
						grid.Items.Add(item, 
									   nodeML.Attributes[TagRow].Value.GetInt(0),
									   nodeML.Attributes[TagColumn].Value.GetInt(0),
									   nodeML.Attributes[TagRowSpan].Value.GetInt(1),
									   nodeML.Attributes[TagColumnSpan].Value.GetInt(1));
			// Devuelve el grid
			return grid;
	}

	/// <summary>
	///		Carga los datos de una galería
	/// </summary>
	protected UiGallery LoadGallery(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiGallery gallery = new(layer, RepositoryHelper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(gallery, rootML);
			gallery.Columns = rootML.Attributes[TagRows].Value.GetInt(1);
			gallery.Rows = rootML.Attributes[TagColumns].Value.GetInt(1);
			gallery.ViewportRows = rootML.Attributes[TagVisibleRows].Value.GetInt(1);
			gallery.ViewportColumns = rootML.Attributes[TagVisibleColumns].Value.GetInt(1);
			// Carga los elementos
			foreach (MLNode nodeML in rootML.Nodes)
				if (nodeML.Name == TagItem)
					foreach (UiElement item in LoadItems(layer, nodeML.Nodes))
						gallery.Add(item, nodeML.Attributes[TagRow].Value.GetInt(0), nodeML.Attributes[TagColumn].Value.GetInt(0));
			// Devuelve el grid
			return gallery;
	}

	/// <summary>
	///		Asigna los atributos generales
	/// </summary>
	protected void AssignGeneralAttributes(UiElement component, MLNode rootML)
	{
		// Carga los datos básicos
		component.Id = GetId(rootML.Attributes[TagId].Value.TrimIgnoreNull());
		component.Style = rootML.Attributes[TagStyle].Value.TrimIgnoreNull();
		component.Tag = rootML.Attributes[TagTag].Value.TrimIgnoreNull();
		component.ZIndex = rootML.Attributes[TagZIndex].Value.GetInt(0);
		component.Visible = rootML.Attributes[TagVisible].Value.GetBool(true);
		// Carga el margen y el espaciado
		if (!string.IsNullOrWhiteSpace(rootML.Attributes[TagMargin].Value))
			component.Position.Margin = _helper.GetMargin(rootML.Attributes[TagMargin].Value);
		if (!string.IsNullOrWhiteSpace(rootML.Attributes[TagPadding].Value))
			component.Position.Margin = _helper.GetMargin(rootML.Attributes[TagPadding].Value);
	}

	/// <summary>
	///		Lee los datos de la fuente de un nodo
	/// </summary>
	protected SpriteTextDefinition GetFont(MLNode rootML)
	{
		return new SpriteTextDefinition(rootML.Attributes[TagFont].Value.TrimIgnoreNull())
						{
							LineSpacing = (float) rootML.Attributes[TagLineSpacing].Value.GetDouble(1),
							TextScale = (float) rootML.Attributes[TagTextScale].Value.GetDouble(1)
						};
	}

	/// <summary>
	///		Obtiene un Id. Si no hay nada en la cadena, crea un Guid
	/// </summary>
	private string GetId(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return Guid.NewGuid().ToString();
		else
			return value;
	}

	/// <summary>
	///		Helper para acceso a funciones de interpretación de cadenas en el XML
	/// </summary>
	protected RepositoryXmlHelper RepositoryHelper { get; } = new();
}