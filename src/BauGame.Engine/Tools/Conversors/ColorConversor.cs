using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.Conversors;

/// <summary>
///     Conversión de colores
/// </summary>
public static class ColorConversor
{
    /// <summary>
    ///     Conversión de HSV a RGB (hue: 0-360, saturation: 0-1, value: 0-1)
    /// </summary>
    public static Color HsvToRgb(float hue, float saturation, float value)
    {
        float c, x, m;

            // Normaliza los valores de entrada
            hue = hue % 360f;
            if (hue < 0) 
                hue += 360f;
            saturation = MathHelper.Clamp(saturation, 0f, 1f);
            value = MathHelper.Clamp(value, 0f, 1f);
            // Variables intermedias        
            c = value * saturation;
            x = c * (1 - MathF.Abs((hue / 60f) % 2 - 1));
            m = value - c;
            // Convierte a RGB
            if (hue < 60f)
                return CreateColor(c, x, 0, m);
            else if (hue < 120f)
                return CreateColor(x, c, 0, m);
            else if (hue < 180f)
                return CreateColor(0, c, x, m);
            else if (hue < 240f)
                return CreateColor(0, x, c, m);
            else if (hue < 300f)
                return CreateColor(x, 0, c, m);
            else
                return CreateColor(c, 0, x, m);

            // Convierte el color
            Color CreateColor(float r, float g, float b, float m) => new Color((byte) ((r + m) * 255), (byte) ((g + m) * 255), (byte)((b + m) * 255));
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
