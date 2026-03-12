namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.ProgressBars;

/// <summary>
/// Configuración específica para modo Clip
/// </summary>
public class ClipSettings
{
    /// <summary>
    /// Alineación de la porción visible
    /// </summary>
    public UiProgressBarComplex.ClipAlignment Alignment { get; set; } = UiProgressBarComplex.ClipAlignment.Left;
        
    /// <summary>
    /// Si true, invierte la dirección del clip cuando la dirección de llenado es inversa
    /// </summary>
    public bool AutoFlipWithDirection { get; set; } = true;
        
    /// <summary>
    /// Offset adicional en píxeles para ajuste fino
    /// </summary>
    public int PixelOffset { get; set; } = 0;
        
    /// <summary>
    /// Si true, el clip comienza desde el final de la textura hacia el inicio,
    /// pero alineado según la dirección de llenado (no a la derecha)
    /// </summary>
    public bool InvertProgressDirection { get; set; } = false;
    