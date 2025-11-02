using Bau.Libraries.BauGame.Engine.Models;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Transforms;

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
							Bounds = Bounds.Clone(),
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
            return left.Bounds == right.Bounds && left.Rotation == right.Rotation;
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
    public override int GetHashCode() => HashCode.Combine(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, Rotation);

    /// <summary>
    ///     Sobrescribe el método ToString
    /// </summary>
    public override string ToString() => $"{nameof(TransformComponent)} (WorldBouds = {Bounds}, Rotation = {Rotation})";

    /// <summary>
    ///     Limita la posición a las coordenadas de un rectángulo
    /// </summary>
	public void Clamp(Rectangle bounds)
	{
	    if (Bounds.X < bounds.X)
            Bounds.X = bounds.X;
        if (Bounds.Y < bounds.Y)
            Bounds.Y = bounds.Y;
        if (Bounds.X + Bounds.Width > bounds.Width)
            Bounds.X = bounds.Width - Bounds.Width;
        if (Bounds.Y + Bounds.Height > bounds.Height)
            Bounds.Y = bounds.Height - Bounds.Height;
	}

	/// <summary>
	///		Actor al que se asocia la transformación
	/// </summary>
	public AbstractActor Owner { get; } = owner;

	/// <summary>
	///		Rectángulo con la posición centrada en el mundo
	/// </summary>
	public RectangleF BoundsCentered => new(Bounds.X + 0.5f * Bounds.Width, Bounds.Y + 0.5f * Bounds.Height, Bounds.Width, Bounds.Height);

	/// <summary>
	///		Rectángulo con la posición en el mundo
	/// </summary>
	public RectangleF Bounds { get; set; } = new(0, 0, 0, 0);

	/// <summary>
	///		Centro del elemento
	/// </summary>
	public Vector2 Center => new(0.5f * Bounds.Width, 0.5f * Bounds.Height);

	/// <summary>
	///		Rotación
	/// </summary>
	public float Rotation { get; set; }

	/// <summary>
	///		Indica si la transformación es correcta
	/// </summary>
	public bool IsValid => Bounds.Width != 0 && Bounds.Height != 0;
}