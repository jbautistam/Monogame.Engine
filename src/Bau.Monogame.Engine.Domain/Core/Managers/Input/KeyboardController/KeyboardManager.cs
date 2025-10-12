using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bau.Monogame.Engine.Domain.Core.Managers.Input.KeyboardController;

/// <summary>
///     Manager de entradas de teclado
/// </summary>
public class KeyboardManager
{
	// Variables privadas
	private KeyboardStatus? _previous;
	private KeyboardStatus _current = new();

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
    public bool IsPressed(Keys key) => _current.IsPressed(key);

    /// <summary>
    ///     Comprueba si una tecla estaba pulsada
    /// </summary>
    public bool WasPressed(Keys key) => _previous?.IsPressed(key) ?? false;

    /// <summary>
    ///     Indica si se acaba de presionar la tecla
    /// </summary>
    public bool JustPressed(Keys key) => IsPressed(key) && !WasPressed(key);

    /// <summary>
    ///     Indica si se acaba de liberar la tecla
    /// </summary>
    public bool JustReleased(Keys key) => !IsPressed(key) && WasPressed(key);
}
