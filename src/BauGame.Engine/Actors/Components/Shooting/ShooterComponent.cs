using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Shooting;

/// <summary>
///     Componente para manejar armas
/// </summary>
public class ShooterComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
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
        CurrentWeapon?.Update(gameContext);
	}

    /// <summary>
    ///     Dispara
    /// </summary>
    public void Shoot()
    {
        if (Owner.Layer is Scenes.Layers.Games.AbstractGameLayer gameLayer)
            if (CurrentWeapon is not null && CurrentWeapon.CanShoot())
                foreach (Weapon.ShootProperties shoot in CurrentWeapon.Shoot(Owner.Transform.WorldBounds.TopLeft, Owner.Transform.Rotation))
                    gameLayer.ProjectileManager.Create(shoot.Projectile, shoot.Position, shoot.Rotation);
    }

    /// <summary>
    ///     Equipa un arma por su índice
    /// </summary>
	public void EquipWeapon(int index)
	{
		if (index >= 0 && index < Weapons.Count)
            EquipWeapon(Weapons[index]);
	}

    /// <summary>
    ///     Equipa el siguiente arma o el anterior
    /// </summary>
	public void EquipWeapon(bool next)
	{
        if (Weapons.Count > 0)
        {
            int index = 0;
            
                // Asigna el índice del arma actual
                if (CurrentWeapon is not null)
                    index = Weapons.IndexOf(CurrentWeapon);
                // Cambia el índice
                if (next)
                    index = (index + 1) % Weapons.Count;
                else
                {
                    index--;
                    if (index < 0)
                        index = Weapons.Count - 1;
                }
                // Equipa el arma
                EquipWeapon(index);
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
                EquipWeapon(weapon);
	}

    /// <summary>
    ///     Equipa un arma
    /// </summary>
    public void EquipWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
    }

    /// <summary>
    ///     Recarga el arma
    /// </summary>
    public void Reload()
    {
        CurrentWeapon?.Reload();
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
	///     Armas asociadas al shooter
	/// </summary>
	public List<Weapon> Weapons { get; } = [];

    /// <summary>
    ///     Arma equipada
    /// </summary>
    public Weapon? CurrentWeapon { get; set; }
}
