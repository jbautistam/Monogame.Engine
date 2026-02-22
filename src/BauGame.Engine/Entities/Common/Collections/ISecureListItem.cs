namespace Bau.Libraries.BauGame.Engine.Entities.Common.Collections;

/// <summary>
///		Interface que deben cumplir los elementos para añadirse a una <see cref="SecureList"/>
/// </summary>
public interface ISecureListItem
{
	/// <summary>
	///		Arranca el elemento cuando se añade a la lista
	/// </summary>
	void Start();

	/// <summary>
	///		Finaliza el elemento cuando se borra de la lista
	/// </summary>
	void End(Managers.GameContext gameContext);

	/// <summary>
	///		Identificador
	/// </summary>
	string Id { get; }
}
