using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Galleries;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de galerías
/// </summary>
public class UserInterfaceGalleryBuilder : AbstractElementUserInterfaceBuilder<UiGallery>
{
	public UserInterfaceGalleryBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiGallery(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Asigna los parámetros a la galería
	/// </summary>
	public UserInterfaceGalleryBuilder WithParameters(int rows, int columns, int visibleRows, int visibleColumns)
	{
		// Asigna los parámetros
		Item.Rows = rows;
		Item.Columns = columns;
		Item.ViewportRows = visibleRows;
		Item.ViewportColumns = visibleColumns;
		Item.FirstVisibleRow = 0;
		Item.FirstVisibleColumn = 0;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un elemento
	/// </summary>
	public UserInterfaceGalleryBuilder WithItem(UiElement element, int row, int column)
	{
		// Crea el elemento
		Item.Add(element, row, column);
		// Devuelve el generador
		return this;
	}
}