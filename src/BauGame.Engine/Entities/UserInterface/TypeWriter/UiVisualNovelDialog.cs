using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

/// <summary>
///		Cuadro de diálogo para novela visual
/// </summary>
public class UiVisualNovelDialog(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
	// Variables privadas
	private UiVisualNovelAvatar? _leftAvatar, _rightAvatar;
	private UiImage? _cursor;
	private UiVisualNovelHeader? _header;
	private UiTypeWriterLabel? _typeWriter;
	private float _elapsedCursor = 0;

	/// <summary>
	///		Calcula las coordenadas de pantalla
	/// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
		float leftAvatarWidth = 0f, rightAvatarWidth = 0f;
		float headerHeight = 0f;

			// Ajusta el avatar izquierdo
			if (LeftAvatar is not null && LeftAvatar.Visible)
			{
				leftAvatarWidth = 0.15f;
				LeftAvatar.Position = new UiPosition(0, 0, leftAvatarWidth, 1f);
				LeftAvatar.Invalidate();
			}
			// Ajusta el avatar derecho
			if (RightAvatar is not null && RightAvatar.Visible)
			{
				rightAvatarWidth = 0.15f;
				RightAvatar.Position = new UiPosition(1 - rightAvatarWidth, 0, rightAvatarWidth, 1f);
				RightAvatar.Invalidate();
			}
			// Ajusta la cabecera
			if (Header is not null && Header.Visible)
			{ 
				// Modifica la posición
				headerHeight = 0.25f;
				Header.Position = new UiPosition(0.1f + leftAvatarWidth, 0, 0.25f, headerHeight);
				// Invalida el control
				Header.Invalidate();
			}
			// Ajusta el cuadro de texto
			if (TypeWriter is not null)
			{
				float top = 0;

					// Cambia el top del cuadro de texto si es necesario
					if (headerHeight > 0)
						top = headerHeight - 0.01f;
					// Modifica la posición
					TypeWriter.Position = new UiPosition(leftAvatarWidth, top, 1 - leftAvatarWidth - rightAvatarWidth, 1);
					// Invalida el control
					TypeWriter.Invalidate();
					// Ajusta el cursor
					if (Cursor is not null)
					{
						Cursor.Position = new UiPosition(0.93f - rightAvatarWidth, 0.9f, 0.2f, 0.2f);
						Cursor.Invalidate();
					}
			}
	}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		// Actualiza el cursor
		if (TypeWriter is not null && Cursor is not null)
		{
			// Muestra / oculta el cursor
			Cursor.Visible = !TypeWriter.IsWriting;
			// Cambia la opacidad del cursor
			if (Cursor.Visible)
			{
				float flashDuration = Math.Max(CursorFlashDuration, 0.5f);

					// Modifica el tiempo
					_elapsedCursor += gameContext.DeltaTime;
					if (_elapsedCursor > flashDuration)
						_elapsedCursor = 0;
					// Cambia la opacidad
					Cursor.Opacity = Tools.MathTools.Easing.EasingFunctionsHelper.Interpolate(0, 1, _elapsedCursor / flashDuration, 
																							  Tools.MathTools.Easing.EasingFunctionsHelper.EasingType.Linear);
			}
		}
		// Actualiza los controles
		LeftAvatar?.Update(gameContext);
		RightAvatar?.Update(gameContext);
		Header?.Update(gameContext);
		TypeWriter?.Update(gameContext);
		Cursor?.Update(gameContext);
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
		// Dibuja el avatar izquierdo
		if (LeftAvatar is not null && LeftAvatar.Visible)
			LeftAvatar.Draw(renderingManager, gameContext);
		// Dibuja los textos y el cursor
		TypeWriter?.Draw(renderingManager, gameContext);
		Header?.Draw(renderingManager, gameContext);
		if (Cursor is not null && Cursor.Visible)
			Cursor.Draw(renderingManager, gameContext);
		// Dibuja el avatar derecho
		if (RightAvatar is not null && RightAvatar.Visible)
			RightAvatar.Draw(renderingManager, gameContext);
	}

	/// <summary>
	///		Imagen del avatar izquierdo
	/// </summary>
	public UiVisualNovelAvatar? LeftAvatar
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
	public UiVisualNovelAvatar? RightAvatar
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
	///		Cabecera
	/// </summary>
	public UiVisualNovelHeader? Header
	{
		get { return _header; }
		set
		{
			_header = value;
			if (_header is not null)
				_header.Parent = this;
			Invalidate();
		}
	}

	/// <summary>
	///		Cursor
	/// </summary>
	public UiImage? Cursor
	{
		get { return _cursor; }
		set
		{
			_cursor = value;
			if (_cursor is not null)
				_cursor.Parent = this;
			Invalidate();
		}
	}

	/// <summary>
	///		Duración del flash del cursor
	/// </summary>
	public float CursorFlashDuration { get; set; } = 2;
}
