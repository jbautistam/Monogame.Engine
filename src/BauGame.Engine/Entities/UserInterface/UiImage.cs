using Bau.Libraries.BauGame.Engine.Entities.Common;
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
    public override void UpdateSelf(Managers.GameContext gameContext) 
    {
        Sprite?.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
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
                // Dibuja la imagen
                Sprite.Draw(camera, target, new Vector2(0, 0), Rotation, style.Color * style.Opacity);
                // Dibuja un rectángulo para depuración
		        if (GameEngine.Instance.EngineSettings.DebugMode)
                    camera.SpriteBatchController.DrawRectangleOutline(Position.ContentBounds, GameEngine.Instance.EngineSettings.DebugImageColor, 2);
        }
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
    ///     Indica si se debe ajustar
    /// </summary>
    public bool Stretch { get; set; }

    /// <summary>
    ///     Indica si se debe preservar el ratio de la imagen
    /// </summary>
    public bool PreserveAspectRatio { get; set; } = true;
}
