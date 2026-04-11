using Microsoft.Xna.Framework;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///     Opción de menú
/// </summary>
public class UiMenuOption(UiMenu menu, UiPosition position, int optionId) : UiElement(menu.Layer, position)
{
    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext) 
    {
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        UiStyle? style = GetStyle();

            if (!string.IsNullOrEmpty(Text) && style?.Font is not null)
            {
                Vector2 textSize = style.Font.MeasureString(Text);
                Vector2 textPosition = new(Position.ContentBounds.X + (Position.ContentBounds.Width - textSize.X) / 2, 
                                           Position.ContentBounds.Y + (Position.ContentBounds.Height - textSize.Y) / 2);

                    // Dibuja la textura de fondo si existe
                    Layer.DrawStyle(renderingManager, Style, State, Position.Bounds, gameContext);
                    // Dibuja el texto
                    renderingManager.SpriteTextRenderer.DrawString(style.Font, Text, textPosition, (style?.StyleText?.Color ?? Color.White) * (style?.StyleText?.Opacity ?? 1));
            }
    }

    /// <summary>
    ///     Obtiene el estilo correspondiente a la opción
    /// </summary>
    private UiStyle? GetStyle()
    {
        if (!string.IsNullOrWhiteSpace(Style))
            return Menu.Layer.Styles.GetStyle(Style, State);
        else
            return null;
    }

    /// <summary>
    ///     Id de opción
    /// </summary>
    public int OptionId { get; } = optionId;

    /// <summary>
    ///     Menú
    /// </summary>
    public UiMenu Menu { get; } = menu;

    /// <summary>
    ///     Texto
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     Indica si el cursor está sobre el botón
    /// </summary>
    public bool IsHovered { get; set; }

    /// <summary>
    ///     Indica si el botón está presionado
    /// </summary>
    public bool IsPressed { get; set; }

    /// <summary>
    ///     Indica si está seleccionado
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    ///     Estado de la opción
    /// </summary>
    public UiStyle.StyleType State 
    { 
        get
        {
            if (!Enabled)
                return UiStyle.StyleType.Disabled;
            else if (IsPressed)
                return UiStyle.StyleType.Pressed;
            else if (IsSelected)
                return UiStyle.StyleType.Selected;
            else if (IsHovered)
                return UiStyle.StyleType.Hover;
            else
                return UiStyle.StyleType.Normal;
        }
    }
}