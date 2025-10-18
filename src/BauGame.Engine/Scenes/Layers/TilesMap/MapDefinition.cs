namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.TilesMap;

/// <summary>
///     Definición de un mapa
/// </summary>
public class MapDefinition
{
    /// <summary>
    ///     Ancho del mapa
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    ///     Alto del mapa
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    ///     Ancho de los patrones del mapa
    ///     TODO: esto se debería averiguar por las texturas
    /// </summary>
    public int TileWidth { get; set; }

    /// <summary>
    ///     Alto de los patrones del mapa
    ///     TODO: esto se debería averiguar por las texturas
    /// </summary>
    public int TileHeight { get; set; }

    /// <summary>
    ///     Tiles del mapa
    /// </summary>
    public int [] Tiles { get; set; } = [];
}