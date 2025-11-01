using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Shooting;

/// <summary>
///     Componente para manejar armas
/// </summary>
public class ShooterComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		// ... no hace nada
	}

    /// <summary>
    ///     Actualiza las físicas (no hace nada, simplemente implementa la interface)
    /// </summary>
	public override void UpdatePhysics(Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
        foreach (WeaponSlot slot in Slots.Items.Values)
            if (slot.Enabled)
                slot.SelectedWeapon?.Update(gameContext);
	}

    /// <summary>
    ///     Añade armas a un slot
    /// </summary>
	public void AddWeapons(string slot, List<Weapon> weapons)
	{
        WeaponSlot weaponSlot = new(slot);

            // Añade el slot al diccionario
		    Slots.Add(slot, weaponSlot);
            // Añade el arma al slot
            weaponSlot.AddRange(weapons);
	}

    /// <summary>
    ///     Dispara un proyectil teniendo en cuenta la posición y la rotación
    /// </summary>
	public void Shoot(string slot, Vector2 position, float rotation, int physicsLayer)
    {
        Shoot(slot, position, new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation)), rotation, physicsLayer);
    }

    /// <summary>
    ///     Dispara un proyectil teniendo en cuenta la posición y la dirección
    /// </summary>
	public void Shoot(string slot, Vector2 position, Vector2 direction, float rotation, int physicsLayer)
    {
        WeaponSlot? weaponSlot = Slots.Get(slot);

            // Dispara el arma seleccionada en el slot
            if (weaponSlot is not null && weaponSlot.Enabled && weaponSlot.SelectedWeapon is not null)
                CreateProjectile(weaponSlot.SelectedWeapon, position, direction, rotation, physicsLayer);
    }

    /// <summary>
    ///     Crea un proyectil
    /// </summary>
    private void CreateProjectile(Weapon weapon, Vector2 position, Vector2 direction, float rotation, int physicsLayer)
    {
        if (Owner.Layer is Scenes.Layers.Games.AbstractGameLayer gameLayer)
            if (weapon.CanShoot())
                foreach (Weapon.ShootProperties shoot in weapon.Shoot(position, rotation))
                    gameLayer.ProjectileManager.Create(shoot.Projectile, shoot.Position, direction, shoot.Rotation, physicsLayer);
    }

    /// <summary>
    ///     Equipa un arma por su índice
    /// </summary>
	public void EquipWeapon(string slot, int index)
	{
        WeaponSlot? weaponSlot = Slots.Get(slot);

            if (weaponSlot is not null)
                weaponSlot.EquipWeapon(index);
	}

    /// <summary>
    ///     Equipa el siguiente arma o el anterior
    /// </summary>
	public void EquipWeapon(string slot, bool next)
	{
        WeaponSlot? weaponSlot = Slots.Get(slot);

            if (weaponSlot is not null)
                weaponSlot.EquipWeapon(next);
	}

    /// <summary>
    ///     Equipa un arma por su nombre
    /// </summary>
	public void EquipWeapon(string slot, string weapon)
	{
        WeaponSlot? weaponSlot = Slots.Get(slot);

            if (weaponSlot is not null)
                weaponSlot.EquipWeapon(weapon);
	}

    /// <summary>
    ///     Recarga el arma
    /// </summary>
    public void Reload(string slot)
    {
        WeaponSlot? weaponSlot = Slots.Get(slot);

            if (weaponSlot is not null)
                weaponSlot.SelectedWeapon?.Reload();
    }

    /// <summary>
    ///     Dibuja el componente (no hace nada, simplemente implementa la interface)
    /// </summary>
	public override void Draw(Camera2D camera, Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Finaliza el componente (no hace nada, simplemente implementa la interface)
    /// </summary>
	public override void End()
	{
	}

	/// <summary>
	///     Grupos de armas
	/// </summary>
	public Base.DictionaryModel<WeaponSlot> Slots { get; } = new();
}
