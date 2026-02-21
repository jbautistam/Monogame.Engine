using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Control botón para el interface
/// </summary>
public class UiButton : UiElement
{
    // Eventos públicos
    public event EventHandler? Click;
    public event EventHandler? MouseEnter;
    public event EventHandler? MouseLeave;
    // Variables privadas
    private float _timeFromLastClick;

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
    public override void UpdateSelf(Managers.GameContext gameContext)
    {
        if (Enabled)
        {
            Vector2 mousePosition = GameEngine.Instance.InputManager.MouseManager.MousePosition;
            bool wasHovered = IsHovered;

                // Incrementa el tiempo pasado desde el último click
                _timeFromLastClick += gameContext.DeltaTime;
                // Indica que por ahora no está pulsado
                IsPressed = false;
                // Comprueba si el ratón está encima del control
                IsHovered = Position.Bounds.Contains(mousePosition);
                // Detecta cambios de hover
                if (IsHovered && !wasHovered)
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                else if (!IsHovered && wasHovered)
                    MouseLeave?.Invoke(this, EventArgs.Empty);
                // Detecta clicks
                if (IsHovered)
                {
                    // Indica si se ha pulsado el elemento
                    IsPressed = GameEngine.Instance.InputManager.MouseManager.IsPressed(Managers.Input.MouseController.MouseStatus.MouseButton.Left);
                    // Lanza el evento de pulsación
                    if (IsPressed && _timeFromLastClick > 1f)
                    {
                        Click?.Invoke(this, EventArgs.Empty);
                        _timeFromLastClick = 0;
                    }
                }
                // Actualiza los elementos
                Label?.UpdateSelf(gameContext);
        }
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        // Dibujar textura de fondo si existe
        Layer.DrawStyle(camera, Style, State, Position.ContentBounds, gameContext);
        // Dibuja el texto
        Label?.Draw(camera, gameContext);
    }

    /// <summary>
    ///     Etiqueta
    /// </summary>
    public UiLabel? Label { get; set; }

    /// <summary>
    ///     Indica si el cursor está sobre el botón
    /// </summary>
    public bool IsHovered { get; private set; }

    /// <summary>
    ///     Indica si el botón está presionado
    /// </summary>
    public bool IsPressed { get; private set; }

    /// <summary>
    ///     Indica si está seleccionado
    /// </summary>
    public bool IsSelected { get; private set; }

    /// <summary>
    ///     Estado del botón
    /// </summary>
    public Styles.UiStyle.StyleType State 
    { 
        get
        {
            if (!Enabled)
                return Styles.UiStyle.StyleType.Disabled;
            else if (IsPressed)
                return Styles.UiStyle.StyleType.Pressed;
            else if (IsSelected)
                return Styles.UiStyle.StyleType.Selected;
            else if (IsHovered)
                return Styles.UiStyle.StyleType.Hover;
            else
                return Styles.UiStyle.StyleType.Normal;
        }
    }
}