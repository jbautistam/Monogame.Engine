using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bau.Libraries.BauGame.Engine.Core.Managers.Input.GamePadController;

/// <summary>
///     Manager de entradas de gamepad
/// </summary>
public class GamePadManager
{
	// Variables privadas
    private bool _initialized = false;
	private List<GamePadStatus>? _previous = [];
	private List<GamePadStatus> _current = [];

    /// <summary>
    ///     Inicializa los objetos de gamepads
    /// </summary>
    private void InitGamePads()
    {
        if (!_initialized)
        {
            // Busca los gamepads conectados
            for (int index = 0; index < 4; index++)
            {
                PlayerIndex playerIndex = (PlayerIndex) index;
                GamePadState state = GamePad.GetState(playerIndex);
    
                    // Crea los datos
                    if (state.IsConnected)
                        _current.Add(new GamePadStatus(playerIndex));
            }
            // Indica que ya se ha inicializado
            _initialized = true;
        }
    }

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameTime gameTime)
    {
        if (!_initialized)
        {
            // Inicializa los gamepadas conectados
            InitGamePads();
            // Indica que ya se ha inicializado
            _initialized = true;
        }            
        // Clona el estado actual sobre el previo
        _previous = new List<GamePadStatus>();
        foreach (GamePadStatus current in _current)
            _previous.Add(current.Clone());
        // Obtiene las teclas pulsadas actualmente
        foreach (GamePadStatus current in _current)
            current.Update(gameTime);
    }

    /// <summary>
    ///     Comprueba si se ha pulsado una tecla
    /// </summary>
    public bool IsPressed(PlayerIndex playerIndex, Buttons button) => GetGamePadStatus(playerIndex, _current).IsPressed(button);

    /// <summary>
    ///     Comprueba si una tecla estaba pulsada
    /// </summary>
    public bool WasPressed(PlayerIndex playerIndex, Buttons button) => GetGamePadStatus(playerIndex, _previous).IsPressed(button);

    /// <summary>
    ///     Indica si se acaba de presionar la tecla
    /// </summary>
    public bool JustPressed(PlayerIndex playerIndex, Buttons button) => IsPressed(playerIndex, button) && !WasPressed(playerIndex, button);

    /// <summary>
    ///     Indica si se acaba de liberar la tecla
    /// </summary>
    public bool JustReleased(PlayerIndex playerIndex, Buttons button) => !IsPressed(playerIndex, button) && WasPressed(playerIndex, button);

    /// <summary>
    ///     Obtiene el gamepadstatus de un jugador
    /// </summary>
    private GamePadStatus GetGamePadStatus(PlayerIndex playerIndex, List<GamePadStatus>? status) 
    {
        return status?.FirstOrDefault(item => item.PlayerIndex == playerIndex) ?? new GamePadStatus(playerIndex);
    }

    /// <summary>
    ///     Delta de las posiciones del stick delta
    /// </summary>
    public Vector2 GetLeftStickDelta(PlayerIndex playerIndex)
    {
        return GetGamePadStatus(playerIndex, _current).LeftStickPosition - GetGamePadStatus(playerIndex, _previous).LeftStickPosition;
    }

    /// <summary>
    ///     Delta de las posiciones del stick delta
    /// </summary>
    public Vector2 GetRightStickDelta(PlayerIndex playerIndex)
    {
        return GetGamePadStatus(playerIndex, _current).RightStickPosition - GetGamePadStatus(playerIndex, _previous).RightStickPosition;
    }

    /// <summary>
    ///     Delta de las posiciones del stick delta
    /// </summary>
    public float GetLeftTriggerDelta(PlayerIndex playerIndex)
    {
        return GetGamePadStatus(playerIndex, _current).LeftTrigger - GetGamePadStatus(playerIndex, _previous).LeftTrigger;
    }

    /// <summary>
    ///     Delta de las posiciones del stick delta
    /// </summary>
    public float GetRightTriggerDelta(PlayerIndex playerIndex)
    {
        return GetGamePadStatus(playerIndex, _current).RightTrigger - GetGamePadStatus(playerIndex, _previous).RightTrigger;
    }

    /// <summary>
    ///     Deadzone del stick izquierdo
    /// </summary>
    public float LeftStickDeadZone { get; set; } = 0.24f;

    /// <summary>
    ///     Deadzone del stick izquierdo
    /// </summary>
    public float RightStickDeadZone { get; set; } = 0.24f;

    /// <summary>
    ///     Deadzone del gatillo
    /// </summary>
    public float TriggerDeadZone { get; set; } = 0.1f;

    /// <summary>
    ///     Indica si se va a utilizar un deadzone radial
    /// </summary>
    public bool UseRadialDeadZone { get; set; } = true;
}