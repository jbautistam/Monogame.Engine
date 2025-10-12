using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Components.Shooting;

/// <summary>
///		Generador de armas
/// </summary>
public class WeaponBuilder
{
    /// <summary>
    ///     Crea un arma
    /// </summary>
    public WeaponBuilder WithWeapon(string name, bool automatic, Projectiles.ProjectileProperties projectileProperties)
    {
        // Crea un arma con el proyectil definido
        Weapons.Add(new Weapon(name)
                         {
                            ProjectileProperties = projectileProperties,
                            IsAutomatic = automatic
                         }
                   );
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Añade un offset para el cañón
    /// </summary>
    public WeaponBuilder WithShootOffset(Vector2 shootOffset)
    {
        // Añade el cañón
        ActualWeapon.ShootOffsets.Add(shootOffset);
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Añade el ratio de disparo
    /// </summary>
    public WeaponBuilder WithFireRate(float fireRate)
    {
        // Asigna el ratio de disparo
        ActualWeapon.FireRate = fireRate;
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Añade los cargadores
    /// </summary>
    public WeaponBuilder WithMagazines(int magazines, int size)
    {
        // Asigna los cargadores
        ActualWeapon.Magazines = magazines;
        ActualWeapon.MagazineSize = size;
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Añade el tiempo de recarga
    /// </summary>
    public WeaponBuilder WithReloadTime(float reloadTime)
    {
        // Asigna el tiempo de recarga
        ActualWeapon.ReloadTime = reloadTime;
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Añade la dispersión
    /// </summary>
    public WeaponBuilder WithSpread(int projectilesPerShot, float spread)
    {
        // Asigna la dispersión
        ActualWeapon.ProjectilesPerShot = projectilesPerShot;
        ActualWeapon.Spread = spread;
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Crea una pistola
    /// </summary>
    public WeaponBuilder WithPistol(string texture, string region)
    {
        // Crea la pistola
        WithWeapon("Pistol", false, new Projectiles.ProjectileProperties
                                            {
                                                Texture = texture,
                                                Region = region,
                                                Speed = 500f,
                                                MaxDistance = 400f,
                                                Damage = 25
                                            }
                  );
        WithShootOffset(new Vector2(15, 0));
        WithFireRate(2f);
        WithMagazines(3, 12);
        WithReloadTime(1.5f);
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Crea un rifle
    /// </summary>
    public WeaponBuilder WithShotgun(string texture, string region)
    {
        // Crea el rifle
        WithWeapon("Shotgun", false, new Projectiles.ProjectileProperties
                                            {
                                                Texture = texture,
                                                Region = region,
                                                Speed = 400f,
                                                MaxDistance = 300f,
                                                Damage = 15
                                            }
                  );
        WithShootOffset(new Vector2(20, 0));
        WithFireRate(1f);
        WithMagazines(4, 60);
        WithReloadTime(1.5f);
        WithSpread(8, 30f);
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Crea una ametralladora con dos cañones
    /// </summary>
    public WeaponBuilder WithMachineGun(string texture, string region)
    {
        // Crea la ametralladora
        WithWeapon("Machine gun", true, new Projectiles.ProjectileProperties
                                            {
                                                Texture = texture,
                                                Region = region,
                                                Speed = 600f,
                                                MaxDistance = 600f,
                                                Damage = 12
                                            }
                  );
        WithShootOffset(new Vector2(25, -2));
        WithShootOffset(new Vector2(25, 2));
        WithFireRate(8f);
        WithMagazines(3, 100);
        WithReloadTime(1.5f);
        WithSpread(8, 30f);
        // Devuelve el generador
        return this;
    }

    /// <summary>
    ///     Genera la lista de armas
    /// </summary>
    public List<Weapon> Build() => Weapons;

    /// <summary>
    ///     Arma actual
    /// </summary>
    private Weapon ActualWeapon => Weapons[Weapons.Count - 1];

    /// <summary>
    ///     Lista de armas
    /// </summary>
    public List<Weapon> Weapons { get; } = [];
}
