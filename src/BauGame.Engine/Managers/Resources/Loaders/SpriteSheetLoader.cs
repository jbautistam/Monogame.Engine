using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Resources.Loaders;

/// <summary>
///     Cargador para las definiciones de un <see cref="SpriteSheetModel"/>
/// </summary>
public class SpriteSheetLoader
{
    /// <summary>
    ///     Carga las definiciones de un <see cref="SpriteSheetModel"/> para una textura
    /// </summary>
    public SpriteSheetModel LoadFromJson(string textureId)
    {
        SpriteSheetModel spriteSheet = new(textureId);

            // Obtiene los datos del spritesheet de la textura
            using (Stream fileStream = TitleContainer.OpenStream($"Content/Definitions/{textureId}.spritesheet.json"))
            {
                Dictionary<string, Rectangle>? regions = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Rectangle>>(fileStream);

                    if (regions is not null)
                        foreach (KeyValuePair<string, Rectangle> keyValuePair in regions)
                            spriteSheet.Regions.Add(keyValuePair.Key, keyValuePair.Value);
            }
            // Devuelve la definición
            return spriteSheet;
    }
}

