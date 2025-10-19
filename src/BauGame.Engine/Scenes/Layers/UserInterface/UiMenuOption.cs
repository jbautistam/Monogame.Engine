using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///     Opción de menú
/// </summary>
public class UiMenuOption(AbstractUserInterfaceLayer layer, UiPosition position, int optionId) : UiElement(layer, position)
{
    // Variables privadas
    private bool _isInitialized;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds() 
    {
        //Background?.ComputeScreenBounds(Position.ScreenBounds);
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void Update(Managers.GameContext gameContext) 
    {
        if (!_isInitialized)
        {
            // Carga la fuente
            if (!string.IsNullOrWhiteSpace(Font))
                SpriteFont = Layer.Scene.LoadSceneAsset<SpriteFont>(Font);
            // Indica que ya está inicializado
            _isInitialized = true;
            // Actualiza el contenido
            Background?.Update(gameContext);
        }
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        if (!string.IsNullOrEmpty(Text) && SpriteFont is not null)
        {
            Vector2 textSize = SpriteFont.MeasureString(Text);
            Vector2 textPosition = new(Position.ScreenPaddedBounds.X + (Position.ScreenPaddedBounds.Width - textSize.X) / 2, 
                                       Position.ScreenPaddedBounds.Y + (Position.ScreenPaddedBounds.Height - textSize.Y) / 2);

                // Dibuja la textura de fondo si existe
                Background?.ComputeScreenBounds(Position.ScreenBounds);
                Background?.Draw(camera, gameContext);
                // Dibuja el texto
                camera.SpriteBatchController.DrawString(SpriteFont, Text, textPosition, Color * Opacity);
        }
    }

    /// <summary>
    ///     Id de opción
    /// </summary>
    public int OptionId { get; } = optionId;

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
    ///     Color
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Fondo normal
    /// </summary>
    public UiBackground? Background { get; set; }
}