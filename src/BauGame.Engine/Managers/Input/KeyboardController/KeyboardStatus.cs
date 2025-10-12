using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bau.Libraries.BauGame.Engine.Managers.Input.KeyboardController;

/// <summary>
///     Manager de teclas pulsadas
/// </summary>
public class KeyboardStatus
{
    /// <summary>
    ///     Clona el objeto
    /// </summary>
	internal KeyboardStatus Clone()
	{
        KeyboardStatus state = new();

            // Añade las teclas pulsadas
            state.PressedKeys.AddRange(PressedKeys);
            // Devuelve el estado
            return state;
	}

    /// <summary>
    ///     Comprueba si se ha pulsado una tecla
    /// </summary>
    public bool IsPressed(Keys key) => PressedKeys.Contains(key);

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameTime gameTime)
    {
        // Limpia las teclas pulsadas la última vez
        PressedKeys.Clear();
        // Añade las teclas pulsadas
        PressedKeys.AddRange(Keyboard.GetState().GetPressedKeys());
    }

	/// <summary>
	///     Teclas pulsadas en la última comprobación de estado
	/// </summary>
	public List<Keys> PressedKeys = [];
}
