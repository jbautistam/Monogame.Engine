using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups.MobileChats;

/// <summary>
///     Datos del emisor de un mensaje
/// </summary>
public class MobileSender
{
    /// <summary>
    ///     Alineación de los mensajes
    /// </summary>
    public enum AlignmentType
    {
        /// <summary>A la izquierda</summary>
        Left,
        /// <summary>A la derecha</summary>
        Right
    }

    /// <summary>
    ///     Nombre del emisor
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Indica si es el jugador
    /// </summary>
    public bool IsPlayer { get; set; }

    /// <summary>
    ///     Indica si se debe mostrar el nombre en la pantalla
    /// </summary>
    public bool ShowName { get; set; }

    /// <summary>
    ///     Color del nombre
    /// </summary>
    public Color NameForecolor { get; set; }

    /// <summary>
    ///     Color del texto
    /// </summary>
    public Color Forecolor { get; set; }

    /// <summary>
    ///     Avatar
    /// </summary>
    public Common.Sprites.SpriteDefinition? Avatar { get; set; }

    /// <summary>
    ///     Textura del fondo
    /// </summary>
	public Common.Sprites.SpriteDefinition? SpriteBackground { get; set; }

    /// <summary>
    ///     Color de fondo
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    ///     Alineación de los mensajes
    /// </summary>
    public AlignmentType Alignment { get; set; } = AlignmentType.Left;
}