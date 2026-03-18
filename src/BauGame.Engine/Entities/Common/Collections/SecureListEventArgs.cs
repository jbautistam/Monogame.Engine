namespace Bau.BauEngine.Entities.Common.Collections;

/// <summary>
///		Argumentos de los eventos de <see cref="SecureList"/>
/// </summary>
public class SecureListEventArgs<TypeData>(TypeData item) : EventArgs where TypeData : ISecureListItem
{
	/// <summary>
	///		Elemento al que afecta el evento
	/// </summary>
	public TypeData Item { get; } = item;
}
