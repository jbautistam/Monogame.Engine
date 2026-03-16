using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Galleries;

/// <summary>
///		Elemento de un <see cref="UiGallery"/>
/// </summary>
public class UiGalleryItem : UiElement
{
	public UiGalleryItem(UiGallery gallery, UiElement item, int row, int column) : base(gallery.Layer, new UiPosition(0, 0, 1, 1))
	{
		Parent = gallery;
		Row = row;
		Column = column;
		Item = item;
		Item.Parent = this;
	}

    /// <summary>
    ///     Calcula la posición en pantalla
    /// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
        Item.ComputeScreenBounds(Position.ContentBounds);
	}

	/// <summary>
	///		Actualiza el elemento
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		Item.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja el elemento
    /// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
        if (Item.Visible)
		    Item.Draw(camera, gameContext);
	}

	/// <summary>
	///		Fila
	/// </summary>
	public int Row { get; }

	/// <summary>
	///		Columna
	/// </summary>
	public int Column { get; }

	/// <summary>
	///		Elemento
	/// </summary>
	public UiElement Item { get; }
}