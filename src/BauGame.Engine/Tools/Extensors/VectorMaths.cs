using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.Extensors;

/// <summary>
///		Funciones de cálculo de vectores
/// </summary>
public static class VectorMaths
{
	/// <summary>
	///		Limita los componentes de un vector
	/// </summary>
	public static Vector2 ClampComponents(this Vector2 vector, float maximum)
	{
		return new Vector2(MathHelper.Clamp(vector.X, -maximum, maximum), MathHelper.Clamp(vector.Y, -maximum, maximum));
	}

	/// <summary>
	///		Limita la magnitud del vector
	/// </summary>
	public static Vector2 ClampLength(this Vector2 vector, float maxLength)
	{
		float length = vector.Length();

			// Normaliza el vector
			if (length > maxLength && length > 0f)
				return vector / length * maxLength;
			else
				return vector;
	}

    /// <summary>
    ///     Obtiene un vector con la dirección hacia un punto
    /// </summary>
    public static Vector2 DirectionTo(this Vector2 from, float x, float y) => DirectionTo(from, new Vector2(x, y));

    /// <summary>
    ///     Obtiene un vector con la dirección hacia un punto
    /// </summary>
    public static Vector2 DirectionTo(this Vector2 from, Vector2 to)
    {
        Vector2 direction = to - from;

            // Evitamos los errores cuando la longitud es menor o igual que 0
            if (direction.LengthSquared() > 0)
                direction.Normalize();
            else
                direction = Vector2.Zero;
            // Devuelve el vector de dirección
            return direction;
    }

    /// <summary>
    ///     Obtiene el ángulo hacia un punto
    /// </summary>
    public static float AngleTo(this Vector2 from, Vector2 to) => (to - from).Angle();

    /// <summary>
    ///     Obtiene el ángulo hacia un punto
    /// </summary>
    public static float LookAt(this Vector2 position, Vector2 target) => position.AngleTo(target);

    /// <summary>
    ///     Obtiene el ángulo de un vector
    /// </summary>
    public static float Angle(this Vector2 source) => MathF.Atan2(source.Y, source.X);

    /// <summary>
    ///     Obtiene el ángulo entre dos vectores
    /// </summary>
    public static float AngleBetween(this Vector2 source, Vector2 to)
    {
        float dot = Vector2.Dot(source, to);
        float length = source.Length() * to.Length();

            // Calcula el ángulo
            if (length <= 0) 
                return 0;
            else
                return MathF.Acos(MathHelper.Clamp(dot / length, -1f, 1f));
    }

    /// <summary>
    ///     Obtiene el ángulo entre dos vectores con signo
    /// </summary>
    public static float SignedAngleBetween(this Vector2 from, Vector2 to) => MathF.Atan2(from.X * to.Y - from.Y * to.X, Vector2.Dot(from, to));

    /// <summary>
    ///     Rota un vector
    /// </summary>
    public static Vector2 Rotate(this Vector2 source, float radians)
    {
        float cos = MathF.Cos(radians);
        float sin = MathF.Sin(radians);

            // Devuelve el vector rotado
            return new Vector2(source.X * cos - source.Y * sin, source.X * sin + source.Y * cos);
    }

    /// <summary>
    ///     Obtiene un vector perpendicular
    /// </summary>
    public static Vector2 Perpendicular(this Vector2 v) => new Vector2(-v.Y, v.X);

    /// <summary>
    ///     Obtiene un vector perpendicular por la izquierda
    /// </summary>
    public static Vector2 PerpendicularLeft(this Vector2 forward) => new Vector2(-forward.Y, forward.X);

    /// <summary>
    ///     Obtiene un vector perpendicular por la derecha
    /// </summary>
    public static Vector2 PerpendicularRight(this Vector2 forward) => new Vector2(forward.Y, -forward.X);

    /// <summary>
    ///     Normalización segura (comprueba que la longitud no sea cero antes de normalizarla)
    /// </summary>
    public static Vector2 NormalizeSafe(this Vector2 source)
    {
        float length = source.Length();

            // Noramliza el vector
            if (length > 0)
                return source / length;
            else
                return Vector2.Zero;
    }

    /// <summary>
    ///     Calcula la distancia entre dos puntos (evita la raíz cuadrada para que sea más rápido aunque menos exacta)
    /// </summary>
    public static float DistanceSquared(this Vector2 from, Vector2 to)
    {
        float dx = from.X - to.X, dy = from.Y - to.Y;

            // Devuelve la distancia calculada
            return dx * dx + dy * dy;
    }

    /// <summary>
    ///     Comprueba si un punto está en un círculo
    /// </summary>
    public static Vector2 PointOnCircle(this Vector2 center, float radius, float angleRadians)
    {
        return center + new Vector2(MathF.Cos(angleRadians), MathF.Sin(angleRadians)) * radius;
    }

    /// <summary>
    ///     Comprueba si un punto está en un círculo
    /// </summary>
    public static bool IsPointInCircle(this Vector2 point, Vector2 center, float radius)
    {
        return point.DistanceSquared(center) <= radius * radius;
    }

    /// <summary>
    ///     Obtiene el punto en el vector más cercano a una línea
    /// </summary>
    public static Vector2 ClosestPointOnLine(this Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 line = lineEnd - lineStart;
        float t = Vector2.Dot(point - lineStart, line) / line.LengthSquared();

            // Limita la distancia
            t = MathHelper.Clamp(t, 0f, 1f);
            // Devuelve el punto
            return lineStart + line * t;
    }

    /// <summary>
    ///     Obtiene la distancia de un punto a una línea
    /// </summary>
    public static float DistancePointToLineSegment(this Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        return Vector2.Distance(point, point.ClosestPointOnLine(lineStart, lineEnd));
    }

    /// <summary>
    ///     Obtiene el vector reflejado sobre otro
    /// </summary>
    public static Vector2 Reflect(this Vector2 vector, Vector2 normal)
    {
        float dot = Vector2.Dot(vector, normal);

            // Devuelve el vector
            return vector - 2f * dot * normal;
    }

    /// <summary>
    ///     Proyecta un vector sobre otro
    /// </summary>
    public static Vector2 Project(this Vector2 source, Vector2 target)
    {
        float length = target.LengthSquared();

            // Devuelve el vector
            if (length > 0)
                return target * (Vector2.Dot(source, target) / length);
            else
                return Vector2.Zero;
    }

    /// <summary>
    ///     Obtiene el vector contrario
    /// </summary>
    public static Vector2 Reject(this Vector2 source, Vector2 onto) => source - source.Project(onto);

    /// <summary>
    ///     Obtiene el producto escalar de dos vectores
    /// </summary>
    public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

    /// <summary>
    ///     Obtiene la rotación entre dos vectores
    /// </summary>
    public static float RotationDelta(this Vector2 from, Vector2 to) => from.SignedAngleBetween(to);

    /// <summary>
    ///     Cambia la magnitud de un vector
    /// </summary>
    public static Vector2 SetLength(this Vector2 source, float length)
    {
        if (source.LengthSquared() > 0)
        {
            // Normaliza el vector
            source.Normalize();
            // Devuelve el vector con la nueva longitud
            return source * length;
        }
        else
            return Vector2.Zero;
    }
}
