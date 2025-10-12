using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Core.Scenes.Layers.UserInterface;

/// <summary>
///     Control para mostrar una imagen
/// </summary>
public class UiImage(UserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds() {}

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void Update(GameTime gameTime) {}

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
        if (Texture is not null)
        {
            Rectangle destinationRect = Position.ScreenPaddedBounds;

            // Ajustar tamaño si se requiere preservar aspecto
            if (PreserveAspectRatio && Texture.Width > 0 && Texture.Height > 0)
            {
                float textureRatio = (float) Texture.Width / Texture.Height;
                float boundsRatio = (float) Position.ScreenPaddedBounds.Width / Position.ScreenPaddedBounds.Height;

                if (textureRatio > boundsRatio)
                {
                    // Textura más ancha
                    int newHeight = (int) (Position.ScreenPaddedBounds.Width / textureRatio);
                    destinationRect = new Rectangle(
                        Position.ScreenPaddedBounds.X,
                        Position.ScreenPaddedBounds.Y + (Position.ScreenPaddedBounds.Height - newHeight) / 2,
                        Position.ScreenPaddedBounds.Width,
                        newHeight
                    );
                }
                else
                {
                    // Textura más alta
                    int newWidth = (int)(Position.ScreenPaddedBounds.Height * textureRatio);
                    destinationRect = new Rectangle(
                        Position.ScreenPaddedBounds.X + (Position.ScreenPaddedBounds.Width - newWidth) / 2,
                        Position.ScreenPaddedBounds.Y,
                        newWidth,
                        Position.ScreenPaddedBounds.Height
                    );
                }
            }

            camera.SpriteBatchController.Draw(Texture, destinationRect, SourceRectangle, Origin, Color * Opacity, Rotation, SpriteEffects, 0f);
        }
    }

    public Texture2D? Texture { get; set; }

    public Color Color { get; set; } = Color.White;

    public Rectangle? SourceRectangle { get; set; }

    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

    public float Rotation { get; set; }

    public Vector2 Origin { get; set; } = Vector2.Zero;

    public bool Stretch { get; set; }

    public bool PreserveAspectRatio { get; set; } = true;
}
