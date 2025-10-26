namespace Bau.Libraries.BauGame.Engine.Actors.Components.Shooting;

/// <summary>
///     Clase con los datos de un slot de armas
/// </summary>
public class WeaponSlot(string name)
{
	// Variables privadas
	public int _actualWeapon;

    /// <summary>
    ///     Añade un arma a la lista
    /// </summary>
	public void Add(Weapon weapon)
	{
        if (!Weapons.Any(item => item.Name.Equals(weapon.Name, StringComparison.CurrentCultureIgnoreCase)))
            Weapons.Add(weapon);
	}

    /// <summary>
    ///     Añade una serie de armas a la lista
    /// </summary>
	public void AddRange(List<Weapon> weapons)
	{
        foreach (Weapon weapon in weapons)
            Add(weapon);
	}

    /// <summary>
    ///     Equipa un arma por su índice
    /// </summary>
	public void EquipWeapon(int index)
	{
		if (IsIndexValid(index))
            _actualWeapon = index;
	}

    /// <summary>
    ///     Equipa el siguiente arma o el anterior
    /// </summary>
	public void EquipWeapon(bool next)
	{
        if (Weapons.Count > 0)
        {
            if (!IsIndexValid(_actualWeapon))
                _actualWeapon = 0;
            else
            {
                // Cambia el índice
                if (next)
                    _actualWeapon = (_actualWeapon + 1) % Weapons.Count;
                else
                {
                    _actualWeapon--;
                    if (_actualWeapon < 0)
                        _actualWeapon = Weapons.Count - 1;
                }
            }
        }
	}

    /// <summary>
    ///     Equipa un arma por su nombre
    /// </summary>
	public void EquipWeapon(string name)
	{
        Weapon? weapon = Weapons.First(item => item.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            // Equipa el arma
            if (weapon is not null)
                EquipWeapon(Weapons.IndexOf(weapon));
	}

    /// <summary>
    ///     Comprueba si un índice es válido
    /// </summary>
    private bool IsIndexValid(int index) => index >= 0 && index < Weapons.Count;

	/// <summary>
	///		Nombre del slot
	/// </summary>
	public string Name { get; } = name;

    /// <summary>
    ///     Indica si el slot está activo
    /// </summary>
    public bool Enabled { get; set; } = true;

	/// <summary>
	///		Lista de armas
	/// </summary>
	public List<Weapon> Weapons { get; } = [];

	/// <summary>
	///		Arma seleccionada en el slot
	/// </summary>
	public Weapon? SelectedWeapon 
	{ 
		get
		{
			if (IsIndexValid(_actualWeapon))
				return Weapons[_actualWeapon];
			else
				return null;
		}
	}
}