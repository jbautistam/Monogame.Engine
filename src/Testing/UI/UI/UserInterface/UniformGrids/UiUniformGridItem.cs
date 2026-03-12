using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace UI.UserInterface.UniformGrids;

/// <summary>
///		Elemento de un <see cref="UiUniformGrid"/>
/// </summary>
public class UiUniformGridItem : UiElement
{
	public UiUniformGridItem(UiUniformGrid grid, int row, int column, UiElement element) : base(grid.Layer, new UiPosition(0, 0, 0, 0))
	{
		Parent = grid;
		Row = row;
		Column = column;
		Element = element;
		Element.Parent = this;
	}

	protected override void ComputeScreenComponentBounds()
	{
	}

	public override void Update(GameContext gameContext)
	{
		Element.Update(gameContext);
	}

	public override void Draw(Camera2D camera, GameContext gameContext)
	{
		Element.Draw(camera, gameContext);
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
	public UiElement Element { get; }
}