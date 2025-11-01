namespace Bau.Libraries.BauGame.Engine.Actors.Components.Renderers;

/// <summary>
///		Propiedades del componente para el mezclador de animaciones
/// </summary>
public class AnimatorBlenderProperties(string group)
{
	/// <summary>
	///		Añade un valor lógico a las propiedades
	/// </summary>
	public void Add(string name, bool value)
	{
		Properties.Add(name, value ? 1 : 0);
	}

	/// <summary>
	///		Añade un valor flotante a las propiedades
	/// </summary>
	public void Add(string name, float value)
	{
		Properties.Add(name, value);
	}

	/// <summary>
	///		Grupo al que se aplican las reglas
	/// </summary>
	public string Group { get; } = group;

	/// <summary>
	///		Propiedades del mezclador de animaciones del componente
	/// </summary>
	public Base.DictionaryModel<float> Properties { get; } = new();
}
