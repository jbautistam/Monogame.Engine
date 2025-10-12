using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.UI;

public class Button : UIElement
{
    public ClickableFrame Frame { get; set; }
    public Label Label { get; private set; }
    public Texture2D Icon { get; set; }
    public IconPosition IconPos { get; set; } = IconPosition.Left;
    public int IconSpacing { get; set; } = 10;
    public float IconScale { get; set; } = 1f;

    public enum IconPosition
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public Button(SpriteFont font, string text = "Button", int width = 150, int height = 40)
    {
        Frame = new ClickableFrame
        {
            Size = new Vector2(width, height),
            NormalColor = new Color(70, 70, 70),
            HoverColor = new Color(100, 100, 100),
            PressedColor = new Color(50, 50, 50),
            BorderColor = Color.Gray,
            BorderThickness = 2
        };
        Frame.OnClick += () => OnClick?.Invoke();

        Label = new Label(font, text) { TextColor = Color.White };
        AddChild(Label);

        Size = new Vector2(width, height);
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        if (Label.Font == null) return;

        Vector2 textSize = Label.Font.MeasureString(Label.Text);
        Vector2 iconSize = Icon != null ? new Vector2(Icon.Width * IconScale, Icon.Height * IconScale) : Vector2.Zero;

        float totalWidth = textSize.X;
        float totalHeight = textSize.Y;

        if (Icon != null)
        {
            switch (IconPos)
            {
                case IconPosition.Left:
                case IconPosition.Right:
                    totalWidth += iconSize.X + IconSpacing;
                    break;
                case IconPosition.Top:
                case IconPosition.Bottom:
                    totalHeight += iconSize.Y + IconSpacing;
                    break;
            }
        }

        Vector2 contentOffset = new Vector2((Size.X - totalWidth) / 2, (Size.Y - totalHeight) / 2);
        Vector2 iconPos = Vector2.Zero;
        Vector2 textPos = Vector2.Zero;

        switch (IconPos)
        {
            case IconPosition.Left:
                iconPos = contentOffset;
                textPos = iconPos + new Vector2(iconSize.X + IconSpacing, (iconSize.Y - textSize.Y) / 2);
                break;
            case IconPosition.Right:
                textPos = contentOffset;
                iconPos = textPos + new Vector2(textSize.X + IconSpacing, (textSize.Y - iconSize.Y) / 2);
                break;
            case IconPosition.Top:
                iconPos = contentOffset;
                textPos = iconPos + new Vector2((iconSize.X - textSize.X) / 2, iconSize.Y + IconSpacing);
                break;
            case IconPosition.Bottom:
                textPos = contentOffset;
                iconPos = textPos + new Vector2((textSize.X - iconSize.X) / 2, textSize.Y + IconSpacing);
                break;
        }

        Label.Position = textPos;
        _iconDrawPosition = AbsolutePosition + iconPos;
    }

    private Vector2 _iconDrawPosition;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Frame?.Update(gameTime);
        UpdateLayout();
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        Frame?.Draw(spriteBatch);
        if (Icon != null) spriteBatch.Draw(Icon, _iconDrawPosition, null, Color.White, 0f, Vector2.Zero, IconScale, SpriteEffects.None, 0f);
    }

    public event Action OnClick;
}
