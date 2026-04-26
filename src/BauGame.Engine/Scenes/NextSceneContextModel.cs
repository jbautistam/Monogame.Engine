namespace Bau.BauEngine.Scenes;

/// <summary>
///     Datos del contexto de una escena
/// </summary>
public class NextSceneContextModel(string scene)
{
    /// <summary>
    ///     Nombre de la escena
    /// </summary>
    public string Scene { get; } = scene;

    /// <summary>
    ///     Parámetros de la siguiente escena
    /// </summary>
    public Dictionary<string, object> Parameters { get; } = new(StringComparer.CurrentCultureIgnoreCase);
}
