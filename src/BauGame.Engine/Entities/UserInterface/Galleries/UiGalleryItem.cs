using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Entities.UserInterface.Galleries;

/// <summary>
///		Elemento de un <see cref="UiGallery"/>
/// </summary>
public class UiGalleryItem : UiElement, Interfaces.IComponentPanel
{
	public UiGalleryItem(UiGallery gallery, int row, int column) : base(gallery.Layer, new UiPosition(0, 0, 1, 1))
	{
		Parent = gallery;
		Row = row;
		Column = column;
	}

	/// <summary>
	///		Añade un elemento a la lista
	/// </summary>
	public void Add(UiElement item)
	{
		item.Parent = this;
		Items.Add(item);
	}

    /// <summary>
    ///     Calcula la posición en pantalla
    /// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
		foreach (UiElement item in Items)
			item.ComputeScreenBounds(Position.ContentBounds);
	}

	/// <summary>
	///		Actualiza el elemento
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		foreach (UiElement item in Items)
			if (item.Enabled)
				item.Update(gameContext);
	}

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeData? GetItem<TypeData>(string id) where TypeData : UiElement
    {
        // Busca el elemento en la lista o en sus componetes hijo
        foreach (UiElement item in Items)
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
    ///     Dibuja el elemento
    /// </summary>
	public override void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext)
	{
        // Ordena los elementos
        Items.Sort((first, second) => first.ZIndex.CompareTo(second.ZIndex));
		// Dibuja los elementos
		foreach (UiElement item in Items)
			if (item.Visible)
				item.Draw(renderingManager, gameContext);
	}

	/// <summary>
	///		Fila
	/// </summary>
	public int Row { get; }

	/// <summary>
	///		Columna
	/// </summary>
	public int Column { get; }

	/// <summary>
	///		Elemento
	/// </summary>
	public List<UiElement> Items { get; } = [];
}