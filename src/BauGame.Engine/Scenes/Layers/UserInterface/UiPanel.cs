using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///     Componente del interface de usuario para un panel
/// </summary>
public class UiPanel(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds() 
    {
        // Calcula los límites de fondo y borde
        Border?.ComputeScreenBounds(Position.ScreenBounds);
        Background?.ComputeScreenBounds(Position.ScreenBounds);
        // Calcula los límites de los elementos hijo
        foreach (UiElement element in Children)
            element.ComputeScreenBounds(Position.ScreenPaddedBounds);
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void Update(GameTime gameTime) {}

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
        // Dibuja el borde y el fondo
        Border?.Draw(camera, gameTime);
        Background?.Draw(camera, gameTime);
        // Dibuja los elementos hijo
        foreach (UiElement child in Children)
            if (child.Visible)
                child.Draw(camera, gameTime);
    }

    /// <summary>
    ///     Fondo
    /// </summary>
    public UiBackground? Background { get; set; }

    /// <summary>
    ///     Borde
    /// </summary>
    public UiBorder? Border { get; set; }
    
    /// <summary>
    ///     Elementos hijo
    /// </summary>
    public List<UiElement> Children { get; } = [];
}
