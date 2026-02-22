namespace Bau.Libraries.BauGame.Engine.Entities.Common.Collections;

/// <summary>
///		Lista que permite añadir y borrar elementos de forma segura
/// </summary>
public abstract class SecureList<TypeData> where TypeData : ISecureListItem
{
	// Variables privadas
	private List<TypeData> _itemsToAdd = [];
	private List<(TypeData Item, TimeSpan TimeToDestroy)> _itemsToRemove = [];
	private int _countAllLife = 0;

	/// <summary>
	///		Añade un elemento (lo prepara para añadirlo más tarde)
	/// </summary>
	public void Add(TypeData item)
	{
		_itemsToAdd.Add(item);
	}

    /// <summary>
    ///     Marca un elemento para eliminar
    /// </summary>
	public void MarkToDestroy(TypeData item, TimeSpan timeToDestroy)
	{
        _itemsToRemove.Add((item, timeToDestroy));
	}

	/// <summary>
	///		Actualiza los elementos
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
		// Elimina los elementos pendientes de eliminar
		RemoveOld(gameContext);
		// Añade los elementos pendientes
		AddPendingItems();
		// Actualiza el contenido
		UpdateSelf(gameContext);
	}

	/// <summary>
	///		Actualiza el contenido de los elementos
	/// </summary>
	protected abstract void UpdateSelf(Managers.GameContext gameContext);

	/// <summary>
	///		Enumera los elementos de la lista
	/// </summary>
	public IEnumerable<TypeData> Enumerate()
	{
		foreach (TypeData item in Items)
			yield return item;
	}

	/// <summary>
	///		Añade los elementos pendientes
	/// </summary>
	private void AddPendingItems()
	{
		// Añade los elementos pendientes
		foreach (TypeData item in _itemsToAdd)
		{
			// Incrementa el número de elementos añadidos a la lista
			_countAllLife++;
			// Añade el elemento
			Items.Add(item);
			// Indica a la lista que se ha añadido el elemento y que se inicialize
			Added(item);
			item.Start();
		}
		// Limpia la lista de elementos pendientes
		_itemsToAdd.Clear();
	}

	/// <summary>
	///		Indica que se ha añadido un elemento a la lista
	/// </summary>
	protected abstract void Added(TypeData item);

    /// <summary>
    ///     Elimina los elementos pendientes
    /// </summary>
	private void RemoveOld(Managers.GameContext gameContext)
	{
        for (int index = _itemsToRemove.Count - 1; index >= 0; index--)
            if (gameContext.GameTime.TotalGameTime > _itemsToRemove[index].TimeToDestroy)
            {
                // Indica al elemento que ha acabado su vida y a la lista que se ha eliminado
				_itemsToRemove[index].Item.End(gameContext);
				Removed(_itemsToRemove[index].Item);
                // Elimina el elemento de la lista
				Items.Remove(_itemsToRemove[index].Item);
                // Elimina el elemento de la lista de elementos a eliminar
                _itemsToRemove.RemoveAt(index);
            }
	}

	/// <summary>
	///		Indica a la lista que se ha eliminado el elemento
	/// </summary>
	protected abstract void Removed(TypeData item);

	/// <summary>
	///		Ordena los elementos de la lista
	/// </summary>
	public void Sort(Func<TypeData, TypeData, int> compare)
	{
		Items.Sort((first, second) => compare(first, second));
	}

	/// <summary>
	///		Limpia los elementos de la lista
	/// </summary>
	public void Clear()
	{
		foreach (TypeData item in Items)
			MarkToDestroy(item, TimeSpan.FromMilliseconds(3));
	}

	/// <summary>
	///		Número de elementos
	/// </summary>
	public int Count => Items.Count();

	/// <summary>
	///		Número de elementos durante toda la vida de la lista (por ejemplo, para calcular ZOrders)
	/// </summary>
	public int CountAllLife => _countAllLife;

	/// <summary>
	///		Elementos de la lista segura
	/// </summary>
	private List<TypeData> Items { get; } = [];
}
