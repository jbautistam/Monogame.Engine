using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.Common;

/// <summary>
///		Clase con los datos de una transformación
/// </summary>
public class TransformModel
{
    // Variables privadas
    private Vector2 _position = Vector2.Zero, _scale = Vector2.One;
    private Vector2? _origin;
    private float _rotation;

    /// <summary>
    ///     Posición
    /// </summary>
    public Vector2 Position 
    { 
        get { return _position; }
        set
        {
            if (_position != value)
            {
                _position = value;
                IsDirty = true;
            }
        }
    }

    /// <summary>
    ///     Rotación
    /// </summary>
    public float Rotation 
    { 
        get { return _rotation; }
        set 
        {
            if (_rotation != value)
            {
                _rotation = value;
                IsDirty = true;
            }
        }
    }

    /// <summary>
    ///     Escala
    /// </summary>
    public Vector2 Scale
    { 
        get { return _scale; }
        set
        {
            if (_scale != value)
            {
                _scale = value;
                IsDirty = true;
            }
        }
    }

    /// <summary>
    ///     Origen
    /// </summary>
    public Vector2 Origin
    { 
        get { return _origin ?? Vector2.Zero; }
        set
        {
            if (_origin != value)
            {
                _origin = value;
                IsDirty = true;
            }
        }
    }

    /// <summary>
    ///     Indica si se han modificado los datos
    /// </summary>
    public bool IsDirty { get; set; }

    /// <summary>
    ///     Obtiene la cadena que describe la clase
    /// </summary>
	public override string ToString() => $"Position: {Position.ToString()}, Rotation: {Rotation:F2}, Scale: {Scale.ToString()}, Origin: {Origin.ToString()}";
}