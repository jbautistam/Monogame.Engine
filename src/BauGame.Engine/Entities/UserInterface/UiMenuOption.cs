using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Opción de menú
/// </summary>
public class UiMenuOption(UiMenu menu, UiPosition position, int optionId) : UiElement(menu.Layer, position)
{
    // Variables privadas
    private bool _isInitialized;

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
        if (!_isInitialized)
        {
            // Carga la fuente
            if (!string.IsNullOrWhiteSpace(Font))
                SpriteFont = Layer.Scene.LoadSceneAsset<SpriteFont>(Font);
            // Indica que ya está inicializado
            _isInitialized = true;
        }
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        if (!string.IsNullOrEmpty(Text) && SpriteFont is not null)
        {
            Vector2 textSize = SpriteFont.MeasureString(Text);
            Vector2 textPosition = new(Position.ContentBounds.X + (Position.ContentBounds.Width - textSize.X) / 2, 
                                       Position.ContentBounds.Y + (Position.ContentBounds.Height - textSize.Y) / 2);
            UiStyle? style = GetStyle();

                // Dibuja la textura de fondo si existe
                Layer.DrawStyle(camera, Style, State, Position.Bounds, gameContext);
                // Dibuja el texto
                camera.SpriteBatchController.TextRenderer.DrawString(SpriteFont, Text, textPosition, (style?.Color ?? Color.White) * (style?.Opacity ?? 1));
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
    ///     Nombre de la fuente
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Fuente del texto
    /// </summary>
    private SpriteFont? SpriteFont { get; set; }

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