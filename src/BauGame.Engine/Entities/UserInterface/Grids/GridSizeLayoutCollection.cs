namespace Bau.BauEngine.Entities.UserInterface.Grids;

/// <summary>
///     Lista de filas o columnas del grid
/// </summary>
public class GridSizeLayoutCollection : List<GridSizeLayoutModel>
{
    /// <summary>
    ///     Añade una definición a la colección
    /// </summary>
    public GridSizeLayoutModel Add(float size, float minSize = 0, float maxSize = 0, bool visible = true)
    {
        GridSizeLayoutModel gridSize = new()
                                        { 
                                            RelativeSize = size,
                                            IsVisible = visible,
                                            MinSize = minSize,
                                            MaxSize = maxSize
                                        };

            // Añade el tamaño a la lista
            Add(gridSize);
            // Devuelve el tamaño añadido
            return gridSize;
    }

    /// <summary>
    ///     Obtiene un elemento de la lista
    /// </summary>
	public GridSizeLayoutModel? Get(int index)
	{
        if (index >= 0 && index < Count)
            return this[index];
        else
            return null;
	}

    /// <summary>
    ///     Calcula el tamaño de los elementos
    /// </summary>
	public float ComputeSize(int from, int span)
	{
        float total = 0;

            // Suma los anchos de columnas
		    for (int index = from; index < from + span; index++)
            {
                GridSizeLayoutModel? item = Get(index);

                    if (item is not null && item.IsVisible)
                        total += item.CalculatedSize;
            }
            // Devuelve el total
            return total;
	}

    /// <summary>
    ///     Actualiza el layout con el tamaño del grid
    /// </summary>
    public void UpdateLayout(int gridSize, float spacing)
    {
        if (Count > 0)
        {
            int availableSpace = gridSize - (int) (spacing * (Count - 1));
            float totalWeight = ComputeTotalWeight();
            float currentPos = 0;

                // Calcula los tamaños relativos de cada definición
                foreach (GridSizeLayoutModel definition in this)
                    if (definition.IsVisible)
                    {
                        float size = availableSpace * definition.RelativeSize / totalWeight;

                            // Normaliza los tamaños
                            if (definition.MinSize > 0) 
                                size = Math.Max(size, definition.MinSize);
                            if (definition.MaxSize > 0) 
                                size = Math.Min(size, definition.MaxSize);
                            // Asigna el valor calculado
                            definition.CalculatedSize = size;
                            definition.CalculatedPosition = currentPos;
                            // Cambia la posición actual
                            currentPos += size + spacing;
                    }
                    else
                    {
                        definition.CalculatedSize = 0;
                        definition.CalculatedPosition = currentPos;
                    }
        }
    }

    /// <summary>
    ///     Modifica la visibilidad de un elemento
    /// </summary>
	public bool SetVisibility(int index, bool visible)
	{
        if (index >= 0 && index < Count && this[index].IsVisible != visible)
        {
            this[index].IsVisible = visible;
            return true;
        }
        else
            return false;
	}

    /// <summary>
    ///     Modifica el tamaño relativo de un elemento
    /// </summary>
	public bool SetSize(int index, float size)
	{
        if (index >= 0 && index < Count && this[index].RelativeSize != size)
        {
            this[index].RelativeSize = size;
            return true;
        }
        else
            return false;
	}

	/// <summary>
	///     Calcula el peso total de las columnas visibles
	/// </summary>
	private float ComputeTotalWeight()
    {
        float total = 0;

            // Calcula el peso total
            foreach (GridSizeLayoutModel gridSize in this)
                if (gridSize.IsVisible)
                    total += gridSize.RelativeSize;
            // Normaliza el peso
            if (total <= 0)
                total = 1;
            // Devuelve el peso total
            return total;
    }
}