namespace Bau.Libraries.BauGame.Engine.Entities.Common.Collections;

/// <summary>
///		Interface que deben cumplir los elementos para añadirse a una <see cref="SecureList"/>
/// </summary>
public interface ISecureListItem
{
	/// <summary>
	///		Identificador
	/// </summary>
	string Id { get; }
}
