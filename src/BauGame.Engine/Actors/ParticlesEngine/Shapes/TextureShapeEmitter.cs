using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Actors.ParticlesEngine.Shapes;

/// <summary>
///     Emisor de partículas a partir de una textura
/// </summary>
public class TextureShapeEmitter(string texture, string? region) : AbstractShapeEmitter
{
    // Variables privadas
    private List<Vector2> _points = [];
    private bool _initialized;

    /// <summary>
    ///     Obtiene los datos de emisión
    /// </summary>
    public override EmissionData GetEmissionData(EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position = Vector2.Zero;
        Vector2 normal = Vector2.UnitY;

            // Inicializa la textura si es necesario
            if (!_initialized)
            {
                Texture2D? texture = LoadTexture();

                    // Calcula los puntos
                    if (texture is not null)
                        ComputeTexturePoints(texture);
                    // Indica que se ha inicializado
                    _initialized = true;
            }
            // Si realmente hay algo en la textura
            if (_points.Count > 0)
            {
                // Obtiene un punto aleatorio
                position = _points[Tools.Randomizer.Random.Next(_points.Count)];
                // Calcula la normal
                normal = Vector2.Normalize(position);
                if (normal == Vector2.Zero) 
                    normal = Vector2.UnitY;
            }
            // Devuelve los datos
            return new EmissionData(position, ResolveDirection(directionMode, normal, fixedDirection));
    }

    /// <summary>
    ///     Carga una textura
    /// </summary>
    private Texture2D? LoadTexture()
    {
        //TODO: se debería cargar la textura aquí
        return null;
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
    ///     Definición del sprite
    /// </summary>
    public Entities.Sprites.SpriteDefinition Sprite { get; } = new(texture, region);
}

