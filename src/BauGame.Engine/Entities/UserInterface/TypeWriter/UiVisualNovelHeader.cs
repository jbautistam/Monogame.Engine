using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

/// <summary>
///		Cabecera para un <see cref="UiVisualNovelDialog"/>
/// </summary>
public class UiVisualNovelHeader : UiElement
{
	// Variables privadas
	private UiImage? _image;
	private UiLabel? _label;

	public UiVisualNovelHeader(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
	{
		Image = new UiImage(layer, new UiPosition(0, 0, 0.2f, 1f));
		Label = new UiLabel(layer, new UiPosition(0.8f, 0, 0.2f, 1f));
		Label.HorizontalAlignment = UiLabel.HorizontalAlignmentType.Left;
		Label.VerticalAlignment = UiLabel.VerticalAlignmentType.Center;
	}

	/// <summary>
	///		Calcula las coordenadas de pantalla
	/// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
		float widthTotal = 1.0f;
		float imageWidth = 0;

			// Ajusta la imagen
			if (Image is not null && Image.Visible)
			{
				imageWidth = 0.25f;
				Image.Position = new UiPosition(0.05f, 0, imageWidth, 1f);
				Image.Stretch = true;
				Image.Invalidate();
				widthTotal -= imageWidth;
			}
			// Ajusta el cuadro de texto
			if (Label is not null)
			{
				// Modifica la posición
				Label.Position = new UiPosition(0.07f + imageWidth, 0, widthTotal, 1);
				// Invalida el control
				Label.Invalidate();
			}
	}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	protected override void UpdateSelf(Managers.GameContext gameContext)
	{
		Image?.Update(gameContext);
		Label?.Update(gameContext);
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Managers.GameContext gameContext)
	{
		// Dibuja el estilo
		Layer.DrawStyle(camera, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
		// Dibuja la imagen
		if (Image is not null && Image.Visible)
			Image.Draw(camera, gameContext);
		// Dibuja la etiqueta
		Label?.Draw(camera, gameContext);
	}

    /// <summary>
    ///     Prepara los comandos de presentación
    /// </summary>
	public override void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, Managers.GameContext gameContext)
	{
		// Dibuja el estilo
		Layer.PrepareStyleRendercommands(builder, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
		// Dibuja la imagen
		if (Image is not null && Image.Visible)
			Image.PrepareRenderCommands(builder, gameContext);
		// Dibuja la etiqueta
		Label?.PrepareRenderCommands(builder, gameContext);
	}

	/// <summary>
	///		Control de texto
	/// </summary>
	public UiLabel? Label
	{ 
		get { return _label; }
		set
		{
			_label = value;
			if (_label is not null)
				_label.Parent = this;
			Invalidate();
		}
	}

	/// <summary>
	///		Imagen del avatar derecho
	/// </summary>
	public UiImage? Image
	{
		get { return _image; }
		set
		{
			_image = value;
			if (_image is not null)
				_image.Parent = this;
			Invalidate();
		}
	}
}
