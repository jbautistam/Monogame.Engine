using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Clase base para un elemento de interface de usuario
/// </summary>
public abstract class UiElement(AbstractUserInterfaceLayer layer, UiPosition position)
{
    // Variables privadas
    private bool _transformDirty = true;

    /// <summary>
    ///     Invalida los datos
    /// </summary>
    public void Invalidate()
    {
        _transformDirty = true;
    }

    /// <summary>
    ///     Cálculo del layout
    /// </summary>
    internal void ComputeScreenBounds(Rectangle parentBounds)
    {
        // Asigna el margen y el paddir
        if (!string.IsNullOrWhiteSpace(Style) && (Position.Margin is null || Position.Padding is null))
        {
            UiStyle? style = Layer.Styles.GetDefault(Style);

                // Asigna el margen si no tenía
                if (style is not null && Position.Margin is null)
                    Position.Margin = style.Margin;
                // Asigna el espaciado interno si no tenía
                if (style is not null && Position.Padding is null)
                    Position.Padding = style.Padding;
        }
        // Calcula los límites en la pantalla
        Position.ComputeLayout(parentBounds);
        // Calcula el layout del propio elemento
        ComputeScreenBoundsSelf();
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected abstract void ComputeScreenBoundsSelf();

    /// <summary>
    ///     Obtiene los límites del elemento padre
    /// </summary>
    protected Rectangle GetParentBounds()
    {
        if (Parent is null)
            return Layer.Bounds;
        else
            return Parent.Position.ContentBounds;
    }

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        // Recalcula los límites si es necesario
        if (_transformDirty)
        {
            ComputeScreenBounds(GetParentBounds());
            _transformDirty = false;
        }
        // Actualiza el elemento
        if (Enabled)
            UpdateSelf(gameContext);
    }

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
    public abstract void UpdateSelf(Managers.GameContext gameContext);

    /// <summary>
    ///     Dibuja el elemento
    /// </summary>
    public abstract void Draw(Camera2D camera, Managers.GameContext gameContext);

    /// <summary>
    ///     Manager del elemento
    /// </summary>
    public AbstractUserInterfaceLayer Layer { get; } = layer;

    /// <summary>
    ///     Identificador del elemento
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Elemento padre
    /// </summary>
    public UiElement? Parent { get; set; }

    /// <summary>
    ///     Indica si el elemento es visible
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    ///     Indica si el elemento está activo
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     Tag asociada
    /// </summary>
    public object? Tag { get; set; }

    /// <summary>
    ///     Posición
    /// </summary>
    public UiPosition Position { get; set; } = position;

    /// <summary>
    ///     Estilo del elemento
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    ///     Orden de dibujo
    /// </summary>
    public float ZIndex { get; set; }
}
