using Bau.BauEngine.Tools.MathTools.Easing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Transitions.Effects;

/// <summary>
///     Clase abstracta para los efectos de transición
/// </summary>
public abstract class AbstractTransitionEffect(TransitionManager manager, string shaderFile)
{
    // Enumerados públicos
    /// <summary>
    ///     Tipos de direcciones de los efectos
    /// </summary>
    public enum DirectionType
    {
        /// <summary>Hacia la derecha</summary>
        Right,
        /// <summary>Hacia la izquierda</summary>
        Left,
        /// <summary>Hacia arriba</summary>
        Up,
        /// <summary>Hacia abajo</summary>
        Down,
        /// <summary>En diagonal</summary>
        Diagonal
    }
    // Variables privadas
    private float _elapsed;

    /// <summary>
    ///     Inicializa la transición (por ejemplo, carga los shaders)
    /// </summary>
    public void Start()
    {
        try
        {
            Shader = Manager.Content.Load<Effect>(ShaderFile);
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"Error when load shader {ShaderFile}. {exception.Message}");
        }
    }
        
    /// <summary>
    ///     Ejecuta los pasos de inicio del efecto
    /// </summary>
    protected abstract void StartShelf();

    /// <summary>
    ///     Actualiza la transición
    /// </summary>
    public void Apply(RenderTarget2D previousScreen, RenderTarget2D nextScreen, float deltaTime)
    {
        if (!IsComplete)
        {
            float rawProgress;

                // Calcula el progreso en crudo (porque puede que apliquemos un suavizado)
                _elapsed += deltaTime;
                rawProgress = MathHelper.Clamp(_elapsed / Duration, 0, 1);
                // Aplica la función de easing al progreso
                if (Easing is not null)
                    Progress = EasingFunctionsHelper.Apply(rawProgress, Easing ?? EasingFunctionsHelper.EasingType.Linear);
                else
                    Progress = rawProgress;
                // Ejecuta el efecto
                if (rawProgress >= 1.0f)
                    IsComplete = true;
                else if (Shader is not null)
                {
                    float progress = IsReversed ? 1 - Progress : Progress;
            
                        // Asigna los parámetros generales al progreso
                        Shader.Parameters["Progress"]?.SetValue(progress);
                        Shader.Parameters["PreviousTexture"]?.SetValue(previousScreen);
                        Shader.Parameters["NextTexture"]?.SetValue(nextScreen);
                        Shader.Parameters["ScreenSize"]?.SetValue(new Vector2(Manager._screenWidth, Manager._screenHeight));
                        // Actualiza los parámetros
                        UpdateParameters(deltaTime);
                }
        }
    }

    /// <summary>
    ///     Actualiza los parámetros particulares del efecto
    /// </summary>
    protected abstract void UpdateParameters(float deltaTime);

    /// <summary>
    ///     Obtiene el vector que se tiene que utilizar para cambiar la dirección
    /// </summary>
    protected Vector2 GetDirection(DirectionType direction)
    {
        return direction switch
                    { 
                        DirectionType.Right => Vector2.UnitX,
                        DirectionType.Up => -Vector2.UnitY,
                        DirectionType.Down => Vector2.UnitY,
                        DirectionType.Diagonal => Vector2.One,
                        _ => -Vector2.UnitX // left
                    };
    }

    /// <summary>
    ///     Salta al final
    /// </summary>
    public void SkipToEnd()
    {
        Progress = 1.0f;
        IsComplete = true;
    }
        
    /// <summary>
    ///     Cambia el orden de aplicación
    /// </summary>
    public void Reverse()
    {
        IsReversed = !IsReversed;
        _elapsed = Duration - _elapsed;
    }

    /// <summary>
    ///     Manager de transiciones
    /// </summary>
    public TransitionManager Manager { get; } = manager;

    /// <summary>
    ///     Duración del efecto
    /// </summary>
    public required float Duration { get; init; }

    /// <summary>
    ///     Función de suavizado
    /// </summary>
    public EasingFunctionsHelper.EasingType? Easing { get; }

    /// <summary>
    ///     Nombre del archivo de Shader
    /// </summary>
    public string ShaderFile { get; } = shaderFile;

    /// <summary>
    ///     Shader del efecto
    /// </summary>
    public Effect? Shader { get; private set; }

    /// <summary>
    ///     Porcentaje de progreso
    /// </summary>
    public float Progress { get; private set; }

    /// <summary>
    ///     Indica si ha terminado
    /// </summary>
    public bool IsComplete { get; private set; }

    /// <summary>
    ///     Indica si se ejecuta al revés
    /// </summary>
    public bool IsReversed { get; set; }
}