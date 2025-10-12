namespace Bau.Libraries.BauGame.Engine.Base;

/// <summary>
///     Base para los diccionarios
/// </summary>
public class DictionaryModel<TypeData>
{
    /// <summary>
    ///     Añade un elemento a la definición
    /// </summary>
    public void Add(string name, TypeData item)
    {
        if (Items.ContainsKey(name))
            Items[name] = item;
        else
            Items.Add(name, item);
    }

	/// <summary>
	///     Añade una serie de elementos
	/// </summary>
	public void AddRange(List<(string name, TypeData item)> items)
	{
        foreach ((string name, TypeData item) in items)
            Add(name, item);
	}

    /// <summary>
    ///     Obtiene un elemento por su nombre
    /// </summary>
    public TypeData? Get(string name)
    {
        if (Items.TryGetValue(name, out TypeData? region))
            return region;
        else
            return default;
    }

    /// <summary>
    ///     Elimina un elemento
    /// </summary>
    public void Remove(string name)
    {
        if (Items.ContainsKey(name))
            Items.Remove(name);
    }

	/// <summary>
	///     Elementos del <see cref="TypeData"/>
	/// </summary>
	public Dictionary<string, TypeData> Items { get; } = new(StringComparer.CurrentCultureIgnoreCase);
}