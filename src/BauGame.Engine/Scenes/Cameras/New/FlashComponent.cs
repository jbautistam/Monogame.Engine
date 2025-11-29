public class FlashComponent : ICamaraComponent
{
    private Color _color;
    private float _duration, _timer;
    private bool _active;

    public void Trigger(Color color, float duration)
    {
        _color = color;
        _duration = duration;
        _timer = 0f;
        _active = true;
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (_active)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _duration)
                _active = false;
        }
    }

    public void Reset() => _active = false;

    // Método extra para dibujar
    public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
    {
        if (!_active) return;
        float alpha = 1f - (_timer / _duration);
        var rect = new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height);
        spriteBatch.Draw(_pixel, rect, _color * alpha);
    }

    // Necesitas una textura blanca de 1x1 (pásala en el constructor o usa una estática)
    private static Texture2D _pixel;
    public static void InitializePixel(GraphicsDevice gd)
    {
        if (_pixel == null)
        {
            _pixel = new Texture2D(gd, 1, 1);
            _pixel.SetData(new Color[] { Color.White });
        }
    }
}