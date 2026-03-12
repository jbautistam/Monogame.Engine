namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Grids;

/// <summary>
///		Definición de un grid
/// </summary>
public class GridDefinitionModel(UiGrid grid)
{
	// Variables privadas
    private GridSizeLayoutCollection _columns = [];
    private GridSizeLayoutCollection _rows = [];

    /// <summary>
    ///     Añade una definición de columna al grid
    /// </summary>
    public GridSizeLayoutModel AddColumn(float width, float minWidth = 0, float maxWidth = 0, bool visible = true)
    {
        Grid.Invalidate();
        return _columns.Add(width, minWidth, maxWidth, visible);
    }

    /// <summary>
    ///     Añade una definición de fila al grid
    /// </summary>
    public GridSizeLayoutModel AddRow(float height, float minHeight = 0, float maxHeight = 0, bool visible = true)
    {
        Grid.Invalidate();
        return _rows.Add(height, minHeight, maxHeight, visible);
    }

    /// <summary>
    ///     Obtiene una columna del grid
    /// </summary>
	public GridSizeLayoutModel? GetColumn(int column) => _columns.Get(column);

    /// <summary>
    ///     Obtiene una fila del grid
    /// </summary>
	public GridSizeLayoutModel? GetRow(int row) => _rows.Get(row);

    /// <summary>
    ///     Calcula el ancho de una serie de columnas
    /// </summary>
	public float ComputeWidth(int column, int columnSpan) => _columns.ComputeSize(column, columnSpan);

    /// <summary>
    ///     Calcula el alto de una serie de filas 
    /// </summary>
	public float ComputeHeight(int row, int rowSpan) => _rows.ComputeSize(row, rowSpan);

    /// <summary>
    ///     Actualiza el layout del elemento
    /// </summary>
    public void UpdateLayout(int width, int height, int columnsSpacing, int rowsSpacing)
    {
        _columns.UpdateLayout(width, columnsSpacing);
        _rows.UpdateLayout(height, rowsSpacing);
    }

    /// <summary>
    ///     Muestra u oculta una columna
    /// </summary>
    public void SetColumnVisibility(int index, bool visible)
    {
        if (_columns.SetVisibility(index, visible))
            Grid.Invalidate();
    }

    /// <summary>
    ///     Muestra u oculta una fila
    /// </summary>
    public void SetRowVisibility(int index, bool visible)
    {
        if (_rows.SetVisibility(index, visible))
            Grid.Invalidate();
    }

    /// <summary>
    ///     Cambia el tamaño relativo de una columna
    /// </summary>
    public void SetColumnSize(int index, float size)
    {
        if (_columns.SetSize(index, size))
            Grid.Invalidate();
    }

    /// <summary>
    ///     Cambia el tamaño relativo de una fila
    /// </summary>
    public void SetRowSize(int index, float size)
    {
        if (_rows.SetSize(index, size))
            Grid.Invalidate();
    }

	/// <summary>
	///		Grid al que se asocia la definición
	/// </summary>
	public UiGrid Grid { get; } = grid;
}