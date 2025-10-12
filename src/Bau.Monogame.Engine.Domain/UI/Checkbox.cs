using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.UI;

public class Checkbox : Button
{
    public Texture2D CheckedTexture { get; set; }
    public Texture2D UncheckedTexture { get; set; }
    public bool IsChecked { get; private set; } = false;

    public event Action<Checkbox> OnCheckedChanged;

    public Checkbox(SpriteFont font, string text = "Checkbox", int width = 150, int height = 40) : base(font, text, width, height)
    {
        Frame.BackgroundColor = Color.Transparent;
        Frame.HoverColor = Color.Transparent;
        Frame.PressedColor = Color.Transparent;
        Frame.BorderColor = Color.Transparent;
        Frame.DrawBackground = false;
        Frame.DrawBorder = false;
        OnClick += Toggle;
    }

    private void Toggle()
    {
        IsChecked = !IsChecked;
        OnCheckedChanged?.Invoke(this);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Icon = IsChecked ? CheckedTexture : UncheckedTexture;
    }

    public void SetChecked(bool checkedState, bool triggerEvent = true)
    {
        if (IsChecked == checkedState) return;
        IsChecked = checkedState;
        if (triggerEvent) OnCheckedChanged?.Invoke(this);
    }
}
