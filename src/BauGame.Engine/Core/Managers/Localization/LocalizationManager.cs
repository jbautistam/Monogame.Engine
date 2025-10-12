using System.Globalization;
using System.Resources;

namespace Bau.Monogame.Engine.Domain.Core.Managers.Localization;

/// <summary>
///     Controla los datos de localización del juego, incluyendo la recuperación de las culturas admitidas y configuración de
///     la cultura actual a partir de la localización
/// </summary>
public class LocalizationManager
{
    /// <summary>
    ///     Recupera una lista de las culturas admitidas en el juego
    /// </summary>
    public List<CultureInfo> GetSupportedCultures()
    {
        List<CultureInfo> supportedCultures = [];
        ResourceManager resourceManager = new(GameEngine.Instance.EngineSettings.ResourceFolder, 
                                              GameEngine.Instance.EngineSettings.MainAssembly);
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            // Obtiene las culturas
            foreach (CultureInfo culture in cultures)
                try
                {
                    ResourceSet? resourceSet = resourceManager.GetResourceSet(culture, true, false);

                        // Añade el conjunto de recursos
                        if (resourceSet is not null)
                            supportedCultures.Add(culture);
                }
                catch {}
            // Añade siempre la cultura predeterminada
            supportedCultures.Add(CultureInfo.InvariantCulture);
            // Devuelve las culturas admitidas
            return supportedCultures;
    }

    /// <summary>
    ///     Asigna una cultura
    /// </summary>
    public void SetCulture(string? culture)
    {
        CultureInfo cultureInfo = new(Normalize(culture));

            // Asigna la cultura al hilo actual
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

        // Normaliza la cadena de cultura
        string Normalize(string? culture)
        {
            // Deja la cultura predeterminada si no se ha pasado ninguna
            if (string.IsNullOrEmpty(culture))
                culture = "en-EN";
            // Devuelve la cultura normalizada
            return culture;
        }
    }
}