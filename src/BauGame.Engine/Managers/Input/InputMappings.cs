using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bau.Libraries.BauGame.Engine.Managers.Input;

/// <summary>
///     Mapeo de entradas a acciones
/// </summary>
public class InputMappings(string name)
{
    // Constantes públicas
    public const string DefaultIntroAction = "Intro";
    public const string DefaultMouseClickAction = "MouseClick";
    public const string DefaulQuitAction = "Quit";
    /// <summary>
    ///     Estado que se comprueba
    /// </summary>
    public enum Status
    {
        /// <summary>Comprueba si el botón está pulsado</summary>
        Pressed,
        /// <summary>Comprueba si el botón se acaba de pulsar</summary>
        JustPressed,
        /// <summary>Comprueba si el botón se acaba de liberar</summary>
        JustReleased
    }

    /// <summary>
    ///     Comprueba la acción con las diferentes teclas y botones
    /// </summary>
	public bool Check(InputManager manager, PlayerIndex playerIndex) => Enabled && CheckKeys(manager) || CheckMouse(manager) || CheckGamePad(playerIndex, manager);

    /// <summary>
    ///     Comprueba las teclas definidas en la acción
    /// </summary>
    private bool CheckKeys(InputManager manager)
    {
        int keyChecked = 0;

            // Comprueba el estado de las teclas del mapeo
            foreach ((Status status, Keys key) in KeyboardKeys)
                switch (status)
                {
                    case Status.Pressed:
                            if (manager.KeyboardManager.IsPressed(key))
                                keyChecked++;
                        break;
                    case Status.JustPressed:
                            if (manager.KeyboardManager.JustPressed(key))
                                keyChecked++;
                        break;
                    case Status.JustReleased:
                            if (manager.KeyboardManager.JustReleased(key))
                                keyChecked++;
                        break;
                }
            // Devuelve el valor que indica que todas las teclas están pulsadas
            return KeyboardKeys.Count > 0 && keyChecked == KeyboardKeys.Count;
    }

    /// <summary>
    ///     Comprueba los botones del ratón definidos en la acción
    /// </summary>
    private bool CheckMouse(InputManager manager)
    {
        int keyChecked = 0;

            // Comprueba el estado de los botones del ratón del mapeo
            foreach ((Status status, MouseController.MouseStatus.MouseButton button) in MouseButtons)
                switch (status)
                {
                    case Status.Pressed:
                            if (manager.MouseManager.IsPressed(button))
                                keyChecked++;
                        break;
                    case Status.JustPressed:
                            if (manager.MouseManager.JustPressed(button))
                                keyChecked++;
                        break;
                    case Status.JustReleased:
                            if (manager.MouseManager.JustReleased(button))
                                keyChecked++;
                        break;
                }
            // Devuelve el valor que indica que todos los botones del ratón están pulsados
            return MouseButtons.Count > 0 && keyChecked == MouseButtons.Count;
    }

    /// <summary>
    ///     Comprueba los botones del gamepad definidos en la acción
    /// </summary>
    private bool CheckGamePad(PlayerIndex playerIndex, InputManager manager)
    {
        int keyChecked = 0;

            // Comprueba el estado de los botones del gamepad del mapeo
            foreach ((Status status, Buttons button) in GamepadButtons)
                switch (status)
                {
                    case Status.Pressed:
                            if (manager.GamePadManager.IsPressed(playerIndex, button))
                                keyChecked++;
                        break;
                    case Status.JustPressed:
                            if (manager.GamePadManager.JustPressed(playerIndex, button))
                                keyChecked++;
                        break;
                    case Status.JustReleased:
                            if (manager.GamePadManager.JustReleased(playerIndex, button))
                                keyChecked++;
                        break;
                }
            // Devuelve el valor que indica que todos los botones del ratón están pulsados
            return GamepadButtons.Count > 0 && keyChecked == GamepadButtons.Count;
    }

    /// <summary>
    ///     Nombre del mapeo
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     Teclas asociadas al mapeo
    /// </summary>
    public List<(Status status, Keys key)> KeyboardKeys { get; } = [];

    /// <summary>
    ///     Botones del ratón asociados al mapeo
    /// </summary>
    public List<(Status status, MouseController.MouseStatus.MouseButton button)> MouseButtons { get; } = [];

    /// <summary>
    ///     Botones del teclado
    /// </summary>
    public List<(Status status, Buttons button)> GamepadButtons { get; set; } = [];

    /// <summary>
    ///     Indica si el mapeo está configurado
    /// </summary>
    public bool Enabled => KeyboardKeys.Count > 0 || MouseButtons.Count > 0 || GamepadButtons.Count > 0;
}