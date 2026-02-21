using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

/// <summary>
///		Texto para novela visual
/// </summary>
public class UiVisualNovelText : UiElement
{
	// Variables privadas
	private UiImage? _leftAvatar, _rightAvatar;
	private UiTypeWriterLabel? _typeWriter;
	private bool _leftAvatarVisible, _rightAvatarVisible;

	public UiVisualNovelText(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
	{
		TypeWriter = new UiTypeWriterLabel(layer, new UiPosition(0.2f, 0, 0.6f, 1f));
		LeftAvatar = new UiImage(layer, new UiPosition(0, 0, 0.2f, 1f));
		LeftAvatarVisible = true;
		RightAvatar = new UiImage(layer, new UiPosition(0.8f, 0, 0.2f, 1f));
		RightAvatarVisible = true;
	}

	/// <summary>
	///		Calcula las coordenadas de pantalla
	/// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
		float widthTotal = 1.0f;

			// Ajusta el avatar izquierdo
			if (LeftAvatarVisible && LeftAvatar is not null)
			{
				LeftAvatar.Position = new UiPosition(0, 0, 0.2f, 1f);
				LeftAvatar.Stretch = true;
				LeftAvatar.Invalidate();
				widthTotal -= 0.21f;
			}
			// Ajusta el avatar derecho
			if (RightAvatarVisible && RightAvatar is not null)
			{
				RightAvatar.Position = new UiPosition(0.8f, 0, 0.2f, 1f);
				RightAvatar.Stretch = true;
				RightAvatar.Invalidate();
				widthTotal -= 0.21f;
			}
			// Ajusta el cuadro de texto
			if (TypeWriter is not null)
			{
				float left = 0;

					// Calcula la posición X del texto
					if (LeftAvatarVisible)
						left = 0.2f;
					// Modifica la posición
					TypeWriter.Position = new UiPosition(left, 0, widthTotal, 1);
					// Invalida el control
					TypeWriter.Invalidate();
			}
	}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	public override void UpdateSelf(GameContext gameContext)
	{
		LeftAvatar?.Update(gameContext);
		RightAvatar?.Update(gameContext);
		TypeWriter?.Update(gameContext);
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
		// Dibuja el avatar izquierdo, el cuadro de texto y el avatar derecho
		if (LeftAvatarVisible)
			LeftAvatar?.Draw(camera, gameContext);
		TypeWriter?.Draw(camera, gameContext);
		if (RightAvatarVisible)
			RightAvatar?.Draw(camera, gameContext);
	}

	/// <summary>
	///		Imagen del avatar izquierdo
	/// </summary>
	public UiImage? LeftAvatar
	{
		get { return _leftAvatar; }
		set
		{
			_leftAvatar = value;
			if (_leftAvatar is not null)
				_leftAvatar.Parent = this;
			Invalidate();
		}
	}

	/// <summary>
	///		Indica si se muestra el avatar izquierdo
	/// </summary>
	public bool LeftAvatarVisible
	{
		get { return _leftAvatarVisible && _leftAvatar is not null; }
		set
		{
			_leftAvatarVisible = value;
			Invalidate();
		}
	}

	/// <summary>
	///		Control de texto
	/// </summary>
	public UiTypeWriterLabel? TypeWriter 
	{ 
		get { return _typeWriter; }
		set
		{
			_typeWriter = value;
			if (_typeWriter is not null)
				_typeWriter.Parent = this;
			Invalidate();
		}
	}

	/// <summary>
	///		Imagen del avatar derecho
	/// </summary>
	public UiImage? RightAvatar
	{
		get { return _rightAvatar; }
		set
		{
			_rightAvatar = value;
			if (_rightAvatar is not null)
				_rightAvatar.Parent = this;
			Invalidate();
		}
	}

	/// <summary>
	///		Indica si se muestra el avatar derecho
	/// </summary>
	public bool RightAvatarVisible
	{
		get { return _rightAvatarVisible && _rightAvatar is not null; }
		set
		{
			_rightAvatarVisible = value;
			Invalidate();
		}
	}
}
