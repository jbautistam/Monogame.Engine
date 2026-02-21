namespace UI.Configuration;

public class GraphicsConfiguration
{
    public int ResolutionWidth { get; set; } = 1920;
    public int ResolutionHeight { get; set; } = 1080;
    public bool FullScreen { get; set; } = false;
    public bool Borderless { get; set; } = true; // Si true, ignora FullScreen tradicional
    public bool VSync { get; set; } = true;
    public int TargetFps { get; set; } = 60;
    public bool MultiSampling { get; set; } = true;
    public float MasterVolume { get; set; } = 1.0f;
}
