using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.Transitions;

public class TransitionManager
{
    // Variables privadas
    public readonly ContentManager Content; // TODO: esto debería ser del motor
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDevice _graphics; // TODO: esto debería ser del motor
    private Effects.AbstractTransitionEffect? _currentTransition;
    private RenderTarget2D? _previousScreen;
    private RenderTarget2D? _nextScreen;
    public int _screenWidth, _screenHeight;
        
    public TransitionManager(GraphicsDevice graphics, ContentManager content)
    {
        _graphics = graphics;
        Content = content;
        _spriteBatch = new SpriteBatch(graphics);
    }

    /// <summary>
    ///     Inicia una transición
    /// </summary>
    public void StartTransition(Effects.AbstractTransitionEffect transition, RenderTarget2D previousScreen, RenderTarget2D nextScreen)
    {
        // Finaliza la transición actual
        if (IsTransitioning) 
            Skip();
        // Indica que está ejecutando una transición
        IsTransitioning = true;
        // Guarda las pantallas
        _previousScreen = previousScreen;
        _nextScreen = nextScreen;
        // Cambia la transición actual y la arranca
        _currentTransition = transition;
        _currentTransition.Start();
        // Captura la escena anterior
        _graphics.SetRenderTarget(previousScreen);
        _graphics.Clear(Color.Transparent);
        // Pre-renderizar escena siguiente
        _graphics.SetRenderTarget(nextScreen);
        _graphics.Clear(Color.Transparent);
        // Limpia los gráficos
        _graphics.SetRenderTarget(null);
    }
       
    /// <summary>
    ///     Actualiza el efecto de la transición
    /// </summary>
    public void Update(float deltaTime)
    {
        // Actualiza la transición
        if (IsTransitioning && _currentTransition is not null && _currentTransition.Shader is not null)
        {
            // Cambia las dimensiones de pantalla
            if (_graphics.Viewport.Width != _screenWidth || _graphics.Viewport.Height != _screenHeight)
            {
                // Guarda los nuevos datos
                _screenWidth = _graphics.Viewport.Width;
                _screenHeight = _graphics.Viewport.Height;
                // Elimina los datos anteriores
                _previousScreen?.Dispose();
                _nextScreen?.Dispose();
                // Recrea las capas de presentación
                _previousScreen = new RenderTarget2D(_graphics, _screenWidth, _screenHeight);
                _nextScreen = new RenderTarget2D(_graphics, _screenWidth, _screenHeight);
            }
            // Actualiza la transición
            if (_previousScreen is not null && _nextScreen is not null)
                _currentTransition?.Apply(_previousScreen, _nextScreen, deltaTime);
            else
                IsTransitioning = false;
        }
        // Indica si se ha finalizado la transición
        IsTransitioning = _currentTransition is null || _currentTransition.Shader is null || _currentTransition.IsComplete;
    }
      
    /// <summary>
    ///     Dibuja la transición
    /// </summary>
    public void Draw()
    {
        if (IsTransitioning && _currentTransition is not null && _currentTransition.Shader is not null)
        {            
            // Limpia la pantalla
            _graphics.Clear(Color.Black);
            // Abre el canvas con el efecto
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, 
                               RasterizerState.CullNone, _currentTransition.Shader);
            // Dibuja la pantalla anterior
            _spriteBatch.Draw(_previousScreen, Vector2.Zero, Color.White);
            // Cierra el canvas
            _spriteBatch.End();
        }
    }
        
    /// <summary>
    ///     Fuerza que se termine la transición
    /// </summary>
    public void Skip()
    {
        // Ejecuta la transición hasta el final
        _currentTransition?.SkipToEnd();
        // Indica que se ha acabado la transición
        IsTransitioning = false;
    }

    /// <summary>
    ///     Indica si se está ejecutando una transición
    /// </summary>
    public bool IsTransitioning { get; private set; }

    /// <summary>
    ///     Progreso de la transición actual
    /// </summary>
    public float CurrentProgress => _currentTransition?.Progress ?? 0;
}
