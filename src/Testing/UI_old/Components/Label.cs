using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI
{
	/// <summary>
	/// Componente de texto
	/// </summary>
	public class Label : UiComponent
    {
        public enum TextAlignmentMode { Left, Center, Right }
        public enum VerticalAlignmentMode { Top, Middle, Bottom }

        public string Text { get; set; } = "";
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public TextAlignmentMode HorizontalAlignment { get; set; } = TextAlignmentMode.Left;
        public VerticalAlignmentMode VerticalAlignment { get; set; } = VerticalAlignmentMode.Top;
        public bool AutoScale { get; set; } = false;
        public Vector2 Scale { get; set; } = Vector2.One;

		protected override void InvalidateSelf()
		{
		}

		protected override void UpdateSelf(GameTime gameTime)
		{
		}

        protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (string.IsNullOrEmpty(Text) || Font == null) return;

            var content = ContentBounds;
            var textSize = Font.MeasureString(Text) * Scale;
            
            Vector2 position = new Vector2(content.X, content.Y);

            // Alineación horizontal
            switch (HorizontalAlignment)
            {
                case TextAlignmentMode.Center:
                    position.X = content.X + (content.Width - textSize.X) / 2f;
                    break;
                case TextAlignmentMode.Right:
                    position.X = content.Right - textSize.X;
                    break;
            }

            // Alineación vertical
            switch (VerticalAlignment)
            {
                case VerticalAlignmentMode.Middle:
                    position.Y = content.Y + (content.Height - textSize.Y) / 2f;
                    break;
                case VerticalAlignmentMode.Bottom:
                    position.Y = content.Bottom - textSize.Y;
                    break;
            }

            // Clipping opcional: solo dibuja si cabe
            if (textSize.X <= content.Width && textSize.Y <= content.Height)
            {
                spriteBatch.DrawString(Font, Text, position, TextColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
            else if (AutoScale)
            {
                // Escala para ajustar
                float scaleX = content.Width / textSize.X;
                float scaleY = content.Height / textSize.Y;
                float finalScale = Math.Min(scaleX, scaleY);
                
                var scaledSize = textSize * finalScale;
                position.X = content.X + (content.Width - scaledSize.X) / 2f;
                position.Y = content.Y + (content.Height - scaledSize.Y) / 2f;
                
                spriteBatch.DrawString(Font, Text, position, TextColor, 0f, Vector2.Zero, finalScale, SpriteEffects.None, 0f);
            }
        }
    }
}
