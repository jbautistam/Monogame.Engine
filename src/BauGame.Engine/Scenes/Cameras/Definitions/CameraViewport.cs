using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Cameras.Definitions;

/// <summary>
///     Configuración de un viewport relativo donde se va a colocar la cámara
/// </summary>
public class CameraViewport(float x, float y, float width, float height)
{
    /// <summary>
    ///     Transforma el rectángulo relativo a coordenadas de pantalla
    /// </summary>
    public Viewport ToViewport(Viewport viewport)
    {
        return new Viewport((int) (X * viewport.Width), (int) (Y * viewport.Height), 
                            (int) (Width * viewport.Width), (int) (Height * viewport.Height));
    }

    /// <summary>
    ///     Coordenada X
    /// </summary>
    public float X { get; set; } = x;

    /// <summary>
    ///     Coordenada Y
    /// </summary>
    public float Y { get; set; } = y;

    /// <summary>
    ///     Ancho
    /// </summary>
    public float Width { get; set; } = width;

    /// <summary>
    ///     Altura
    /// </summary>
    public float Height { get; set; } = height;
        
    /// <summary>
    ///     Indica si se debe mantener el ratio de aspecto
    /// </summary>
    public bool MaintainAspectRatio { get; set; } = true;
}