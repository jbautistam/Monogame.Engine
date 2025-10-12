namespace Bau.Monogame.Engine.Domain.Core.Managers.Resources;

/// <summary>
///		Manager de contenido
/// </summary>
public class ContentDisposableManager(ResourcesManager resourcesManager) : IDisposable
{
    // Variables privadas
    private Dictionary<string, object?> _loadedAssets = new(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    ///     Carga un asset
    /// </summary>
    public TypeAsset? LoadAsset<TypeAsset>(string asset) where TypeAsset : class
    {
		// Devuelve el asset desde la caché o cargando el contenido
        if (_loadedAssets.TryGetValue(asset, out object? content))
            return content as TypeAsset;
        else
			try
			{
				TypeAsset assetContent = GameEngine.Instance.MonogameServicesManager.Content.Load<TypeAsset>(asset);

					// Añade el contenido al diccionario
					_loadedAssets.Add(asset, assetContent);
					// Devuelve el contenido
					return assetContent;
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine($"Error when load {asset}. {exception.Message}");
			}
		// Si ha llegado hasta aquí es porque no ha encontrado nada
		return null;
    }

    /// <summary>
    ///     Descarga el contenido
    /// </summary>
	private void UnloadContent()
	{
        // Descarga los assets cargados
        foreach (KeyValuePair<string, object?> asset in _loadedAssets)
            GameEngine.Instance.MonogameServicesManager.Content.UnloadAsset(asset.Key);
        // Limpia el caché de assets
        _loadedAssets.Clear();
	}

	/// <summary>
	///		Libera la memoria
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if (!Disposed)
		{
			// Libera el contenido
			if (disposing)
				UnloadContent();
			// Indica que se ha liberado la memoria
			Disposed = true;
		}
	}

	/// <summary>
	///		Libera la memoria
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	///		Manager de recursos
	/// </summary>
	public ResourcesManager ResourcesManager { get; } = resourcesManager;

	/// <summary>
	///		Indica si se ha liberado la memoria
	/// </summary>
	public bool Disposed { get; private set; }
}