using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Grids;

/// <summary>
///     Lista de elementos del grid
/// </summary>
public class GridItemCollection(UiGrid grid) : List<GridItemModel>
{
    /// <summary>
    ///     Añade una definición a la colección
    /// </summary>
    public GridItemModel Add(UiElement item, int row, int column, int rowSpan, int columnSpan)
    {
        GridItemModel gridItem = new(this, item)
                                    { 
                                        Row = row,
                                        Column = column,
                                        RowSpan = rowSpan,
                                        ColumnSpan = columnSpan
                                    };

            // Añade el elemento a la lista
            Add(gridItem);
            // Invalida el grid
            Grid.Invalidate();
            // Devuelve el elemento añadido
            return gridItem;
    }

    /// <summary>
    ///     Elimina un elemento del grid
    /// </summary>
    public bool RemoveElement(GridItemModel element)
    {
        // Elimina el elemento
        if (Remove(element))
        {
            Grid.Invalidate();
            return true;
        }
        // Si ha llegado hasta aquí indica que no ha podido eliminar el elemento
        return false;
    }

    /// <summary>
    ///     Actualiza el layout de los elementos
    /// </summary>
	public void UpdateLayout()
	{
        foreach (GridItemModel element in this)
            element.Invalidate();
	}

    /// <summary>
    ///     Actualiza los elementos
    /// </summary>
	public void Update(GameContext gameContext)
	{
        foreach (GridItemModel item in this)
            if (item.Enabled)
                item.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja los elementos
    /// </summary>
	public void Draw(Camera2D camera, GameContext gameContext)
	{
        // Ordena los elementos
        Sort((first, second) => first.ZIndex.CompareTo(second.ZIndex));
        // Y los dibuja
        foreach (GridItemModel element in this)
            if (element.Visible)
                element.Draw(camera, gameContext);
	}

    /// <summary>
    ///     Prepara los comandos de presentación
    /// </summary>
	public void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, GameContext gameContext)
	{
        // Ordena los elementos
        Sort((first, second) => first.ZIndex.CompareTo(second.ZIndex));
        // Y los dibuja
        foreach (GridItemModel element in this)
            if (element.Visible)
                element.PrepareRenderCommands(builder, gameContext);
	}

	/// <summary>
	///     Grid al que se asocia el elemento
	/// </summary>
	public UiGrid Grid { get; } = grid;
}