using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

/// <summary>
///		Avatar para novela visual
/// </summary>
public class UiVisualNovelAvatar(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
	// Variables privadas
	private UiImage? _avatar;

	/// <summary>
	///		Calcula las coordenadas de pantalla
	/// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
		Avatar?.ComputeScreenBounds(Position.ContentBounds);
	}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		Avatar?.Update(gameContext);
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
		Layer.DrawStyle(camera, Style, Styles.UiStyle.StyleType.Normal, Position.ContentBounds, gameContext);
		Avatar?.Draw(camera, gameContext);
	}

	/// <summary>
	///		Imagen del avatar
	/// </summary>
	public UiImage? Avatar
	{
		get { return _avatar; }
		set
		{
			_avatar = value;
			if (_avatar is not null)
				_avatar.Parent = this;
			Invalidate();
		}
	}
}
