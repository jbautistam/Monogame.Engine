using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.MathTools;

/// <summary>
///     Conversión de coloeres
/// </summary>
public static class ColorConversor
{
    /// <summary>
    ///     Conversión de HSV a RGB (h: 0-360, s: 0-1, v: 0-1)
    /// </summary>
    public static Color HsvToRgb(float h, float s, float v)
    {
        float c, x, m;
        float r, g, b;

            // Normaliza los valores de entrada
            h = h % 360f;
            if (h < 0) 
                h += 360f;
            s = MathHelper.Clamp(s, 0f, 1f);
            v = MathHelper.Clamp(v, 0f, 1f);
            // Variables intermedias        
            c = v * s;
            x = c * (1 - MathF.Abs((h / 60f) % 2 - 1));
            m = v - c;
            // Convierte a RGB
            if (h < 60f)
            {
                r = c; 
                g = x; 
                b = 0;
            }
            else if (h < 120f)
            {
                r = x; 
                g = c; 
                b = 0;
            }
            else if (h < 180f)
            {
                r = 0; 
                g = c; 
                b = x;
            }
            else if (h < 240f)
            {
                r = 0; 
                g = x; 
                b = c;
            }
            else if (h < 300f)
            {
                r = x; 
                g = 0; 
                b = c;
            }
            else
            {
                r = c; 
                g = 0; 
                b = x;
            }
            // Devuelve el color convertido
            return new Color((byte) ((r + m) * 255), (byte) ((g + m) * 255), (byte)((b + m) * 255));
    }
    
    /// <summary>
    ///     Convierte un HSV a RGB a partir de un Vector3 (h, s, v)
    /// </summary>
    public static Color HsvToRgb(Vector3 hsv) => HsvToRgb(hsv.X, hsv.Y, hsv.Z);
    
    /// <summary>
    ///     Convierte de RGB a HSV 
    ///     Devuelve un Vector3: h (0-360), s (0-1), v (0-1)
    /// </summary>
    public static Vector3 RgbToHsv(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;
        float max = MathF.Max(r, MathF.Max(g, b));
        float min = MathF.Min(r, MathF.Min(g, b));
        float delta = max - min;
        float h = 0f;
        float s = max == 0 ? 0 : delta / max;
        float v = max;
        
            // Obtiene los valores HSV
            if (delta != 0)
            {
                if (max == r)
                    h = 60f * (((g - b) / delta) % 6);
                else if (max == g)
                    h = 60f * ((b - r) / delta + 2);
                else
                    h = 60f * ((r - g) / delta + 4);
            }
            // Normaliza h
            if (h < 0) 
                h += 360f;
            // Devuelve el vector
            return new Vector3(h, s, v);
    }
    
    /// <summary>
    ///     Interpolación de colores RGB utilizando HSV
    /// </summary>
    public static Color LerpHsv(Color from, Color to, float t)
    {
        Vector3 hsvFrom = RgbToHsv(from);
        Vector3 hsvTo = RgbToHsv(to);
        float hueDiff = GetHueDiff(hsvFrom, hsvTo);
        float h = hsvFrom.X + hueDiff * t;

            // Normaliza el valor de Hue
            if (h < 0) 
                h += 360f;
            if (h >= 360f) 
                h -= 360f;
            // Devuelve el color interpolado
            return HsvToRgb(h, MathHelper.Lerp(hsvFrom.Y, hsvTo.Y, t), MathHelper.Lerp(hsvFrom.Z, hsvTo.Z, t));

        // Obtiene la diferencia de Hue para interpolar por el camino más corto
        float GetHueDiff(Vector3 hsvFrom, Vector3 hsvTo)
        {
            float hueDiff = hsvTo.X - hsvFrom.X;

                // Intenta la interpolación por el camino más corto
                if (hueDiff > 180f) 
                    hueDiff -= 360f;
                if (hueDiff < -180f) 
                    hueDiff += 360f;
                // Devuelve la diferencia normalizada
                return hueDiff;
        }
    }
}
