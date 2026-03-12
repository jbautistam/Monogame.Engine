namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Grids;

/// <summary>
///     Definición de una fila o columna en el grid
/// </summary>
public class GridSizeLayoutModel
{
    /// <summary>
    ///     Tamaño relativo
    /// </summary>
    public float RelativeSize { get; set; } = 0.0f;
    
    /// <summary>
    ///     Indica si está visible. Si no es visible, su tamaño se reparte entre el resto
    /// </summary>
    public bool IsVisible { get; set; } = true;
    
    /// <summary>
    ///     Tamaño mínimo absoluto en píxeles (0 indica que no se considera tamaño mínimo)
    /// </summary>
    public float MinSize { get; set; } = 0f;
    
    /// <summary>
    ///     Tamaño máximo absoluto en píxeles (0 indica que no se considera tamaño mínimo)
    /// </summary>
    public float MaxSize { get; set; } = 0f;
    
    /// <summary>
    ///     Tamaño calculado
    /// </summary>
    internal float CalculatedSize { get; set; }

    /// <summary>
    ///     Posición calculada
    /// </summary>
    internal float CalculatedPosition { get; set; }
}