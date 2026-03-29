using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///		Lista de elementos de la interface de usuario
/// </summary>
public class UiElementList : Common.Collections.SecureList<UiElement>
{
	/// <summary>
	///		Invalida los elementos de la lista (cuando se cambian los límites de la pantalla por ejemplo)
	/// </summary>
	public void Invalidate()
	{
		foreach (UiElement element in Enumerate())
			element.Invalidate();
	}

	/// <summary>
	///		Actualiza la lista de elementos
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		foreach (UiElement item in Enumerate())
			if (item.Visible)
				item.Update(gameContext);
	}

	/// <summary>
	///		Trata un elemento añadido a la lista
	/// </summary>
	protected override void Added(UiElement item)
	{
	}

	/// <summary>
	///		Trata un elemento eliminado de la lista
	/// </summary>
	protected override void Removed(UiElement item)
	{
	}
}
