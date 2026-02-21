namespace ParticleEngine.Core;

public readonly struct Matrix2D
{
    public readonly float M11, M12, M21, M22, M31, M32;
    
    public Matrix2D(float m11, float m12, float m21, float m22, float m31, float m32)
    {
        M11 = m11; M12 = m12; M21 = m21; M22 = m22; M31 = m31; M32 = m32;
    }
    
    public static Matrix2D CreateTRS(Vector2 translation, float rotation, Vector2 scale)
    {
        float cos = MathF.Cos(rotation);
        float sin = MathF.Sin(rotation);
        return new Matrix2D(
            cos * scale.X, -sin * scale.X,
            sin * scale.Y, cos * scale.Y,
            translation.X, translation.Y);
    }
    
    public static Vector2 Transform(Vector2 v, in Matrix2D m) => new(
        v.X * m.M11 + v.Y * m.M21 + m.M31,
        v.X * m.M12 + v.Y * m.M22 + m.M32);
    
    public static Vector2 TransformNormal(Vector2 v, in Matrix2D m) => new(
        v.X * m.M11 + v.Y * m.M21,
        v.X * m.M12 + v.Y * m.M22);
}
