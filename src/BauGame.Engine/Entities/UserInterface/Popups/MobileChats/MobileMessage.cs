using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups.MobileChats;

/// <summary>
///		Mensaje del móvil
/// </summary>
internal class MobileMessage(MobileSender sender, string text, float timeToShow)
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
    // Variables privadas
    private float _height;

    /// <summary>
    ///     Calcula la altura en pantalla
    /// </summary>
    internal float GetHeight(SpriteFont? spriteFont, int width, float lineSpacing)
    {
        if (!IsDirty || spriteFont is null)
            return _height;
        else
        {
            // Calcula las líneas
            Lines.Clear();
            Lines.AddRange(new StringFontHelper().WrapText(spriteFont, Text, 1, width));
            // Calcula la altura
            _height = ComputeHeight(spriteFont, Lines.Count + (Sender.ShowName ? 1 : 0), lineSpacing);
            // Indica que se ha calculado anteriormente el alto
            IsDirty = false;
            // Devuelve la altura calculada
            return _height;
        }

        // Calcula la altura de todas las líneas
        float ComputeHeight(SpriteFont spriteFont, int lines, float lineSpacing)
        {
            Vector2 size = spriteFont.MeasureString("A");

                // Devuelve la altura de las líneas
                return size.Y * lines + lines * lineSpacing;
        }
    }

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
    ///     Posición del mensaje en pantalla
    /// </summary>
    internal float Y { get; set; }

    /// <summary>
    ///     Indica si se han modificado los datos
    /// </summary>
    internal bool IsDirty { get; set; } = true;
}