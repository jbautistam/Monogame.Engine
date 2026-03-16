using Bau.Libraries.BauGame.Engine.Entities.Common;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Control para mostrar una imagen
/// </summary>
public class UiImage(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() {}

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext) 
    {
        Sprite?.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        // Dibuja los componentes del estilo
        Layer.DrawStyle(camera, Style, Styles.UiStyle.StyleType.Normal, Position.ContentBounds, gameContext);
        // Dibuja la imagen
        if (Sprite is not null)
        {
            Rectangle target = Position.ContentBounds;
            Styles.UiStyle style = Layer.Styles.GetDefault(Style);
            Size size = Sprite.GetSize();

                // Ajusta el tamaño si se requiere preservar aspecto
                if (PreserveAspectRatio && size.Width > 0 && size.Height > 0)
                {
                    float textureRatio = (float) size.Width / size.Height;
                    float boundsRatio = (float) target.Width / target.Height;
                    int newWidth = target.Width, newHeight = target.Height;

                        // Dependiendo de si la textura es más ancha o más alta
                        if (textureRatio > boundsRatio) // Textura más ancha
                            newHeight = (int) (target.Width / textureRatio);
                        else // Textura más alta
                            newWidth = (int) (target.Height * textureRatio);
                        // Calcula el rectángulo destino
                        target = new Rectangle(target.X, target.Y, newWidth, newHeight);
                }
                // Alinea la imagen
                target = AlignImage(target);
                // Dibuja la imagen
                camera.RenderingManager.SpriteRenderer.Draw(Sprite, target, Origin, Rotation, style.Color * style.Opacity * Opacity);
                // Dibuja un rectángulo para depuración
		        if (GameEngine.Instance.EngineSettings.DebugMode)
                    camera.RenderingManager.FiguresRenderer.DrawRectangleOutline(Position.ContentBounds, GameEngine.Instance.EngineSettings.DebugImageColor, 2);
        }
    }

    /// <summary>
    ///     Alinea la imagen
    /// </summary>
    private Rectangle AlignImage(Rectangle bounds)
    {
        int x = bounds.X;
        int y = bounds.Y;

            // Calcula la X teniendo en cuenta la alineación
            switch (HorizontalAlignment)
            {
                case UiLabel.HorizontalAlignmentType.Center:
                case UiLabel.HorizontalAlignmentType.Stretch:
                        x = Position.ContentBounds.X + (int) (0.5f * (Position.ContentBounds.Width - bounds.Width));
                    break;
                case UiLabel.HorizontalAlignmentType.Right:
                        x = Position.ContentBounds.X + Position.ContentBounds.Width - bounds.Width;
                    break;
            }
            // Calcula la Y teniendo en cuenta la alineación
            switch (VerticalAlignment)
            {
                case UiLabel.VerticalAlignmentType.Center:
                case UiLabel.VerticalAlignmentType.Stretch:
                        y = Position.ContentBounds.Y + (int) (0.5f * (Position.ContentBounds.Height - bounds.Height));
                    break;
                case UiLabel.VerticalAlignmentType.Bottom:
                        y = Position.ContentBounds.Y + Position.ContentBounds.Height - bounds.Height;
                    break;
            }

            // Devuelve el rectángulo modificado
            return new Rectangle(x, y, bounds.Width, bounds.Height);
    }

    /// <summary>
    ///     Definición de la textura
    /// </summary>
    public SpriteDefinition? Sprite { get; set; }

    /// <summary>
    ///     Rotación de la imagen
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Origen
    /// </summary>
    public Vector2 Origin { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Opacidad
    /// </summary>
    public float Opacity { get; set; } = 1;

    /// <summary>
    ///     Indica si se debe ajustar
    /// </summary>
    public bool Stretch { get; set; }

    /// <summary>
    ///     Indica si se debe preservar el ratio de la imagen
    /// </summary>
    public bool PreserveAspectRatio { get; set; } = true;

    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public UiLabel.HorizontalAlignmentType HorizontalAlignment { get; set; } = UiLabel.HorizontalAlignmentType.Center;

    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public UiLabel.VerticalAlignmentType VerticalAlignment { get; set; } = UiLabel.VerticalAlignmentType.Center;
}
