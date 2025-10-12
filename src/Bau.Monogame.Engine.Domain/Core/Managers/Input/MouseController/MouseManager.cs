using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Managers.Input.MouseController;

/// <summary>
///     Manager de entradas de ratón
/// </summary>
public class MouseManager
{
	// Variables privadas
	private MouseStatus? _previous;
	private MouseStatus _current = new();

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameTime gameTime)
    {
        // Clona el estado actual sobre el previo
        _previous = _current.Clone();
        // Obtiene las teclas pulsadas actualmente
        _current.Update(gameTime);
    }

    /// <summary>
    ///     Comprueba si se ha pulsado una tecla
    /// </summary>
    public bool IsPressed(MouseStatus.MouseButton button) => _current.IsPressed(button);

    /// <summary>
    ///     Comprueba si una tecla estaba pulsada
    /// </summary>
    public bool WasPressed(MouseStatus.MouseButton button) => _previous?.IsPressed(button) ?? false;

    /// <summary>
    ///     Indica si se acaba de presionar la tecla
    /// </summary>
    public bool JustPressed(MouseStatus.MouseButton button) => IsPressed(button) && !WasPressed(button);

    /// <summary>
    ///     Indica si se acaba de liberar la tecla
    /// </summary>
    public bool JustReleased(MouseStatus.MouseButton button) => !IsPressed(button) && WasPressed(button);

    /// <summary>
    ///     Posición del ratón
    /// </summary>
    public Vector2 MousePosition => _current.MousePosition;

    /// <summary>
    ///     Delta de las posiciones del ratón
    /// </summary>
    public Vector2 MouseDelta
    {
        get
        {
            if (_previous is null)
                return _current.MousePosition;
            else
                return _current.MousePosition - _previous.MousePosition;
        }
    }

    /// <summary>
    ///     Delta de scroll botón de la rueda del ratón
    /// </summary>
    public int ScrollWheelDelta => _current.ScrollWheelValue - _previous?.ScrollWheelValue ?? 0;

    /// <summary>
    ///     Delta de scroll horizontal botón de la rueda del ratón
    /// </summary>
    public int HorizontalScrollWheelDelta => _current.HorizontalScrollWheelValue - _previous?.HorizontalScrollWheelValue ?? 0;
}
