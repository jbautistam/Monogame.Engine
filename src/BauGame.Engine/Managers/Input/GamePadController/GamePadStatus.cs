using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bau.Libraries.BauGame.Engine.Managers.Input.GamePadController;

/// <summary>
///     Manager de teclas pulsadas
/// </summary>
public class GamePadStatus(PlayerIndex playerIndex)
{
    /// <summary>
    ///     Clona el objeto
    /// </summary>
	internal GamePadStatus Clone()
	{
        GamePadStatus state = new(PlayerIndex);

            // Añade las teclas pulsadas
            state.PressedButtons.AddRange(PressedButtons);
            // Devuelve el estado
            return state;
	}

    /// <summary>
    ///     Comprueba si se ha pulsado una tecla
    /// </summary>
    public bool IsPressed(Buttons button) => PressedButtons.Contains(button);

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(GameContext gameContext)
    {
        List<Buttons> buttons = [
                                  Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
                                  Buttons.LeftShoulder, Buttons.RightShoulder,
                                  Buttons.LeftTrigger, Buttons.RightTrigger,
                                  Buttons.LeftStick, Buttons.RightStick,
                                  Buttons.DPadUp, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight,
                                  Buttons.Start, Buttons.Back
                                 ];
        GamePadState state = GamePad.GetState(PlayerIndex);

            // Limpia los botones pulsados la última vez
            PressedButtons.Clear();
            // Añade los botones pulsados
            foreach (Buttons button in buttons)
                if (state.IsButtonDown(button))
                    PressedButtons.Add(button);
            // Guarda la posición de lo sticks
            LeftStickPosition = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Right.Y);
            RightStickPosition = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            // Guarda las posición de los triggers
            LeftTrigger = state.Triggers.Left;
            RightTrigger = state.Triggers.Right;
    }

    /// <summary>
    ///     Indica del GamePad
    /// </summary>
    public PlayerIndex PlayerIndex { get; } = playerIndex;

	/// <summary>
	///     Teclas pulsadas en la última comprobación de estado
	/// </summary>
	public List<Buttons> PressedButtons { get; } = [];

    /// <summary>
    ///     Posición del stick izquierdo
    /// </summary>
    public Vector2 LeftStickPosition { get; set; }

    /// <summary>
    ///     Posición del stick derecho
    /// </summary>
    public Vector2 RightStickPosition { get; set; }

    /// <summary>
    ///     Posición del trigger izquierdo
    /// </summary>
    public float LeftTrigger { get; set; }

    /// <summary>
    ///     Posición del trigger derecho
    /// </summary>
    public float RightTrigger { get; set; }
}
