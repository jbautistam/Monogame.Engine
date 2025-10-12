using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.UI;

public class Frame : UIElement
{
    public Texture2D Texture { get; set; }
    public Color BackgroundColor { get; set; } = new Color(30, 30, 30, 200);
    public Color BorderColor { get; set; } = new Color(100, 100, 100, 255);
    public int BorderThickness { get; set; } = 2;
    public Vector2 Padding { get; set; } = Vector2.Zero;
    public bool DrawBackground { get; set; } = true;
    public bool DrawBorder { get; set; } = true;

    public enum DrawMode
    {
        Stretch,
        NineSlice
    }

    public DrawMode Mode { get; set; } = DrawMode.Stretch;
    public int CornerSize { get; set; } = 10;

    public enum GradientType
    {
        None,
        Horizontal,
        Vertical,
        Radial
    }

    public GradientType BackgroundGradient { get; set; } = GradientType.None;
    public Color BackgroundColor2 { get; set; } = Color.White;
    public GradientType BorderGradient { get; set; } = GradientType.None;
    public Color BorderColor2 { get; set; } = Color.White;

    public Frame()
    {
        LayerDepth = 0.9f;
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;
        if (Texture != null && Mode == DrawMode.NineSlice) DrawNineSlice(spriteBatch);
        else DrawStretchOrFlat(spriteBatch);
    }

    private void DrawStretchOrFlat(SpriteBatch spriteBatch)
    {
        if (DrawBackground)
        {
            if (Texture != null && Mode == DrawMode.Stretch) spriteBatch.Draw(Texture, Bounds, Color.White);
            else if (BackgroundGradient != GradientType.None) DrawGradient(spriteBatch, Bounds, BackgroundColor, BackgroundColor2, BackgroundGradient);
            else spriteBatch.Draw(TextureHelper.Pixel, Bounds, BackgroundColor);
        }

        if (DrawBorder && BorderThickness > 0)
        {
            var rect = Bounds;
            int t = BorderThickness;
            if (BorderGradient != GradientType.None)
            {
                DrawGradient(spriteBatch, new Rectangle(rect.X, rect.Y, rect.Width, t), BorderColor, BorderColor2, BorderGradient);
                DrawGradient(spriteBatch, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), BorderColor, BorderColor2, BorderGradient);
                DrawGradient(spriteBatch, new Rectangle(rect.X, rect.Y, t, rect.Height), BorderColor, BorderColor2, BorderGradient);
                DrawGradient(spriteBatch, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), BorderColor, BorderColor2, BorderGradient);
            }
            else
            {
                spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), BorderColor);
                spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), BorderColor);
                spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y, t, rect.Height), BorderColor);
                spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), BorderColor);
            }
        }
    }

    private void DrawNineSlice(SpriteBatch spriteBatch)
    {
        if (Texture == null || Texture.Width < CornerSize * 2 || Texture.Height < CornerSize * 2) return;

        int w = Bounds.Width, h = Bounds.Height, cx = CornerSize, cy = CornerSize;

        spriteBatch.Draw(Texture, new Rectangle(Bounds.X, Bounds.Y, cx, cy), new Rectangle(0, 0, cx, cy), Color.White);
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + w - cx, Bounds.Y, cx, cy), new Rectangle(Texture.Width - cx, 0, cx, cy), Color.White);
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X, Bounds.Y + h - cy, cx, cy), new Rectangle(0, Texture.Height - cy, cx, cy), Color.White);
        spriteBatch.Draw(Texture, new Rectangle(Bounds.X + w - cx, Bounds.Y + h - cy, cx, cy), new Rectangle(Texture.Width - cx, Texture.Height - cy, cx, cy), Color.White);

        if (w > cx * 2)
        {
            spriteBatch.Draw(Texture, new Rectangle(Bounds.X + cx, Bounds.Y, w - cx * 2, cy), new Rectangle(cx, 0, Texture.Width - cx * 2, cy), Color.White);
            spriteBatch.Draw(Texture, new Rectangle(Bounds.X + cx, Bounds.Y + h - cy, w - cx * 2, cy), new Rectangle(cx, Texture.Height - cy, Texture.Width - cx * 2, cy), Color.White);
        }

        if (h > cy * 2)
        {
            spriteBatch.Draw(Texture, new Rectangle(Bounds.X, Bounds.Y + cy, cx, h - cy * 2), new Rectangle(0, cy, cx, Texture.Height - cy * 2), Color.White);
            spriteBatch.Draw(Texture, new Rectangle(Bounds.X + w - cx, Bounds.Y + cy, cx, h - cy * 2), new Rectangle(Texture.Width - cx, cy, cx, Texture.Height - cy * 2), Color.White);
        }

        if (w > cx * 2 && h > cy * 2)
        {
            spriteBatch.Draw(Texture, new Rectangle(Bounds.X + cx, Bounds.Y + cy, w - cx * 2, h - cy * 2), new Rectangle(cx, cy, Texture.Width - cx * 2, Texture.Height - cy * 2), Color.White);
        }
    }

    private void DrawGradient(SpriteBatch spriteBatch, Rectangle rect, Color color1, Color color2, GradientType type)
    {
        if (type == GradientType.None) { spriteBatch.Draw(TextureHelper.Pixel, rect, color1); return; }

        int steps = Math.Max(rect.Width, rect.Height);
        for (int i = 0; i < steps; i++)
        {
            float t = (float)i / steps;
            Color color = Color.Lerp(color1, color2, t);

            switch (type)
            {
                case GradientType.Horizontal:
                    spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X + i, rect.Y, 1, rect.Height), color);
                    break;
                case GradientType.Vertical:
                    spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y + i, rect.Width, 1), color);
                    break;
                case GradientType.Radial:
                    Vector2 center = new Vector2(rect.Center.X, rect.Center.Y);
                    float maxDist = Math.Max(rect.Width, rect.Height) / 2f;
                    for (int x = rect.X; x < rect.Right; x++)
                        for (int y = rect.Y; y < rect.Bottom; y++)
                        {
                            float dist = Vector2.Distance(center, new Vector2(x, y));
                            float t2 = MathHelper.Clamp(dist / maxDist, 0, 1);
                            Color c = Color.Lerp(color1, color2, t2);
                            spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(x, y, 1, 1), c);
                        }
                    return;
            }
        }
    }

    protected override void UpdateUI(GameTime gameTime) { }
}
