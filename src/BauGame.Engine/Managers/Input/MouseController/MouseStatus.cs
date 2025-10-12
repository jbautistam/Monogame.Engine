using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bau.Libraries.BauGame.Engine.Managers.Input.MouseController;

/// <summary>
///     Manager de teclas pulsadas
/// </summary>
public class MouseStatus
{
    /// <summary>
    ///     Botón del ratón
    /// </summary>
    public enum MouseButton
    {
        /// <summary>Botón izquierdo</summary>
        Left,
        /// <summary>Botón derecho</summary>
        Right,
        /// <summary>Botón intermedio</summary>
        Middle
    }

    /// <summary>
    ///     Clona el objeto
    /// </summary>
	internal MouseStatus Clone()
	{
        MouseStatus state = new()
                              {
                                MousePosition = MousePosition,
                                HorizontalScrollWheelValue = HorizontalScrollWheelValue,
                                ScrollWheelValue = ScrollWheelValue
                              };

            // Añade las teclas pulsadas
            state.PressedButtons.AddRange(PressedButtons);
            // Devuelve el estado
            return state;
	}

    /// <summary>
    ///     Comprueba si se ha pulsado una tecla
    /// </summary>
    public bool IsPressed(MouseButton key) => PressedButtons.Contains(key);

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameTime gameTime)
    {
        MouseState state = Mouse.GetState();

            // Guarda la posición
            MousePosition = new Vector2(state.X, state.Y);
            // Limpia los botones pulsados la última vez
            PressedButtons.Clear();
            // Añade los botones pulsados
            if (state.LeftButton == ButtonState.Pressed)
                PressedButtons.Add(MouseButton.Left);
            if (state.RightButton == ButtonState.Pressed)
                PressedButtons.Add(MouseButton.Right);
            if (state.MiddleButton == ButtonState.Pressed)
                PressedButtons.Add(MouseButton.Middle);
            // Obtiene la posición de la rueda del ratón
            HorizontalScrollWheelValue = state.HorizontalScrollWheelValue;
            ScrollWheelValue = state.ScrollWheelValue;
    }

	/// <summary>
	///     Teclas pulsadas en la última comprobación de estado
	/// </summary>
	public List<MouseButton> PressedButtons { get; } = [];

    /// <summary>
    ///     Posición del ratón
    /// </summary>
    public Vector2 MousePosition { get; private set; }

    /// <summary>
    ///     Scroll de la rueda de ratón horizontal
    /// </summary>
    public int HorizontalScrollWheelValue { get; private set; }

    /// <summary>
    ///     Scroll de la rueda de ratón
    /// </summary>
    public int ScrollWheelValue { get; private set; }
}
