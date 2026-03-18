namespace Bau.BauEngine.Entities.UserInterface.Interfaces;

/// <summary>
///		Interface para los componentes que pueden contener elementos
/// </summary>
public interface IComponentPanel
{
	/// <summary>
	///		Obtiene un elemento por su Id
	/// </summary>
	TypeData? GetItem<TypeData>(string id) where TypeData : UiElement;
}