namespace Bau.Libraries.BauGame.Engine.Entities.Common.Collections;

/// <summary>
///		Lista que permite añadir y borrar elementos de forma segura
/// </summary>
public abstract class SecureList<TypeData> where TypeData : ISecureListItem
{
	// Eventos públicos
	public event EventHandler<SecureListEventArgs<TypeData>>? Added;
	public event EventHandler<SecureListEventArgs<TypeData>>? Removed;
	// Variables privadas
	private List<TypeData> _itemsToAdd = [];
	private List<(TypeData Item, TimeSpan TimeToDestroy)> _itemsToRemove = [];

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
			// Añade el elemento
			Items.Add(item);
			// Lanza el evento que indica que el elemento se ha añadido realmente a la lista segura (para que se pueda inicializar, por ejemplo)
			Added?.Invoke(this, new SecureListEventArgs<TypeData>(item));
		}
		// Limpia la lista de elementos pendientes
		_itemsToAdd.Clear();
	}

    /// <summary>
    ///     Elimina los elementos pendientes
    /// </summary>
	private void RemoveOld(Managers.GameContext gameContext)
	{
        for (int index = _itemsToRemove.Count - 1; index >= 0; index--)
            if (gameContext.GameTime.TotalGameTime > _itemsToRemove[index].TimeToDestroy)
            {
                // Lanza el evento que indica que se ha borrado el elemento
				Removed?.Invoke(this, new SecureListEventArgs<TypeData>(_itemsToRemove[index].Item));
                // Elimina el elemento de la lista
				Items.Remove(_itemsToRemove[index].Item);
                // Elimina el elemento de la lista de elementos a eliminar
                _itemsToRemove.RemoveAt(index);
            }
	}

	/// <summary>
	///		Elementos de la lista segura
	/// </summary>
	private List<TypeData> Items { get; } = [];
}
