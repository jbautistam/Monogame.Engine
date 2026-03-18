using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Grids;

/// <summary>
///     Representa un elemento hijo dentro del grid
/// </summary>
public class GridItemModel : UiElement
{
    public GridItemModel(GridItemCollection parent, UiElement item) : base(parent.Grid.Layer, new UiPosition(0, 0, 1, 1))
    {
        GridItems = parent;
        Parent = parent.Grid;
        Item = item;
        item.Parent = this;
    }

    /// <summary>
    ///     Calcula la posición en pantalla
    /// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
        // Calcula la posición del elemento en el grid
        ComputeBounds();
        // Calcula la posición del elemento hijo
        Item.ComputeScreenBounds(Position.ContentBounds);
	}

    /// <summary>
    ///     Calcula el rectángulo de dibujo de este elemento del grid
    /// </summary>
    private void ComputeBounds()
    {
        GridSizeLayoutModel? column = GridItems.Grid.Definitions.GetColumn(Column);
        GridSizeLayoutModel? row = GridItems.Grid.Definitions.GetRow(Row);

            // Calcula la posición del elemento
            if (column is not null && row is not null)
                Position.ContentBounds = new Rectangle(GridItems.Grid.Position.ContentBounds.X + (int) column.CalculatedPosition,
                                                       GridItems.Grid.Position.ContentBounds.Y + (int) row.CalculatedPosition,
                                                       (int) GridItems.Grid.Definitions.ComputeWidth(Column, ColumnSpan), 
                                                       (int) GridItems.Grid.Definitions.ComputeHeight(Row, RowSpan));
            else // esto no debería pasar
                Position.ContentBounds = GridItems.Grid.Position.ContentBounds;
    }

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        if (Item.Enabled)
		    Item.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja el elemento
    /// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
        if (Item.Visible)
		    Item.Draw(renderingManager, gameContext);
	}

	/// <summary>
	///     Colección a la que se asocia el elemento
	/// </summary>
	public GridItemCollection GridItems { get; }

    /// <summary>
    ///     Componente visual
    /// </summary>
    public UiElement Item { get; }

    /// <summary>
    ///     Columna en el grid
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    ///     Fila en el grid
    /// </summary>
    public int Row { get; set; }
        
    /// <summary>
    ///     Span de columnas
    /// </summary>
    public int ColumnSpan { get; set; }

    /// <summary>
    ///     Span de filas
    /// </summary>
    public int RowSpan { get; set; }
}
