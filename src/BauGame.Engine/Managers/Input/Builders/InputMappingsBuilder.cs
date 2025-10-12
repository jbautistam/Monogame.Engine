using Bau.Libraries.BauGame.Engine.Managers.Input.MouseController;
using Microsoft.Xna.Framework.Input;

namespace Bau.Libraries.BauGame.Engine.Managers.Input.Builders;

/// <summary>
///		Clase para generación de <see cref="InputMappings"/>
/// </summary>
public class InputMappingsBuilder
{
	/// <summary>
	///		Añade una acción
	/// </summary>
	public InputMappingsBuilder WithAction(string action)
	{
		// Añade el mapeo
		Mappings.Add(new InputMappings(action));
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una tecla al mapeo
	/// </summary>
	public InputMappingsBuilder WithKeyboard(InputMappings.Status status, Keys key)
	{
		// Añade la tecla
		Actual.KeyboardKeys.Add((status, key));
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un botón del ratón al mapeo
	/// </summary>
	public InputMappingsBuilder WithMouse(InputMappings.Status status, MouseStatus.MouseButton button)
	{
		// Añade la tecla
		Actual.MouseButtons.Add((status, button));
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un botón del gamepad al mapeo
	/// </summary>
	public InputMappingsBuilder WithGamepad(InputMappings.Status status, Buttons button)
	{
		// Añade la tecla
		Actual.GamepadButtons.Add((status, button));
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Genera los mapeos
	/// </summary>
	public List<InputMappings> Build() => Mappings;

	/// <summary>
	///		Mapeos definidos
	/// </summary>
	private List<InputMappings> Mappings { get; } = [];

	/// <summary>
	///		Mapeo actual
	/// </summary>
	private InputMappings Actual => Mappings[Mappings.Count - 1];
}
