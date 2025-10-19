using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///		Componente con un borde
/// </summary>
public class UiBorder(AbstractUserInterfaceLayer layer, UiPosition positions) : UiElement(layer, positions)
{
	/// <summary>
	///		Calcula los límites del control
	/// </summary>
	protected override void ComputeScreenComponentBounds() {}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Color del borde
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Grosor del borde
	/// </summary>
	public float Thickness { get; set; } = 1f;
}