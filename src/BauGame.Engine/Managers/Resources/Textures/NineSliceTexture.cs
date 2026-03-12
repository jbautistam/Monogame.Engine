using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

/// <summary>
///     Textura de nueve partes para botones
/// </summary>
public class NineSliceTexture(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
	/// <summary>
	///		Obtiene la región de la textura
	/// </summary>
	public override TextureRegion? GetRegion(string? name)
	{
		Texture2D? texture = GetTexture();

			// Obtiene la textura
			if (texture is not null)
				return new TextureRegion(NormalizeName(name))
								{
									Texture = texture,
									Region = new Rectangle(0, 0, texture.Width, texture.Height)
								};
			else
				return null;
	}

    /// <summary>
    ///     Tamaño de sección a la izquierda
    /// </summary>
    public int LeftSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección a la derecha
    /// </summary>
    public int RightSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección desde arriba
    /// </summary>
    public int TopSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección desde abajo
    /// </summary>
    public int BottomSlice { get; set; }
}

/*
/// <summary>
/// Nine-Slice con control total de cada esquina (ancho/alto independientes)
/// </summary>
public class NineSliceTexture
{
    public NineSliceTexture(Texture2D texture, NineSliceConfig config)
    {
        Texture = texture;
        _config = config;
        GenerateSlices();
    }
    
    private void GenerateSlices()
    {
        int w = Texture.Width;
        int h = Texture.Height;
        
        var cfg = _config;
        
        // Calcular posiciones en la textura fuente
        int x0 = 0;
        int x1 = cfg.TopLeft.Width; // También BottomLeft.Width si son diferentes, asumimos consistencia
        int x2 = w - cfg.TopRight.Width;
        
        int y0 = 0;
        int y1 = cfg.TopLeft.Height; // También TopRight.Height
        int y2 = h - cfg.BottomLeft.Height;
        
        // Ajustar si los bordes horizontales tienen altos diferentes en las esquinas
        // Usamos el máximo para el área central vertical
        int leftHeight = cfg.TopLeft.Height + cfg.BottomLeft.Height;
        int rightHeight = cfg.TopRight.Height + cfg.BottomRight.Height;
        
        // Anchos de bordes verticales
        int topWidth = cfg.TopLeft.Width + cfg.TopRight.Width;
        int bottomWidth = cfg.BottomLeft.Width + cfg.BottomRight.Width;
        
        // Rectángulos fuente (source)
        _slices[0] = CreateSlice("TL", x0, y0, cfg.TopLeft.Width, cfg.TopLeft.Height);
        _slices[1] = CreateSlice("T", x1, y0, w - cfg.TopLeft.Width - cfg.TopRight.Width, cfg.TopEdgeHeight);
        _slices[2] = CreateSlice("TR", x2, y0, cfg.TopRight.Width, cfg.TopRight.Height);
        
        _slices[3] = CreateSlice("L", x0, y1, cfg.LeftEdgeWidth, h - cfg.TopLeft.Height - cfg.BottomLeft.Height);
        _slices[4] = CreateSlice("C", x1, y1, w - cfg.LeftEdgeWidth - cfg.RightEdgeWidth, h - cfg.TopEdgeHeight - cfg.BottomEdgeHeight);
        _slices[5] = CreateSlice("R", x2, y1, cfg.RightEdgeWidth, h - cfg.TopRight.Height - cfg.BottomRight.Height);
        
        _slices[6] = CreateSlice("BL", x0, y2, cfg.BottomLeft.Width, cfg.BottomLeft.Height);
        _slices[7] = CreateSlice("B", x1, y2, w - cfg.BottomLeft.Width - cfg.BottomRight.Width, cfg.BottomEdgeHeight);
        _slices[8] = CreateSlice("BR", x2, y2, cfg.BottomRight.Width, cfg.BottomRight.Height);
    }
    
    private TextureRegion CreateSlice(string name, int x, int y, int width, int height)
    {
        return new TextureRegion
        {
            Name = name,
            SourceRect = new Rectangle(x, y, Math.Max(0, width), Math.Max(0, height))
        };
    }
    
}
*/
