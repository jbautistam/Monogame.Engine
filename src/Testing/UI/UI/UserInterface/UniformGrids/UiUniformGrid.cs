using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace UI.UserInterface.UniformGrids;

/// <summary>
///     Grid con filas y columnas del mismo tamaño
/// </summary>
public class UiUniformGrid(Bau.Libraries.BauGame.Engine.Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Variables privadas
    private int _totalColumns, _totalRows, _viewPortColumns, _viewPortRows;
    private int _firstVisibleRow, _firstVisibleColumn;
    private List<UiUniformGridItem> _items = [];

    // TODO: Quitar esta función
    private void Invalidate()
    {
    }

    /// <summary>
    ///     Añade un elemento a una celda
    /// </summary>
    public void Add(int row, int column, UiElement item)
    {
        _items.Add(new UiUniformGridItem(this, row, column, item));
        Invalidate();
    }

    /// <summary>
    ///     Calcula los límites de los elementos
    /// </summary>
	protected override void ComputeScreenComponentBounds()
	{
        float cellWidth = 1 / Math.Min(ViewportColumns, 1);
        float cellHeight = 1 / Math.Min(ViewportRows, 1);
        float x = 0, y = 0;

            // Oculta todos los elementos del grid
            foreach (UiUniformGridItem gridItem in _items)
                gridItem.Visible = false;
            // Posiciona los elementos visibles del grid
            for (int row = FirstVisibleRow; row < FirstVisibleRow + ViewportRows; row++)
            {
                // Inicializa la coordenada x
                x = 0;
                // Recoloca los elementos
                for (int column = FirstVisibleColumn; column < FirstVisibleColumn + ViewportColumns; column++)
                {
                    List<UiUniformGridItem> gridItems = GetItems(row, column);

                        // Actualiza los elementos
                        foreach (UiUniformGridItem gridItem in gridItems)
                        {
                            // Posiciona el elemento
                            gridItem.Position = new UiPosition(x, y, cellWidth, cellHeight);
                            // Indica que el elemento es visible
                            gridItem.Visible = true;
                        }
                        // Incrementa la coordenada X
                        x += cellWidth;
                }
                // Incrementa la fila
                y += cellHeight;
            }
	}

    /// <summary>
    ///     Actualiza los elementos
    /// </summary>
	public override void Update(GameContext gameContext)
	{
        foreach (UiUniformGridItem item in _items)
            if (item.Visible && item.Element.Enabled)
                item.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja los elementos
    /// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
        for (int row = FirstVisibleRow; row < FirstVisibleRow + ViewportRows; row++)
            for (int column = FirstVisibleColumn; column < FirstVisibleColumn + ViewportColumns; column++)
            {
                List<UiUniformGridItem> gridItems = GetItems(row, column);

                    // Ordena los elementos
                    gridItems.Sort((first, second) => first.ZIndex.CompareTo(second.ZIndex));
                    // Dibja los elementos
                    foreach (UiUniformGridItem gridItem in gridItems)
                        if (gridItem.Element.Visible)
                            gridItem.Draw(camera, gameContext);
            }
	}

    /// <summary>
    ///     Obtiene los elementos de una celda
    /// </summary>
    private List<UiUniformGridItem> GetItems(int row, int column)
    {
        List<UiUniformGridItem> _items = [];

            // Obtiene los elementos de la fila / columna
            foreach (UiUniformGridItem gridItem in _items)
                if (gridItem.Row == row && gridItem.Column == column)
                    _items.Add(gridItem);
            // Devuelve la lista de elementos
            return _items;
    }

    /// <summary>
    ///     Comprueba si se modifica un valor e invalida el control si es necesario
    /// </summary>
    private void CheckUpdated(ref int propertyValue, int newValue)
    {
        if (propertyValue != newValue)
        {
            propertyValue = newValue;
            Invalidate();
        }
    }

    /// <summary>
    ///     Número total de columnas en el grid
    /// </summary>
    public int TotalColumns 
    { 
        get { return _totalColumns; } 
        set { CheckUpdated(ref _totalColumns, value); }
    }
        
    /// <summary>
    ///     Número total de filas en el grid
    /// </summary>
    public int TotalRows
    { 
        get { return _totalRows; } 
        set { CheckUpdated(ref _totalRows, value); }
    }
        
    /// <summary>
    ///     Número de columnas visibles en pantalla
    /// </summary>
    public int ViewportColumns
    { 
        get { return _viewPortColumns; } 
        set { CheckUpdated(ref _viewPortColumns, value); }
    }
        
    /// <summary>
    ///     Número de filas visibles en pantalla
    /// </summary>
    public int ViewportRows
    { 
        get { return _viewPortRows; } 
        set { CheckUpdated(ref _viewPortRows, value); }
    }
        
    /// <summary>
    ///     Primera columna visible del viewport
    /// </summary>
    public int FirstVisibleColumn 
    { 
        get { return _firstVisibleColumn; }
        set { CheckUpdated(ref _firstVisibleColumn, value); }
    }
        
    /// <summary>
    ///     Primera fila visible del viewport
    /// </summary>
    public int FirstVisibleRow 
    { 
        get { return _firstVisibleRow; }
        set { CheckUpdated(ref _firstVisibleRow, value); }
    }
}