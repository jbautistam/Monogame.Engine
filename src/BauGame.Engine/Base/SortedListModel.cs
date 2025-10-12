namespace Bau.Libraries.BauGame.Engine.Base;

/// <summary>
///     Base para las listas ordenadas
/// </summary>
public class SortedListModel<TypeData> where TypeData : class
{
    // Registros
    public record SortedItem(string Name, TypeData Item, int Position);

    /// <summary>
    ///     Añade un elemento a la definición
    /// </summary>
    public void Add(string name, TypeData item, int position)
    {
        // Añade el elemento
        Items.Add(new SortedItem(name, item, position));
        // y ordena
        Sort();
    }

	/// <summary>
	///     Añade una serie de elementos
	/// </summary>
	public void AddRange(List<SortedItem> items)
	{
        // Añade los elementos
        items.AddRange(items);
        // Ordena los elementos
        Sort();
	}

    /// <summary>
    ///     Ordena los elementos de la colección
    /// </summary>
    private void Sort()
    {
        Items.Sort((first, second) => first.Position.CompareTo(second.Position));
    }

    /// <summary>
    ///     Obtiene un elemento por su nombre
    /// </summary>
    public TypeData? Get(string name) => Items.FirstOrDefault(item => item.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.Item;

    /// <summary>
    ///     Elimina un elemento
    /// </summary>
    public void Remove(string name)
    {
        for (int index = Items.Count - 1; index >= 0; index--)
            if (Items[index].Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                Items.RemoveAt(index);
    }

	/// <summary>
	///     Elementos del <see cref="TypeData"/>
	/// </summary>
	public List<SortedItem> Items { get; } = new();
}