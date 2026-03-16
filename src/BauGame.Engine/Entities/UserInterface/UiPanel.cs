using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Componente del interface de usuario para un panel
/// </summary>
public class UiPanel(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position), Interfaces.IComponentPanel
{
    /// <summary>
    ///     Añade un elemento al panel
    /// </summary>
    public void Add(UiElement item)
    {
        // Asigna el elemento padre
        item.Parent = this;
        // Añade el elemento a la lista
        Children.Add(item);
    }

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeData? GetItem<TypeData>(string id) where TypeData : UiElement
    {
        // Busca el elemento en la lista o en sus componetes hijo
        foreach (UiElement item in Children)
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
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
        foreach (UiElement element in Children)
            element.ComputeScreenBounds(Position.ContentBounds);
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext) 
    {
        foreach (UiElement child in Children)
            if (child.Enabled)
                child.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        // Dibuja el borde y el fondo
        Layer.DrawStyle(renderingManager, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
        // Dibuja los elementos hijo
        foreach (UiElement child in Children)
            if (child.Visible)
                child.Draw(renderingManager, gameContext);
    }

    /// <summary>
    ///     Elementos hijo
    /// </summary>
    private List<UiElement> Children { get; } = [];
}
