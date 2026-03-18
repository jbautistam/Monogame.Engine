using Microsoft.Xna.Framework;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Entities.UserInterface.ComicBubbles;

/// <summary>
///     Componente de interface de usuario para mostrar un tenxto en una burbuja de cómic
/// </summary>
public class UiComicBubble(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Variables privadas
    private Rectangle _bubbleBounds;

    /// <summary>
    ///     Calcula los límites del control
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
	{
        // Carga los datos de la fuente
        if (Font is not null && !string.IsNullOrEmpty(TextParameters.Text))
            TextParameters.Bounds = Position.ContentBounds;
        // Carga los datos del fondo del bocadillo
        if (BubbleSprite is not null)
        {
            Managers.Resources.TextureConfigurationManager.TextureResolved? texture = BubbleSprite.LoadAsset(Layer.Scene);

                if (texture is not null)
                {
                    UiMargin? padding = texture.Padding;

                        // Normaliza el padding
                        if (padding is null)
                            padding = new UiMargin(5);
                        // Calcula el nuevo tamaño del fondo del bocadillo = contenido + padding
                        _bubbleBounds = new Rectangle(Position.ContentBounds.X - padding.Value.Left,
                                                      Position.ContentBounds.Y - padding.Value.Top,
                                                      Position.ContentBounds.Width + padding.Value.Horizontal,
                                                      Position.ContentBounds.Height + padding.Value.Vertical);
                }
        }
    }

    /// <summary>
    ///     Actualiza los datos
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        Font?.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja el componente
    /// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
        // Dibuja el bocadillo
        if (BubbleSprite is not null)
            renderingManager.SpriteRenderer.Draw(BubbleSprite, _bubbleBounds, Vector2.Zero, 0, BubbleColor);
        // Dibuja el texto
        DrawText(renderingManager);
	}

    /// <summary>
    ///     Dibuja el texto del bocadillo
    /// </summary>
    private void DrawText(Scenes.Rendering.RenderingManager renderingManager)
    {
        if (Font is not null)
            renderingManager.SpriteTextRenderer.DrawString(Font, TextParameters);
    }

    /// <summary>
    ///     Fuente
    /// </summary>
    public Sprites.SpriteTextDefinition? Font { get; set; }

    /// <summary>
    ///     Parámetros de texto
    /// </summary>
    public Sprites.SpriteTextParameters TextParameters { get; set; } = new();

    /// <summary>
    ///     Sprite con el bocadillo
    /// </summary>
    public Sprites.SpriteDefinition? BubbleSprite { get; set; }

    /// <summary>
    ///     Color del bocadillo
    /// </summary>
    public Color BubbleColor { get; set; } = Color.White;
}