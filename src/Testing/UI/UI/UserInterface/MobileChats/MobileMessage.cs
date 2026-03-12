namespace UI.UserInterface.MobileChats;

/// <summary>
///		Mensaje del móvil
/// </summary>
internal class MobileMessage(MobileSender sender, string text)
{
    /// <summary>
    ///     Datos del emisor del mensaje
    /// </summary>
    internal MobileSender Sender { get; } = sender;

    /// <summary>
    ///     Texto del mensaje
    /// </summary>
    internal string Text { get; } = text;

    /// <summary>
    ///     Líneas del texto
    /// </summary>
    internal List<string> Lines { get; } = [];

    /// <summary>
    ///     Tiempo que hay que esperar para mostrarlo
    /// </summary>
    internal float TimeToShow { get; set; } = 0.9f;

    /// <summary>
    ///     Indica si el mensaje está visible
    /// </summary>
    internal bool Visible { get; set; }

    /// <summary>
    ///     Altura del mensaje en pantalla
    /// </summary>
    internal float Height { get; set; }

    /// <summary>
    ///     Coordenada Y actual
    /// </summary>
    internal float CurrentY { get; set; } = 100_000;

    /// <summary>
    ///     Coordenada Y destino
    /// </summary>
    internal float TargetY { get; set; }
}