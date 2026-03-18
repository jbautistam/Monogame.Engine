using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Entities.UserInterface.Popups.MobileChats;

/// <summary>
///		Mensaje del móvil
/// </summary>
internal class MobileMessage(MobileSender sender, float timeToShow)
{
    /// <summary>
    ///     Estado del mensaje
    /// </summary>
    internal enum StatusType
    {
        /// <summary>Se está mostrando en la pantalla</summary>
        Showing,
        /// <summary>Está escribiendo</summary>
        Writing,
        /// <summary>Está esperando a aparecer</summary>
        Waiting
    }

    /// <summary>
    ///     Datos del emisor del mensaje
    /// </summary>
    internal MobileSender Sender { get; } = sender;

    /// <summary>
    ///     Parámetros de dibujo
    /// </summary>
	internal SpriteTextParameters Parameters { get; } = new();

    /// <summary>
    ///     Tiempo que hay que esperar para mostrarlo
    /// </summary>
    internal float TimeToShow { get; } = timeToShow;

    /// <summary>
    ///     Momento de inicio en que se muestra el mensaje
    /// </summary>
    internal float Start { get; set; } = 0.9f;

    /// <summary>
    ///     Estado
    /// </summary>
    internal StatusType Status
    {
        get
        {
            if (Start <= 0)
                return StatusType.Showing;
            else if (Start <= 1)
                return StatusType.Writing;
            else
                return StatusType.Waiting;
        }
    }

    /// <summary>
    ///     Indica si se han modificado los datos
    /// </summary>
    internal bool IsDirty { get; set; } = true;
}