using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///		Datos de la transformación
/// </summary>
public class TransformRenderModel
{
    /// <summary>
    ///     Transformación
    /// </summary>
    public Entities.Common.TransformModel Coordinates { get; } = new();

    /// <summary>
    ///     Rectángulo destino donde se dibuja
    /// </summary>
    public Rectangle Destination { get; set; } = new();
}