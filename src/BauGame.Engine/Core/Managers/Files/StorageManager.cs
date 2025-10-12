using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Managers.Files;

/// <summary>
///		Manager del storage (TitleContainer) de Monogame (sólo lectura)
/// </summary>
public class StorageManager(FilesManager filesManager) : AbstractFilesManager(filesManager)
{
    /// <summary>
    ///     Lee el contenido de un archivo
    /// </summary>
    public override string? ReadTextFile(string fileName)
    {
        // Lee los datos del storage
        try
        {
            using (Stream stream = TitleContainer.OpenStream(fileName))
            {
                using (StreamReader reader = new(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"Error reading {fileName}: {exception.Message}");
        }
        // Si ha llegado hasta aquí es porque no ha podido leer nada
        return null;
    }

    /// <summary>
    ///     Lee el contenido binario de un archivo
    /// </summary>
    public byte[] ReadBinaryData(string fileName)
    {
        // Lee datos binarios
        try
        {
            using (Stream stream = TitleContainer.OpenStream(fileName))
            {
                using (MemoryStream memoryStream = new())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error reading binary data {fileName}: {exception.Message}");
        }
        // Si ha llegado hasta aquí es porque no ha podido leer
        return [];
    }
}
