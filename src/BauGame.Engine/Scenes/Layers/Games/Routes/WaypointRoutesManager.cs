namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.Routes;

/// <summary>
///		Diccionario de <see cref="WaypointRouteModel"/>
/// </summary>
public class WaypointRoutesManager(AbstractGameLayer layer)
{
	/// <summary>
	///		Añade una ruta
	/// </summary>
	public void AddRoute(string name, WaypointRouteModel waypointRoute)
	{
		Routes.Add(name, waypointRoute);
	}

	/// <summary>
	///		Obtiene el siguiente punto de una ruta
	/// </summary>
	public WaypointRouteModel.Waypoint? GetNextWayPoint(string name, WaypointRouteModel.Waypoint? actual, bool isLooping)
	{
		return Routes.Get(name)?.GetNextWaypoint(actual, isLooping);
	}

	/// <summary>
	///		Capa sobre la que se definen las rutas
	/// </summary>
	public AbstractGameLayer Layer { get; } = layer;

	/// <summary>
	///		Rutas definicadas
	/// </summary>
	private Base.DictionaryModel<WaypointRouteModel> Routes { get; } = new();
}
