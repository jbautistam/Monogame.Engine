using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bau.Monogame.Engine.Domain.UI;

public enum TextAlignment
{
    Left,
    Center,
    Right
}

public class Label : UIElement
{
    public string Text { get; set; } = "";
    public SpriteFont Font { get; set; }
    public Color TextColor { get; set; } = Color.White;
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;
    public bool WordWrap { get; set; } = false;
    public Vector2 Padding { get; set; } = Vector2.Zero;
    public bool AutoSizeHeight { get; set; } = true;
    public Color BackgroundColor { get; set; } = Color.Transparent;
    public Color BorderColor { get; set; } = Color.Transparent;
    public int BorderThickness { get; set; } = 1;

    private List<string> _wrappedLines = new();

    public Label(SpriteFont font, string text = "")
    {
        Font = font;
        Text = text;
        UpdateTextLayout();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateTextLayout();
    }

    private void UpdateTextLayout()
    {
        if (Font == null) return;
        _wrappedLines.Clear();

        if (WordWrap && Size.X > 0)
        {
            string[] words = Text.Split(' ');
            string currentLine = "";
            float maxWidth = Size.X - Padding.X * 2;

            foreach (string word in words)
            {
                string testLine = currentLine == "" ? word : currentLine + " " + word;
                Vector2 size = Font.MeasureString(testLine);
                if (size.X > maxWidth && currentLine != "")
                {
                    _wrappedLines.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }
            if (currentLine != "") _wrappedLines.Add(currentLine);
        }
        else
        {
            _wrappedLines.AddRange(Text.Split('\n'));
        }

        if (AutoSizeHeight && Font != null)
        {
            float lineHeight = Font.LineSpacing;
            Size = new Vector2(Size.X, lineHeight * Math.Max(1, _wrappedLines.Count) + Padding.Y * 2);
        }
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        if (!IsVisible || string.IsNullOrEmpty(Text) || Font == null) return;

        if (BackgroundColor != Color.Transparent)
            spriteBatch.Draw(TextureHelper.Pixel, Bounds, BackgroundColor);

        if (BorderColor != Color.Transparent && BorderThickness > 0)
            DrawBorder(spriteBatch);

        Vector2 drawPos = AbsolutePosition + Padding;
        float lineHeight = Font.LineSpacing;

        for (int i = 0; i < _wrappedLines.Count; i++)
        {
            string line = _wrappedLines[i];
            Vector2 lineSize = Font.MeasureString(line);
            Vector2 linePos = drawPos + new Vector2(0, i * lineHeight);

            switch (Alignment)
            {
                case TextAlignment.Center:
                    linePos.X += (Size.X - Padding.X * 2 - lineSize.X) / 2;
                    break;
                case TextAlignment.Right:
                    linePos.X += Size.X - Padding.X - lineSize.X;
                    break;
            }

            spriteBatch.DrawString(Font, line, linePos, TextColor);
        }
    }

    private void DrawBorder(SpriteBatch spriteBatch)
    {
        var rect = Bounds;
        int t = BorderThickness;
        spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), BorderColor);
        spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), BorderColor);
        spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y, t, rect.Height), BorderColor);
        spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), BorderColor);
    }
}
