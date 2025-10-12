using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Managers.Input;

/// <summary>
///     Manager de entradas de teclado, ratón, etc...
/// </summary>
public class InputManager
{
    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameTime gameTime)
    {
        KeyboardManager.Update(gameTime);
        MouseManager.Update(gameTime);
        GamePadManager.Update(gameTime);
    }

    /// <summary>
    ///     Comprueba si se ha cumplido una acción
    /// </summary>
    public bool IsAction(string action) => IsAction(action, PlayerIndex.One);

    /// <summary>
    ///     Comprueba si se ha cumplido una acción
    /// </summary>
    public bool IsAction(string action, PlayerIndex playerIndex)
    {
        InputMappings? mapping = Mappings.FirstOrDefault(item => item.Name.Equals(action, StringComparison.CurrentCultureIgnoreCase) && item.Enabled);

            // Comprueba si se ha ejecutado la acción
            if (mapping is not null)
                return mapping.Check(this, playerIndex);
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
