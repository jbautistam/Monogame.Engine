using Bau.Libraries.BauGame.Engine.Core.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Components.Shooting;

/// <summary>
///     Componente para manejar armas
/// </summary>
public class ShooterComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
    /// <summary>
    ///     Actualiza las físicas (no hace nada, simplemente implementa la interface)
    /// </summary>
	public override void UpdatePhysics(GameTime gameTime)
	{
	}

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
	public override void Update(GameTime gameTime)
	{
        CurrentWeapon?.Update(gameTime);
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
	public override void Draw(Camera2D camera, GameTime gameTime)
	{
	}

    /// <summary>
    ///     Finaliza el componente (no hace nada, simplemente implementa la interface)
    /// </summary>
	public override void End()
	{
	}

    /// <summary>
    ///     Arma equipada
    /// </summary>
    public Weapon? CurrentWeapon { get; set; }
}
