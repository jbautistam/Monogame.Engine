using System.Text.Json;
namespace Bau.Monogame.Engine.Domain.Core.Managers.Files;

/// <summary>
///     Base para los manager del sistema de archivos
/// </summary>
public abstract class AbstractFilesManager(FilesManager filesManager)
{
    /// <summary>
    ///     Lee un archivo de texto
    /// </summary>
    public abstract string? ReadTextFile(string fileName);

    /// <summary>
    ///     Carga un texto JSON y lo convierte a un objeto
    /// </summary>
    public TypeData? LoadJsonData<TypeData>(string fileName)
    {
        string? json = ReadTextFile(fileName);

            // Convierte la cadena leida
            if (!string.IsNullOrWhiteSpace(json))
                try
                {
                    return JsonSerializer.Deserialize<TypeData>(json, 
                                                                new JsonSerializerOptions
                                                                            {
                                                                                PropertyNameCaseInsensitive = true
                                                                            }
                                                                );
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine($"Error converting JSON {json}: {exception.Message}");
                }
            // Si ha llegado hasta aquí es porque no ha encontrado nada
            return default;
    }

	/// <summary>
	///		Manager principal
	/// </summary>
	public FilesManager FilesManager { get; } = filesManager;
}
