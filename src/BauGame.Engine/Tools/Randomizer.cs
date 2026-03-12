using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools;

/// <summary>
///		Funciones para generación de valores aleatorios
/// </summary>
public static class Randomizer
{
	/// <summary>
	///		Obtiene una dirección aleatoria
	/// </summary>
	public static Vector2 GetRandomDirection()
	{
        float angle = (float) (Random.NextDouble() * Math.PI * 2);

			// Crea un vector unitario con la dirección a partir del ángulo en radianes
			return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
	}

	/// <summary>
	///		Obtiene un vector aleatorio
	/// </summary>
	internal static Vector2 GetRandomVector(Vector2 min, Vector2 max)
	{
		return new Vector2(min.X + (float) (Random.NextDouble() * (max.X - min.X)), min.Y + (float) (Random.NextDouble() * (max.Y - min.Y)));
	}

	/// <summary>
	///		Obtiene una posición aleatoria sobre una posición
	/// </summary>
	public static Vector2 GetRandomOffset(Vector2 position, float radius) => new(position.X + GetRandom(0, radius), position.Y + GetRandom(0, radius));

	/// <summary>
	///		Obtiene un valor float entre dos valores
	/// </summary>
	public static float GetRandom(float minimum, float maximum) => (float) (Random.NextDouble() * (maximum - minimum) + minimum);

	/// <summary>
	///		Obtiene un valor aleatorio entero entre dos valores
	/// </summary>
	public static int GetRandom(int minimum, int maximum) => (int) (Random.NextDouble() * (maximum - minimum) + minimum);

	/// <summary>
	///		Obtiene un valor aleatorio entero para un enumerado
	/// </summary>
	public static TypeEnum GetRandom<TypeEnum>() where TypeEnum : struct
	{
        Array values = Enum.GetValues(typeof(TypeEnum));

			// Obtiene un valor del array aleatoriamente
			return (TypeEnum) values.GetValue(Random.Next(values.Length))!;
	}

	/// <summary>
	///		Obtiene un color aleatorio
	/// </summary>
	public static Color GetRandomColor() => new(Random.Next(256), Random.Next(256), Random.Next(256));

	/// <summary>
	///		Obtiene un color aleatorio
	/// </summary>
	public static Color GetRandomColor(int minimumRed, int maximumRed, int minimumGreen, int maximumGreen, 
									   int minimumBlue, int maximumBlue)
	{
		return new(Random.Next(minimumRed, maximumRed), 
				   Random.Next(minimumGreen, maximumGreen), 
				   Random.Next(minimumBlue, maximumBlue));
	}

	/// <summary>
	///		Obtiene un color aleatorio
	/// </summary>
	public static Color GetRandomColor(Color minimum, Color maximum) => GetRandomColor(minimum.R, maximum.R, minimum.G, maximum.G, minimum.B, maximum.B);

	/// <summary>
	///		Objeto para generación de valores aleatorios
	/// </summary>
	public static Random Random { get; } = new();
}