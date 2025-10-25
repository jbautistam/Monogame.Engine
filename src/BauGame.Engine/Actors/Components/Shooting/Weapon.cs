using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Shooting;

/// <summary>
///     Clase con los datos de un arma
/// </summary>
public class Weapon(string name)
{
    // Registros públicos
    public record ShootProperties(Projectiles.ProjectileProperties Projectile, Vector2 Position, float Rotation);
    // Variables privadas    
    private float _reloadTimer, _timeSinceLastShot;

    /// <summary>
    ///     Actualiza los datos del arma
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        if (IsReloading)
        {
            // Incrementa el temporizador de recarga
            _reloadTimer += gameContext.DeltaTime;
            // Detiene la recarga
            if (_reloadTimer >= ReloadTime)
            {
                // Asigna la munición actual
                if (Magazines > 0)
                {
                    // Asigna el tamaño de la munición
                    CurrentAmmo = MagazineSize;
                    // Decrementa el número de cargador
                    Magazines--;
                }
                // Inicializa los temporizadores
                _reloadTimer = 0;
                _timeSinceLastShot = 0;
                // Indica que ya no está recargando
                IsReloading = false;
            }
        }
        else
            _timeSinceLastShot += gameContext.DeltaTime;
    }

    /// <summary>
    ///     Obtiene la lista de proyectiles a disparar
    /// </summary>
    public List<ShootProperties> Shoot(Vector2 shooterPosition, float shooterRotation)
    {
        List<ShootProperties> projectiles = [];

            // Verificar si puede disparar
            if (CanShoot())
            {
			    List<Vector2> shootPositions = GetShootPositions(shooterPosition, shooterRotation);
			    List<float> shootRotations = GetShootRotations(shooterRotation);
        
                    // Crea los proyectiles para cada combinación
                    for (int shootIndex = 0; shootIndex < shootPositions.Count; shootIndex++)
                        for (int rotationIndex = 0; rotationIndex < shootRotations.Count; rotationIndex++)
                            if (CurrentAmmo > 0) // ... si queda munición
                            {
                                // Añade un proyectil al disparo
                                projectiles.Add(new ShootProperties(ProjectileProperties, shootPositions[shootIndex], shootRotations[rotationIndex]));
                                // Consume un proyectil de la munición
                                ConsumeAmmo();
                            }
            }
            // Reiniciar temporizador
            _timeSinceLastShot = 0f;
            // Devuelve los proyectiles
            return projectiles;
    }

    /// <summary>
    ///     Indica si puede disparar
    /// </summary>
    public bool CanShoot()
    {
        // Comprueba si puede disparar
        if (!IsReloading && CurrentAmmo > 0)
        {
            float fireInterval = 1f / FireRate;

                // Puede disparar si se ha superado el tiempo desde el último disparo
                return _timeSinceLastShot >= fireInterval;
        }
        // Si ha llegado hasta aquí es porque no puede disparar
        return false;
    }

    /// <summary>
    ///     Recarga del arma
    /// </summary>
    public void Reload()
    {
        if (!IsReloading && CurrentAmmo < MagazineSize)
        {
            IsReloading = true;
            _reloadTimer = 0f;
        }
    }

    /// <summary>
    ///     Obtiene la lista de posiciones de disparo (cañones)
    /// </summary>
    private List<Vector2> GetShootPositions(Vector2 shooterPosition, float shooterRotation)
    {
        List<Vector2> positions = [];

            // Obtiene los offset de disparo        
            foreach (Vector2 offset in ShootOffsets)
            {
                Vector2 rotatedOffset = Vector2.Transform(offset, Matrix.CreateRotationZ(shooterRotation));

                    // Rota el desplazamiento según la rotación del tirador
                    positions.Add(shooterPosition + rotatedOffset);
            }
            // Devuelve las posiciones
            return positions;
    }

    /// <summary>
    ///     Obtiene las rotaciones de los disparos (spread)
    /// </summary>
    private List<float> GetShootRotations(float baseRotation)
    {
        List<float> rotations = [];

            // Si vamos a disparar más de un proyectil        
            if (ProjectilesPerShot == 1)
                rotations.Add(baseRotation);
            else
            {
                float totalSpread = MathHelper.ToRadians(Spread);
                float spreadPerProjectile = totalSpread / Math.Max(1, ProjectilesPerShot - 1);
            
                    // Distribuye los proyectiles con dispersión
                    for (int index = 0; index < ProjectilesPerShot; index++)
                    {
                        float rotationOffset = -(totalSpread / 2f) + (spreadPerProjectile * index);

                            // Añade la rotación
                            rotations.Add(baseRotation + rotationOffset);
                    }
            }
            // Devuelve la lista de rotaciones
            return rotations;
    }

    /// <summary>
    ///     Consume la munición
    /// </summary>
    private void ConsumeAmmo()
    {
        if (CurrentAmmo > 0)
            CurrentAmmo--;
    }

    /// <summary>
    ///     Puntos de disparo
    /// </summary>
    public List<Vector2> ShootOffsets { get; set; } = [];

    /// <summary>
    ///     Propiedades del proyectil
    /// </summary>
    public required Projectiles.ProjectileProperties ProjectileProperties { get; init; }

    /// <summary>
    ///     Nombre del arma
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    ///     Disparos por segundo
    /// </summary>
    public float FireRate { get; set; } = 2f;

    /// <summary>
    ///     Dispersión en grados
    /// </summary>
    public float Spread { get; set; } 

     /// <summary>
     ///    Número de balas por disparo
     /// </summary>
    public int ProjectilesPerShot { get; set; } = 1;

    /// <summary>
    ///     Disparo automático o semiautomático
    /// </summary>
    public bool IsAutomatic { get; set; } 
    
    /// <summary>
    ///     Munición actual
    /// </summary>
    public int CurrentAmmo { get; set; } = 100;

    /// <summary>
    ///     Número de cargadores
    /// </summary>
    public int Magazines { get; set; } = 1;

    /// <summary>
    ///     Número de balas del cargador
    /// </summary>
    public int MagazineSize { get; set; } = 30;

    /// <summary>
    ///     Tiempo de recarga del arma
    /// </summary>
    public float ReloadTime { get; set; } = 2f;

    /// <summary>
    ///     Indica si el arma está cargando
    /// </summary>
    public bool IsReloading { get; private set; }
}