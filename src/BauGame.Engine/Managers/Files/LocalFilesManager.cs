namespace Bau.Libraries.BauGame.Engine.Managers.Files;

/// <summary>
///     Manager del sistema de archivos local (lectura y escritura)
/// </summary>
public class LocalFilesManager(FilesManager filesManager) : AbstractFilesManager(filesManager)
{

    /// <summary>
    ///     Lee un archivo de texto
    /// </summary>
    public override string? ReadTextFile(string fileName)
    {
        // Lee el contenido del archivo
        try
        {
            if (FileExists(fileName))
                return File.ReadAllText(GetFullFileName(fileName));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error reading file {GetFullFileName(fileName)}: {ex.Message}");
        }
        // Si ha llegado hasta aquí es porque no ha podido leer correctamente
        return null;
    }
    
    /// <summary>
    ///     Lee un archivo binario
    /// </summary>
    public byte[] ReadBinaryFile(string fileName)
    {
        // Carga el archivo
        try
        {
            if (FileExists(GetFullFileName(fileName)))
                return File.ReadAllBytes(GetFullFileName(fileName));
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"Error reading binary file {GetFullFileName(fileName)}: {exception.Message}");
        }
        // Devuelve un valor vacío
        return [];
    }
    
    /// <summary>
    ///     Escribe un texto a un archivo
    /// </summary>
    public bool WriteTextFile(string fileName, string content)
    {
        bool written = false;

            // Escribe el archivo
            try
            {
                // Graba el archivo
                File.WriteAllText(GetFullFileName(fileName), content);
                // Indica que se ha posido grabar
                written = true;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Error writing file {GetFullFileName(fileName)}: {exception.Message}");
            }
            // Devuelve el valor que indica si se ha podido escribir el archivo
            return written;
    }

    /// <summary>
    ///     Escribe un archivo binario
    /// </summary>
    public bool WriteBinaryFile(string fileName, byte[] data)
    {
        bool written = false;

            // Escribe el contenido del archivo
            try
            {
                File.WriteAllBytes(GetFullFileName(fileName), data);
                written = true;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Error writing binary file {GetFullFileName(fileName)}: {exception.Message}");
            }
            // Devuelve el valor que indica si se ha grabado el archivo
            return written;
    }

    /// <summary>
    ///     Obtiene el nombre de archivo completo a partir de un nombre relativo
    /// </summary>
    public string GetFullFileName(string fileName) => Path.Combine(GetBasePlatformFolder(), fileName);

    /// <summary>
    ///     Comprueba si existe un archivo (el nombre de archivo es relativo a la carpeta de la plataforma)
    /// </summary>
    public bool FileExists(string fileName) => File.Exists(GetFullFileName(fileName));

    /// <summary>
    ///     Obtiene la carpeta específica de la plataforma
    /// </summary>
    private string GetBasePlatformFolder()
    {
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT:
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            case PlatformID.Unix:
                if (Directory.Exists("/Users")) // MAC
                    return Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? string.Empty, "Library", "Application Support");
                else
                {
                    string? xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

                    if (string.IsNullOrEmpty(xdgDataHome))
                        return Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? string.Empty, ".local", "share");
                    else
                        return xdgDataHome;
                }
            default: // móvil
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
