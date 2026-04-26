namespace Bau.BauEngine.Entities.UserInterface.Grids;

/// <summary>
///     Grid
/// </summary>
public class UiGrid : UiElement, Interfaces.IComponentPanel
{
    public UiGrid(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
    {
        Definitions = new GridDefinitionModel(this);
        Items = new GridItemCollection(this);
    }

    /// <summary>
    ///     Calcula el layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
    {
        // Actualiza el layout de filas y columnas
        Definitions.UpdateLayout(Position.ContentBounds.Width, Position.ContentBounds.Height, ColumnSpacing, RowSpacing);
        // Actualiza el layout de los elementos
        Items.UpdateLayout();
    }

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeData? GetItem<TypeData>(string id) where TypeData : UiElement
    {
        // Busca el elemento en la lista o en sus componetes hijo
        foreach (GridItemModel gridItem in Items)
            if (gridItem.Item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase) && gridItem.Item is TypeData converted)
                return converted;
            else if (gridItem.Item is Interfaces.IComponentPanel panel)
            {
                TypeData? child = panel.GetItem<TypeData>(id);

                    if (child is not null)
                        return child;
            }
        // Si ha llegado hasta aquí es porque no ha encontrado nada
        return null;
    }

    /// <summary>
    ///     Actualiza el grid
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        Items.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja el grid
    /// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
	{
        // Dibujar el estilo
        Layer.DrawStyle(renderingManager, Style, Styles.UiStyle.StyleType.Normal, Position.ContentBounds, gameContext);
        // Dibuja los elementos
        Items.Draw(renderingManager, gameContext);
    }

	/// <summary>
	///     Definición de las filas y columnas del grid
	/// </summary>
	public GridDefinitionModel Definitions { get; }

    /// <summary>
    ///     Elementos del grid
    /// </summary>
    public GridItemCollection Items { get; }

    /// <summary>
    ///     Espaciado entre columnas (en pixels)
    /// </summary>
    public int ColumnSpacing { get; set; }

    /// <summary>
    ///     Espaciado entre filas (en pixels)
    /// </summary>
    public int RowSpacing { get; set; }
}
