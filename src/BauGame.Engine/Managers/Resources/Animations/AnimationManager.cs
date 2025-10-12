namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;

/// <summary>
///		Manager de animaciones
/// </summary>
public class AnimationManager(ResourcesManager resourcesManager)
{
	/// <summary>
	///		Añade una animación
	/// </summary>
	public Animation Add(string name, string texture, List<Animation.AnimationFrame> frames)
	{
		Animation animation = new(this, name, texture);

			// Añade los frames
			animation.Frames.AddRange(frames);
			// Añade la animación a la lista
			Animations.Add(animation.Name, animation);
			// Devuelve la animación añadida
			return animation;
	}

	/// <summary>
	///		Manager de recursos
	/// </summary>
	public ResourcesManager ResourcesManager { get; } = resourcesManager;

	/// <summary>
	///		Animaciones
	/// </summary>
	public Base.DictionaryModel<Animation> Animations { get; } = new();
}