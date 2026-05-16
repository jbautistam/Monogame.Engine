using Bau.Libraries.LibMarkupLanguage;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Scenes.Layers;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Repositorio para carga de interface de usuario
/// </summary>
public class UserInterfaceRepository : AbstractScreenRepository
{
	/// <summary>
	///		Carga un elemento de interface de usuario particular para el proyecto: en el caso básico, devuelve siempre un componente nulo
	/// </summary>
	protected override UiElement? LoadUiItem(AbstractUserInterfaceLayer layer, MLNode rootML) => null;
}