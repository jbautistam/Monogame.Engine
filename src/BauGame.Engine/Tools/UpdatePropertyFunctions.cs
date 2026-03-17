using Bau.Libraries.BauGame.Engine.Entities.Common;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools;

/// <summary>
///		Funciones para modificación de propiedades
/// </summary>
public static class UpdatePropertyFunctions
{
	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="string"/>
	/// </summary>
	public static bool ChangeProperty(ref string? field, string? value)
	{
		if (!field.EqualsIgnoreNull(value, StringComparison.CurrentCultureIgnoreCase))
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="Rectangle"/>
	/// </summary>
	public static bool ChangeProperty(ref Rectangle field, Rectangle value)
	{
		if (field.X != value.X || field.Y != value.Y || field.Width != value.Width || field.Height != value.Height)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="Point"/>
	/// </summary>
	public static bool ChangeProperty(ref Point field, Point value)
	{
		if (field.X != value.X || field.Y != value.Y)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="Size"/>
	/// </summary>
	public static bool ChangeProperty(ref Size field, Size value)
	{
		if (field.Width != value.Width || field.Height != value.Height)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="Vector2"/>
	/// </summary>
	public static bool ChangeProperty(ref Vector2 field, Vector2 value)
	{
		if (field.X != value.X || field.Y != value.Y)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad
	/// </summary>
	public static bool ChangeProperty(ref RectangleF field, RectangleF value)
	{
		if (field.X != value.X || field.Y != value.Y || field.Width != value.Width || field.Height != value.Height)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="Color"/>
	/// </summary>
	public static bool ChangeProperty(ref Color field, Color value)
	{
		if (field.A != value.A || field.R != value.R || field.G != value.G || field.B != value.B)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}

	/// <summary>
	///		Modifica el valor de una propiedad de tipo <see cref="float"/>
	/// </summary>
	public static bool ChangeProperty(ref float field, float value)
	{
		if (field != value)
		{
			// Modifica el valor
			field = value;
			// Devuelve el valor que indica si se ha modificado la propiedad
			return true;
		}
		else
			return false;
	}
}
