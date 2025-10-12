using Bau.Monogame.Engine.Domain.Core.Models;
using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Components.Transforms;

/// <summary>
///		Componente de transformación
/// </summary>
public class TransformComponent(AbstractActor owner)
{
	/// <summary>
	///		Clona los datos del componente
	/// </summary>
	public TransformComponent Clone()
	{
		return new TransformComponent(Owner)
						{
							WorldBounds = WorldBounds.Clone(),
							Rotation = Rotation
						};
	}

    /// <summary>
    ///     Sobrecarga el operador ==
    /// </summary>
    public static bool operator ==(TransformComponent? left, TransformComponent? right)
    {
        if (ReferenceEquals(left, right)) 
            return true;
        else if (left is null || right is null) 
            return false;
        else
            return left.WorldBounds == right.WorldBounds && left.Rotation == right.Rotation;
    }

    /// <summary>
    ///     Sobrecarga el operador !=
    /// </summary>
    public static bool operator !=(TransformComponent? left, TransformComponent? right) => !(left == right);

    /// <summary>
    ///     Sobrescribe el método Equals(object)
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null) 
            return false;
        else if (ReferenceEquals(this, obj)) 
            return true;
        else if (obj.GetType() != GetType()) 
            return false;
        else if (obj is TransformComponent transform)
            return this == transform;
        else
            return false;
    }

    /// <summary>
    ///     Sobrescribe el método GetHashCode
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(WorldBounds.X, WorldBounds.Y, WorldBounds.Width, WorldBounds.Height, Rotation);

    /// <summary>
    ///     Sobrescribe el método ToString
    /// </summary>
    public override string ToString() => $"{nameof(TransformComponent)} (WorldBouds = {WorldBounds}, Rotation = {Rotation})";

	/// <summary>
	///		Actor al que se asocia la transformación
	/// </summary>
	public AbstractActor Owner { get; } = owner;

	/// <summary>
	///		Rectángulo con la posición en el mundo
	/// </summary>
	public RectangleF WorldBounds { get; set; } = new(0, 0, 0, 0);

	/// <summary>
	///		Centro del elemento
	/// </summary>
	public Vector2 Center => new(0.5f * WorldBounds.Width, 0.5f * WorldBounds.Height);

	/// <summary>
	///		Rotación
	/// </summary>
	public float Rotation { get; set; }

	/// <summary>
	///		Indica si la transformación es correcta
	/// </summary>
	public bool IsValid => WorldBounds.Width != 0 && WorldBounds.Height != 0;
}