using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Grids;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Galleries;

namespace EngineSample.Core.Configuration.Repositories;

/// <summary>
///		Repositorio para carga de interface de usuario
/// </summary>
internal class UserInterfaceRepository
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
	private const string TagVisualDialog = "VisualDialog";
	private const string TagHeader = "Header";
	private const string TagtAvatar = "Avatar";
	private const string TagTypeWriter = "TypeWriter";
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
	private const string TagSpeed = "Speed";
	private const string TagLineSpacing = "LineSpacing";
	private const string TagTextScale = "TextScale";
	private const string TagMode = "Mode";
	private const string TagVisible = "Visible";
	private const string TagCursor = "Cursor";
	private const string TagFlash = "Flash";
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
	private const string TagClipMode = "ClipMode";
	// Variables privadas
	private RepositoryHelper _helper = new();

	/// <summary>
	///		Carga los estilos a partir de un texto XML
	/// </summary>
	internal List<UiElement> Load(AbstractUserInterfaceLayer layer, string xml)
	{
		List<UiElement> items = [];
		MLFile fileML = new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			// Carga los datos
			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
						items.AddRange(LoadItems(layer, rootML.Nodes));
			// Devuelve la colección de opciones
			return items;
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
					case TagVisualDialog:
							items.Add(LoadVisualDialog(layer, nodeML));
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
				}
			// Devuelve la lista de elementos
			return items;
	}

	/// <summary>
	///		Carga los datos de un menú
	/// </summary>
	private UiMenu LoadMenu(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiMenu menu = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value.TrimIgnoreNull()));

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
	private UiMenuOption LoadOption(UiMenu menu, MLNode rootML)
	{
		UiMenuOption option = new(menu, _helper.GetPosition(rootML.Attributes[TagPosition].Value.TrimIgnoreNull()), 
								  rootML.Attributes[TagValue].Value.GetInt(0));

			// Asigna las propiedades
			AssignGeneralAttributes(option, rootML);
			option.Text = rootML.Attributes[TagText].Value.TrimIgnoreNull();
			option.Font = GetFont(rootML);
			// Devuelve la opción cargada
			return option;
	}

	/// <summary>
	///		Carga los datos de una etiqueta
	/// </summary>
	private UiLabel LoadLabel(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiLabel label = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

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
	private UiImage LoadImage(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiImage image = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

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
	private UiProgressBar LoadProgressBar(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiProgressBar progressBar = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

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
	private UiButton LoadButton(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiButton button = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

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
	///		Carga los datos de una slidebar
	/// </summary>
	private UiSlideBar LoadSlideBar(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiSlideBar slideBar = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna las propiedades comunes
			AssignGeneralAttributes(slideBar, rootML);
			// Asigna el resto de propiedades
			slideBar.Minimum = (float) rootML.Attributes[TagMinimum].Value.GetDouble(0);
			slideBar.Maximum = (float) rootML.Attributes[TagMaximum].Value.GetDouble(0);
			slideBar.Value = (float) rootML.Attributes[TagValue].Value.GetDouble(0);
			slideBar.Orientation = rootML.Attributes[TagOrientation].Value.GetEnum(UiSlideBar.SliderOrientation.Horizontal);
			slideBar.Mode = rootML.Attributes[TagClipMode].Value.GetEnum(UiSlideBar.ClipMode.Stretch);
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
	///		Carga los datos de un diálogo para una visual novel
	/// </summary>
	private UiVisualNovelDialog LoadVisualDialog(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiVisualNovelDialog dialog = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(dialog, rootML);
			// Carga el contenido
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagTypeWriter:
							dialog.TypeWriter = LoadVisualDialogTypeWriter(layer, nodeML);
						break;
					case TagHeader:
							dialog.Header = LoadVisualDialogHeader(layer, nodeML);
						break;
					case TagtAvatar:
							if (dialog.LeftAvatar is null)
								dialog.LeftAvatar = LoadVisualDialogAvatar(layer, nodeML);
							else
								dialog.RightAvatar = LoadVisualDialogAvatar(layer, nodeML);
						break;
					case TagCursor:
							dialog.CursorFlashDuration = (float) nodeML.Attributes[TagFlash].Value.GetDouble(2);
							foreach (MLNode childML in nodeML.Nodes)
								if (childML.Name == TagImage)
									dialog.Cursor = LoadImage(layer, childML);
						break;
				}
			// Devuelve el cuadro de diálogo
			return dialog;
	}

	/// <summary>
	///		Carga los datos de un cuadro de texto de diálogo para una visual novel
	/// </summary>
	private UiTypeWriterLabel LoadVisualDialogTypeWriter(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiTypeWriterLabel typeWriter = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(typeWriter, rootML);
			typeWriter.Font = GetFont(rootML);
			typeWriter.Speed = (float) rootML.Attributes[TagSpeed].Value.GetDouble(0.01);
			typeWriter.Mode = rootML.Attributes[TagMode].Value.GetEnum(UiTypeWriterLabel.WriteMode.Characters);
			typeWriter.Text = rootML.Value.TrimIgnoreNull();
			// Devuelve el cuadro de texto
			return typeWriter;
	}

	/// <summary>
	///		Carga los datos de un cuadro de texto de diálogo para una visual novel
	/// </summary>
	private UiVisualNovelHeader LoadVisualDialogHeader(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiVisualNovelHeader header = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(header, rootML);
			// Carga el contenido
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagLabel:
							header.Label = LoadLabel(layer, nodeML);
						break;
					case TagImage:
							header.Image = LoadImage(layer, nodeML);
						break;
				}
			// Devuelve el cuadro de texto
			return header;
	}

	/// <summary>
	///		Carga los datos de un avatar para una visual novel
	/// </summary>
	private UiVisualNovelAvatar LoadVisualDialogAvatar(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiVisualNovelAvatar avatar = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(avatar, rootML);
			// Carga el contenido
			foreach (MLNode nodeML in rootML.Nodes)
				switch (nodeML.Name)
				{
					case TagImage:
							avatar.Avatar = LoadImage(layer, nodeML);
						break;
				}
			// Devuelve el avatar
			return avatar;
	}

	/// <summary>
	///		Carga los datos de un grid
	/// </summary>
	private UiGrid LoadGrid(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiGrid grid = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

			// Asigna los valores
			AssignGeneralAttributes(grid, rootML);
			// Carga las definiciones de filas
			foreach (float height in _helper.GetValues(rootML.Attributes[TagRows].Value.TrimIgnoreNull()))
				grid.Definitions.AddRow(height);
			foreach (float width in _helper.GetValues(rootML.Attributes[TagColumns].Value.TrimIgnoreNull()))
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
	private UiGallery LoadGallery(AbstractUserInterfaceLayer layer, MLNode rootML)
	{
		UiGallery gallery = new(layer, _helper.GetPosition(rootML.Attributes[TagPosition].Value));

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
	private void AssignGeneralAttributes(UiElement component, MLNode rootML)
	{
		component.Id = GetId(rootML.Attributes[TagId].Value.TrimIgnoreNull());
		component.Style = rootML.Attributes[TagStyle].Value.TrimIgnoreNull();
		component.Tag = rootML.Attributes[TagTag].Value.TrimIgnoreNull();
		component.ZIndex = rootML.Attributes[TagZIndex].Value.GetInt(0);
		component.Visible = rootML.Attributes[TagVisible].Value.GetBool(true);
	}

	/// <summary>
	///		Lee los datos de la fuente de un nodo
	/// </summary>
	public SpriteTextDefinition GetFont(MLNode rootML)
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
}