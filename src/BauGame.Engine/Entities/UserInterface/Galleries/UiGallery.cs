using Bau.BauEngine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Galleries;

/// <summary>
///     Grid con filas y columnas del mismo tamaño
/// </summary>
public class UiGallery(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position), Interfaces.IComponentPanel
{
    // Variables privadas
    private int _columns, _rows, _viewportColumns, _viewportRows;
    private int _firstVisibleRow, _firstVisibleColumn;
    private List<UiGalleryItem> _items = [];

    /// <summary>
    ///     Calcula el layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
    {
        float cellWidth = 1.0f / Math.Max(ViewportColumns, 1);
        float cellHeight = 1.0f / Math.Max(ViewportRows, 1);
        float x = 0, y = 0;

            // Oculta todos los elementos del grid
            foreach (UiGalleryItem gridItem in _items)
                gridItem.Visible = false;
            // Posiciona los elementos visibles del grid
            for (int row = FirstVisibleRow; row < FirstVisibleRow + ViewportRows; row++)
            {
                // Inicializa la coordenada x
                x = 0;
                // Recoloca los elementos
                for (int column = FirstVisibleColumn; column < FirstVisibleColumn + ViewportColumns; column++)
                {
                    UiGalleryItem? gridItem = GetGalleryItem(row, column);

                        // Actualiza los elementos
                        if (gridItem is not null)
                        {
                            // Posiciona el elemento
                            gridItem.Position = new UiPosition(x, y, cellWidth, cellHeight);
                            // Indica que el elemento es visible y lo invalida para que recalcule el elemento hijo
                            gridItem.Visible = true;
                            gridItem.Invalidate();
                        }
                        // Incrementa la coordenada X
                        x += cellWidth;
                }
                // Incrementa la fila
                y += cellHeight;
            }
    }

    /// <summary>
    ///     Añade un elemento a una celda. No invalida el grid porque si se añaden muchos elementos se llamaría demasiadas veces a <see cref="ComputeScreenBoundsSelf"/>,
    /// es mejor añadirlos todos y después llamar al método Invalidate de la galería
    /// </summary>
    public void Add(UiElement item, int row, int column)
    {
        UiGalleryItem? galleryItem = GetGalleryItem(row, column);

            // Crea el elemento si no estaba en la lista
            if (galleryItem is null)
            {
                // Crea el componente de la galería
                galleryItem = new UiGalleryItem(this, row, column);
                // ... y lo añade a la lista de elementos
                _items.Add(galleryItem);
            }
            // Añade el elemento
            galleryItem.Add(item);
    }

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeData? GetItem<TypeData>(string id) where TypeData : UiElement
    {
        // Busca el elemento en la lista o en sus componetes hijo
        foreach (UiElement item in _items)
            if (item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase) && item is TypeData converted)
                return converted;
            else if (item is Interfaces.IComponentPanel panel)
            {
                TypeData? child = panel.GetItem<TypeData>(id);

                    if (child is not null)
                        return child;
            }
        // Si ha llegado hasta aquí es porque no ha encontrado nada
        return null;
    }

    /// <summary>
    ///     Limpia la galería
    /// </summary>
	public void Clear()
	{
		_items.Clear();
	}

    /// <summary>
    ///     Actualiza los elementos
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        foreach (UiGalleryItem item in _items)
            if (item.Visible)
                item.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja los elementos
    /// </summary>
	public override void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext)
	{
        for (int row = FirstVisibleRow; row < FirstVisibleRow + ViewportRows; row++)
            for (int column = FirstVisibleColumn; column < FirstVisibleColumn + ViewportColumns; column++)
            {
                UiGalleryItem? gridItem = GetGalleryItem(row, column);

                    // Dibuja los elementos
                    if (gridItem is not null && gridItem.Visible)
                        gridItem.Draw(renderingManager, gameContext);
            }
	}

    /// <summary>
    ///     Obtiene el elemento asociado a una celda
    /// </summary>
    public UiGalleryItem? GetGalleryItem(int row, int column) => _items.FirstOrDefault(item => item.Row == row && item.Column == column);

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
    ///     Mueve la posición a la derecha X columnas
    /// </summary>
	public void MoveRight(int columns)
	{
		MoveColumns(columns);
	}

    /// <summary>
    ///     Mueve la posición a la izquierda X columnas
    /// </summary>
	public void MoveLeft(int columns)
	{
        MoveColumns(-columns);
	}

    /// <summary>
    ///     Mueve columnas a la izquierda o la derecha
    /// </summary>
    public void MoveColumns(int columns)
    {
		FirstVisibleColumn = MathHelper.Clamp(FirstVisibleColumn + columns, 0, Columns - ViewportColumns);
    }

    /// <summary>
    ///     Mueve la posición hacia arriba X filas
    /// </summary>
	public void MoveUp(int rows)
	{
		MoveRows(-rows);
	}

    /// <summary>
    ///     Mueve la posición hacia abajo X filas
    /// </summary>
	public void MoveDown(int rows)
	{
        MoveRows(rows);
	}

    /// <summary>
    ///     Mueve filas hacia arriba o abajo
    /// </summary>
    public void MoveRows(int rows)
    {
		FirstVisibleRow = MathHelper.Clamp(FirstVisibleRow + rows, 0, Rows - ViewportRows);
    }

    /// <summary>
    ///     Indica si se pueden mover a la derecha un número de columnas
    /// </summary>
    public bool CanMoveRight(int columns) => FirstVisibleColumn + columns < Columns;

    /// <summary>
    ///     Indica si se pueden mover a la izquierda un número de columnas
    /// </summary>
    public bool CanMoveLeft(int columns) => FirstVisibleColumn - columns >= 0;

    /// <summary>
    ///     Número total de columnas en el grid
    /// </summary>
    public int Columns 
    { 
        get { return _columns; } 
        set { CheckUpdated(ref _columns, value); }
    }
        
    /// <summary>
    ///     Número total de filas en el grid
    /// </summary>
    public int Rows
    { 
        get { return _rows; } 
        set { CheckUpdated(ref _rows, value); }
    }
        
    /// <summary>
    ///     Número de columnas visibles en pantalla
    /// </summary>
    public int ViewportColumns
    { 
        get { return _viewportColumns; } 
        set { CheckUpdated(ref _viewportColumns, value); }
    }
        
    /// <summary>
    ///     Número de filas visibles en pantalla
    /// </summary>
    public int ViewportRows
    { 
        get { return _viewportRows; } 
        set { CheckUpdated(ref _viewportRows, value); }
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