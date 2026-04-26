using Bau.BauEngine.Scenes.Layers;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///     Control botón para el interface
/// </summary>
public class UiButton : UiElementClickable
{
    // Variables privadas
    private UiLabel? _label;

    public UiButton(AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
    {
        Position.Padding = new UiMargin(10);
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
        Label?.ComputeScreenBounds(Position.ContentBounds);
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateComponent(Managers.GameContext gameContext)
    {
        Label?.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    protected override void DrawComponent(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        Label?.Draw(renderingManager, gameContext);
    }

    /// <summary>
    ///     Etiqueta
    /// </summary>
    public UiLabel? Label 
    { 
        get { return _label; }
        set
        {
            _label = value;
            if (_label is not null)
                _label.Parent = this;
        }
    }
}