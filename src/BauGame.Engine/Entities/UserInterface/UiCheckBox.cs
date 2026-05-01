using Bau.BauEngine.Entities.Sprites;
using Bau.BauEngine.Managers.Resources;
using Bau.BauEngine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///     Control botón para el interface
/// </summary>
public class UiCheckbox : UiElementClickable
{
    // Variables privadas
    private int _widthImage, _heightImage;
    private bool _isChecked;

    public UiCheckbox(AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
    {
        Position.Padding = new UiMargin(10);
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
    }

    /// <summary>
    ///     Actualiza el contenido del componente
    /// </summary>
    protected override void UpdateComponent(Managers.GameContext gameContext)
    {
        // Actualiza los controles
        CheckedImage?.Update(gameContext);
        UncheckedImage?.Update(gameContext);
        CheckedLabel?.Update(gameContext);
        UncheckedLabel?.Update(gameContext);
        // Obtiene los anchos / altos de las imágenes
        UpdateImageSize();
    }

    /// <summary>
    ///     Actualiza el tamaño de la imagen
    /// </summary>
    private void UpdateImageSize()
    {
        (int widthCheckedImage, int heightCheckedImage) = GetSize(CheckedImage);
        (int widthUncheckedImage, int heightUncheckedImage) = GetSize(UncheckedImage);

            // Obtiene el ancho y alto de la imagen
            _widthImage = Math.Max(widthCheckedImage, widthUncheckedImage);
            _heightImage = Math.Max(heightCheckedImage, heightUncheckedImage);

        // Obtiene el tamaño de una imagen
        (int widht, int height) GetSize(UiImage? image)
        {
            // Si realmente hay alguna imagen definida
            if (image is not null && image.Sprite is not null)
            {
                Common.Size size = image.Sprite.GetSize();

                    return ((int) size.Width, (int) size.Height);
            }
            // Si ha llegado hasta aquí es porque no ha encontrado ningún dato
            return (0, 0);
        }
    }

    /// <summary>
    ///     Actualiza los valores cuando se presiona sobre el componente
    /// </summary>
    protected override void UpdatePressed()
    {
        IsChecked = !IsChecked;
        IsSelected = IsChecked;
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    protected override void DrawComponent(Scenes.Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext)
    {
        Styles.UiStyle? style = GetStyle();
        UiLabel? label = null;

            // Obtiene el texto
            if (IsChecked)
                label = GetLabel(CheckedLabel, UncheckedLabel);
            else
                label = GetLabel(UncheckedLabel, CheckedLabel);
            // Dibuja la imagen
            if (IsChecked)
                DrawImage(CheckedImage ?? UncheckedImage);
            else
                DrawImage(UncheckedImage ?? CheckedImage);
            // Dibuja la etiqueta
            if (label is not null && !string.IsNullOrWhiteSpace(label.Text) && style?.Font is not null)
            {
                Vector2 textSize = style.Font.MeasureString(label.Text);
                Vector2 textPosition = new(Position.ContentBounds.X + _widthImage + 10, Position.ContentBounds.Y + GetTextTop(textSize.Y));

                    // Dibuja el texto
                    renderingManager.SpriteTextRenderer.DrawString(style.Font, label.Text, textPosition, 
                                                                   (style?.StyleText?.Color ?? Color.White) * (style?.StyleText?.Opacity ?? 1));
            }

        // Obtiene la etiqueta
        UiLabel? GetLabel(UiLabel? first, UiLabel? second)
        {
            if (first is not null)
                return first;
            else
                return second;
        }

        // Obtiene la posición superior del texto
        float GetTextTop(float textHeight)
        {
            if (_heightImage > 0)
                return (_heightImage - Math.Min(_heightImage, textHeight)) / 2;
            else
                return (Position.ContentBounds.Height - Math.Min(Position.ContentBounds.Height, textHeight)) / 2;
        }

        // Dibuja la imagen
        void DrawImage(UiImage? image)
        {
            if (image is not null)
            {
                image.Position.ContentBounds = new(Position.ContentBounds.Left, Position.ContentBounds.Top, _widthImage, _heightImage);

                    image.Draw(renderingManager, gameContext);
            }
        }
    }

    /// <summary>
    ///     Imagen cuando el componente está chequeado
    /// </summary>
    public UiImage? CheckedImage { get; set; }

    /// <summary>
    ///     Imagen cuando el componente no está chequeado
    /// </summary>
    public UiImage? UncheckedImage { get; set; }

    /// <summary>
    ///     Etiqueta cuando está chequeado
    /// </summary>
    public UiLabel? CheckedLabel { get; set; }

    /// <summary>
    ///     Etiqueta cuando no está chequeado
    /// </summary>
    public UiLabel? UncheckedLabel { get; set; }

    /// <summary>
    ///     Indica si se ha chequeado el componente
    /// </summary>
    public bool IsChecked
    {
        get { return _isChecked; }
        set
        {
            if (value != _isChecked)
            {
                _isChecked = value;
                Layer.RaiseValueChanged(new EventArguments.ValueChangedEventArgs(this, !value, _isChecked));
            }
        }
    }
}