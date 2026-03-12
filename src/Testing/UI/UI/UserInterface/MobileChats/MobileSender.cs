using Microsoft.Xna.Framework;

namespace UI.UserInterface.MobileChats;

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
    ///     Color del texto
    /// </summary>
    public Color Forecolor { get; set; }

    /// <summary>
    ///     Color de fondo
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    ///     Nombre de la textura
    /// </summary>
    public string? AssetAvatar { get; set; }

    /// <summary>
    ///     Región de la textura
    /// </summary>
	public string? AssetAvatarRegion { get; set; }

    /// <summary>
    ///     Alineación de los mensajes
    /// </summary>
    public AlignmentType Alignment { get; set; } = AlignmentType.Left;
}