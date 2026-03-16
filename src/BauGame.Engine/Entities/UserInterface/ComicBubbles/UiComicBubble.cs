using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.ComicBubbles;

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
        if (Font is not null && !string.IsNullOrEmpty(Text))
        {   
            SpriteFont? spriteFont = Font.LoadAsset();

                if (spriteFont is not null)
                {
                    List<string> lines = new Popups.MobileChats.StringFontHelper().WrapText(spriteFont, Text, TextScale, Position.ContentBounds.Width);
                    float contentHeight = 0f;

                        // Calcula el tamaño de las líneas            
                        foreach (string line in lines)
                        {
                            Vector2 size = spriteFont.MeasureString(line) * TextScale;

                                if (contentHeight < Position.ContentBounds.Height)
                                    contentHeight += size.Y + spriteFont.LineSpacing * LineSpacing * TextScale;
                        }
                }
        }
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
	}

    /// <summary>
    ///     Dibuja el componente
    /// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
        if (!string.IsNullOrWhiteSpace(Text))
        {
            // Dibuja el bocadillo
            if (BubbleSprite is not null)
                camera.RenderingManager.SpriteRenderer.Draw(BubbleSprite, _bubbleBounds, Vector2.Zero, 0, BubbleColor);
            // Dibuja el texto
            DrawText(camera);
        }
	}

    /// <summary>
    ///     Dibuja el texto del bocadillo
    /// </summary>
    private void DrawText(Camera2D camera)
    {
        SpriteFont? spriteFont = Font?.LoadAsset();

            if (Font is not null && spriteFont is not null)
            {
                List<string> lines = new Popups.MobileChats.StringFontHelper().WrapText(spriteFont, Text, TextScale, Position.ContentBounds.Width);
                float lineHeight = spriteFont.LineSpacing * LineSpacing * TextScale;
                float textHeight = lines.Count * lineHeight;
                float startY;

                    // Quita el último espaciado de las líneas
                    textHeight -= lineHeight - spriteFont.LineSpacing * TextScale;
                    // Calcula la posición inicial dependiendo de la alineación
                    switch (VerticalAlignment)
                    {
                        case UiLabel.VerticalAlignmentType.Center: 
                                startY = Position.ContentBounds.Y + (Position.ContentBounds.Height - textHeight) / 2f; 
                            break;
                        case UiLabel.VerticalAlignmentType.Bottom: 
                                startY = Position.ContentBounds.Y + Position.ContentBounds.Height - textHeight; 
                            break;
                        default: 
                                startY = Position.ContentBounds.Y; 
                            break;
                    }
                    // Dibuja las líneas
                    for (int index = 0; index < lines.Count; index++)
                    {
                        string line = lines[index];
                        Vector2 size = spriteFont.MeasureString(line) * TextScale;
                        float x;
                        float y = startY + (index * lineHeight);
                
                            // Posición X según alineación
                            switch (HorizontalAlignment)
                            {
                                case UiLabel.HorizontalAlignmentType.Center: 
                                        x = Position.ContentBounds.X + (Position.ContentBounds.Width - size.X) / 2f; 
                                    break;
                                case UiLabel.HorizontalAlignmentType.Right: 
                                        x = Position.ContentBounds.X + Position.ContentBounds.Width - size.X; 
                                    break;
                                default:
                                        x = Position.ContentBounds.X; 
                                    break;
                            }
                            // Calcula la coordenada Y
                            y = Math.Min(y, y + textHeight - lineHeight);
                            // Dibuja el texto
                            camera.RenderingManager.SpriteTextRenderer.DrawString(Font, line, new Vector2(x, y), Color);
                    }
            }
    }

    /// <summary>
    ///     Texto
    /// </summary>
    public string Text { get; set; } = default!;

    /// <summary>
    ///     Fuente
    /// </summary>
    public Common.Sprites.SpriteTextDefinition? Font { get; set; }
        
    /// <summary>
    ///     Alineación horizontal
    /// </summary>
    public UiLabel.HorizontalAlignmentType HorizontalAlignment { get; set; } = UiLabel.HorizontalAlignmentType.Center;

    /// <summary>
    ///     Alineación vertical
    /// </summary>
    public UiLabel.VerticalAlignmentType VerticalAlignment { get; set; } = UiLabel.VerticalAlignmentType.Center;

    /// <summary>
    ///     Color del texto
    /// </summary>
    public Color Color { get; set; } = Color.Black;

    /// <summary>
    ///     Sprite con el bocadillo
    /// </summary>
    public Common.Sprites.SpriteDefinition? BubbleSprite { get; set; }

    /// <summary>
    ///     Color del bocadillo
    /// </summary>
    public Color BubbleColor { get; set; } = Color.White;

    /// <summary>
    ///     Escala del texto
    /// </summary>
    public float TextScale { get; set; } = 1.0f;

    /// <summary>
    ///     Espaciado de línea
    /// </summary>
    public float LineSpacing { get; set; } = 1.0f;
}