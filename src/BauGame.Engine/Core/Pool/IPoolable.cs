namespace Bau.Libraries.BauGame.Engine.Core.Pool;

/// <summary>
///     Interface para los elementos que se pueden introducir en un pool
/// </summary>
public interface IPoolable
{
    /// <summary>
    ///     Indica si el elemento está activo
    /// </summary>
    bool Enabled { get; }
}
