namespace Bau.Monogame.Engine.Domain.Core.Managers.Resources.Animations;

/// <summary>
///		Clase con los datos de una animación
/// </summary>
public class Animation(AnimationManager manager, string name, string texture)
{
	// Datos de un frame de animación
	public record AnimationFrame(string Region, float time);

	/// <summary>
	///		Obtiene un frame
	/// </summary>
	public AnimationFrame? GetFrame(int frame)
	{
		if (Frames.Count == 0 || frame < 0 || frame >= Frames.Count)
			return null;
		else
			return Frames[frame];
	}

	/// <summary>
	///		Manager de la animación
	/// </summary>
	public AnimationManager Manager { get; } = manager;

	/// <summary>
	///		Nombre de la animación
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Nombre de la textura que se utiliza para la animación
	/// </summary>
	public string Texture { get; } = texture;

	/// <summary>
	///		Frames de la animación
	/// </summary>
	public List<AnimationFrame> Frames { get; } = [];
}