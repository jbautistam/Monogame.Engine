using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Grids;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de grids
/// </summary>
public class UserInterfaceGridBuilder : AbstractElementUserInterfaceBuilder<UiGrid>
{
	public UserInterfaceGridBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiGrid(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Añade una definición de fila
	/// </summary>
	public UserInterfaceGridBuilder WithRow(float height, float minHeight = 0, float maxHeight = 0)
	{
		// Crea el elemento
		Item.Definitions.AddRow(height, minHeight, maxHeight);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una definición de columna
	/// </summary>
	public UserInterfaceGridBuilder WithColumn(float width, float minWidth = 0, float maxWidth = 0)
	{
		// Crea el elemento
		Item.Definitions.AddColumn(width, minWidth, maxWidth);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un elemento
	/// </summary>
	public UserInterfaceGridBuilder WithItem(UiElement element, int row, int column, int rowSpan = 1, int columnSpan = 1)
	{
		// Crea el elemento
		Item.Items.Add(element, row, column, rowSpan, columnSpan);
		// Devuelve el generador
		return this;
	}
}