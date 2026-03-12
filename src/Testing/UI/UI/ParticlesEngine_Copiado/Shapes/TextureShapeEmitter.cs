using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.ParticlesEngine.Shapes;

/// <summary>
///     Emisor de partículas a partir de una textura
/// </summary>
public class TextureShapeEmitter(string texture) : AbstractShapeEmitter
{
    // Variables privadas
    private List<Vector2> _points = [];
    private bool _initialized;

    /// <summary>
    ///     Obtiene los datos de emisión
    /// </summary>
    public override EmissionData GetEmissionData(Random random, EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position = Vector2.Zero;
        Vector2 normal = Vector2.UnitY;

            // Inicializa la textura si es necesario
            if (!_initialized)
            {
                ComputeTexturePoints(Texture);
                _initialized = true;
            }
            // Si realmente hay algo en la textura
            if (_points.Count > 0)
            {
                // Obtiene un punto aleatorio
                position = _points[random.Next(_points.Count)];
                // Calcula la normal
                normal = Vector2.Normalize(position);
                if (normal == Vector2.Zero) 
                    normal = Vector2.UnitY;
            }
            // Devuelve los datos
            return new EmissionData(position, ResolveDirection(directionMode, normal, fixedDirection, random));
    }

    /// <summary>
    ///     Calcula los puntos a partir de la textura
    /// </summary>
    private void ComputeTexturePoints(Texture2D texture)
    {
        Color[] pixelData = new Color[texture.Width * texture.Height];
        int centerX = (int) 0.5f * texture.Width;
        int centerY = (int) 0.5f * texture.Height;

            // Copia los datos de la textura en el array de colores
            texture.GetData(pixelData);
            // Recoge los puntos donde la textura tiene algún valor (la opacidad es mayor que cero)
            for (int y = 0; y < texture.Height; y++)
                for (int x = 0; x < texture.Width; x++)
                    if (pixelData[y * texture.Width + x].A > 0)
                        _points.Add(new Vector2(x - centerX, y - centerY));
    }

    /// <summary>
    ///     Textura
    /// </summary>
    public Texture2D Texture { get; } = texture;
}

