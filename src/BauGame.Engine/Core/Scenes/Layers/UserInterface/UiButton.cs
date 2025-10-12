using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Scenes.Layers.UserInterface;

/// <summary>
///     Control botón para el interface
/// </summary>
public class UiButton : UiElement
{
    // Eventos públicos
    public event EventHandler? Click;
    public event EventHandler? MouseEnter;
    public event EventHandler? MouseLeave;
    public event EventHandler? MouseDown;
    public event EventHandler? MouseUp;
    /// <summary>
    ///     Tipo de estado
    /// </summary>
    public enum ButtonState
    {
        /// <summary>Normal</summary>
        Normal,
        /// <summary>El cursor sobre el botón</summary>
        Hover,
        /// <summary>Presionado</summary>
        Pressed,
        /// <summary>Inhabilitado</summary>
        Disabled,
        /// <summary>Seleccionado</summary>
        Selected
    }

    public UiButton(UserInterfaceLayer layer, UiPosition position) : base(layer, position)
    {
        Position.Padding = new UIMargin(10);
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds() 
    {
        Label?.ComputeScreenBounds(Position.ScreenPaddedBounds);
        Background?.ComputeScreenBounds(Position.ScreenBounds);
        HoverBackground?.ComputeScreenBounds(Position.ScreenBounds);
        PressedBackground?.ComputeScreenBounds(Position.ScreenBounds);
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void Update(GameTime gameTime)
    {
        if (Enabled)
        {
            Vector2 mousePosition = GameEngine.Instance.InputManager.MouseManager.MousePosition;
            bool wasHovered = IsHovered;

                // Indica que por ahora no está pulsado
                IsPressed = false;
                // Comprueba si el ratón está encima del control
                IsHovered = Position.ScreenBounds.Contains(mousePosition);
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
                    if (IsPressed)
                        Click?.Invoke(this, EventArgs.Empty);
                }
                // Actualiza los elementos
                Label?.Update(gameTime);
                Background?.Update(gameTime);
                HoverBackground?.Update(gameTime);
                PressedBackground?.Update(gameTime);
                SelectedBackground?.Update(gameTime);
        }
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    public override void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
        // Dibujar textura de fondo si existe
        GetBackground(State)?.Draw(camera, gameTime);
        // Dibuja el texto
        Label?.Draw(camera, gameTime);

        // Obtiene el fondo adecuado
        UiBackground? GetBackground(ButtonState state)
        {
	        return State switch
	            {
                    ButtonState.Selected => SelectedBackground ?? PressedBackground ?? HoverBackground ?? Background,
		            ButtonState.Hover => HoverBackground ?? Background,
		            ButtonState.Pressed => PressedBackground ?? HoverBackground ?? Background,
		            _ => Background,
	            };
	    }
    }

    /// <summary>
    ///     Etiqueta
    /// </summary>
    public UiLabel? Label { get; set; }

    /// <summary>
    ///     Fondo normal
    /// </summary>
    public UiBackground? Background { get; set; }

    /// <summary>
    ///     Fondo desactivado
    /// </summary>
    public UiBackground? DisabledBackground { get; set; }

    /// <summary>
    ///     Fondo cuando el cursor está sobre el botón
    /// </summary>
    public UiBackground? HoverBackground { get; set; }

    /// <summary>
    ///     Fondo cuando se presiona el botón
    /// </summary>
    public UiBackground? PressedBackground { get; set; }

    /// <summary>
    ///     Fondo cuando se selecciona el botón
    /// </summary>
    public UiBackground? SelectedBackground { get; set; }

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
    public ButtonState State 
    { 
        get
        {
            if (!Enabled)
                return ButtonState.Disabled;
            else if (IsPressed)
                return ButtonState.Pressed;
            else if (IsSelected)
                return ButtonState.Selected;
            else if (IsHovered)
                return ButtonState.Hover;
            else
                return ButtonState.Normal;
        }
    }
}