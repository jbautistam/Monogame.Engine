using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Transitions.Effects;

/// <summary>
///     Efecto de barras
/// </summary>
public class NoiseDissolveEffect(TransitionManager manager) : AbstractTransitionEffect(manager, "Effects/Transitions/NoiseDissolve")
{
	/// <summary>
	///     Inicializa la transición (por ejemplo, carga los shaders)
	/// </summary>
	protected override void StartShelf() {}

    /// <summary>
    ///     Actualiza los parámetros particulares del efecto
    /// </summary>
    protected override void UpdateParameters(float deltaTime)
    {
        if (Texture is not null)
        {
            Shader?.Parameters["NoiseTexture"]?.SetValue(GetDirection(Direction));
            Shader?.Parameters["EdgeColor"]?.SetValue(EdgeColor.ToVector4());
            Shader?.Parameters["EdgeWidth"]?.SetValue(EdgeWidth);
        }
    }

/*
    private void GenerateDefaultNoise()
    {
        int size = 256;
        _defaultNoise = new Texture2D(_graphics, size, size);
        Color[] data = new Color[size * size];
        Random rand = new Random();
            
        for (int i = 0; i < data.Length; i++)
        {
            byte value = (byte) rand.Next(256);

                data[i] = new Color(value, value, value, (byte) 255);
        }
            
        _defaultNoise.SetData(data);
    }
*/

    /// <summary>
    ///     Dirección del efecto
    /// </summary>
    public required DirectionType Direction { get; init; }

    /// <summary>
    ///     Textura
    /// </summary>
    public Texture2D? Texture { get; set; }

    /// <summary>
    ///     Color de los límites
    /// </summary>
    public required Color EdgeColor { get; init; }

    /// <summary>
    ///     Ancho de los bordes
    /// </summary>
    public required float EdgeWidth { get; init; }
}