using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Managers.Input;

/// <summary>
///     Manager de entradas de teclado, ratón, etc...
/// </summary>
public class InputManager
{
    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameContext gameContext)
    {
        KeyboardManager.Update(gameContext);
        MouseManager.Update(gameContext);
        GamePadManager.Update(gameContext);
    }

    /// <summary>
    ///     Comprueba si se ha cumplido una acción
    /// </summary>
    public bool IsAction(string action, InputMappings.Status? status = null) => IsAction(action, PlayerIndex.One, status);

    /// <summary>
    ///     Comprueba si se ha cumplido una acción
    /// </summary>
    public bool IsAction(string action, PlayerIndex playerIndex, InputMappings.Status? status = null)
    {
        InputMappings? mapping = Mappings.FirstOrDefault(item => item.Name.Equals(action, StringComparison.CurrentCultureIgnoreCase) && item.Enabled);

            // Comprueba si se ha ejecutado la acción
            if (mapping is not null)
                return mapping.Check(this, playerIndex, status);
            // Si ha llegado hasta aquí es porque no ha encontrado nada
            return false;
    }

    /// <summary>
    ///     Manager de teclado
    /// </summary>
    public KeyboardController.KeyboardManager KeyboardManager { get; } = new();

    /// <summary>
    ///     Manager de ratón
    /// </summary>
    public MouseController.MouseManager MouseManager { get; } = new();

    /// <summary>
    ///     Manager de ratón
    /// </summary>
    public GamePadController.GamePadManager GamePadManager { get; } = new();

    /// <summary>
    ///     Mapeos de teclas
    /// </summary>
    public List<InputMappings> Mappings { get; }= [];
}
