using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///     Clase base para un elemento de interface de usuario
/// </summary>
public abstract class UiElement(UserInterfaceLayer layer, UiPosition position)
{
    /// <summary>
    ///     Cálculo del layout
    /// </summary>
    public void ComputeScreenBounds(Rectangle parentBounds)
    {
        // Calcula los límites en la pantalla
        Position.ComputeLayout(parentBounds);
        // Calcula el layout de los hijos
        ComputeScreenComponentBounds();
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected abstract void ComputeScreenComponentBounds();

    /// <summary>
    ///     Actualiza el contenido
    /// </summary>
    public abstract void Update(GameTime gameTime);

    /// <summary>
    ///     Dibuja el elemento
    /// </summary>
    public abstract void Draw(Cameras.Camera2D camera, GameTime gameTime);

    /// <summary>
    ///     Manager del elemento
    /// </summary>
    public UserInterfaceLayer Layer { get; } = layer;

    /// <summary>
    ///     Identificador del elemento
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

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
    ///     Opacidad
    /// </summary>
    public float Opacity { get; set; } = 1.0f;

    /// <summary>
    ///     Orden de dibujo
    /// </summary>
    public float ZIndex { get; set; }
}
