using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.BauEngine.Configuration;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Repositorio de carga de configuración
/// </summary>
internal class SettingsRepository
{
	// Constantes privadas
	private const string TagRoot = "Settings";
	private const string TagWindowTitle = "WindowTitle";
	private const string TagEngine = "Engine";
	private const string TagFont = "Font";
	private const string TagScreen = "Screen";
	private const string TagMouseVisible = "MouseVisible";
	private const string TagFullScreen = "FullScreen";
	private const string TagBorderless = "Borderless";
	private const string TagMultiSampling = "MultiSampling";
	private const string TagSynchronizeWithVerticalRetrace = "SynchronizeWithVerticalRetrace";
	private const string TagBufferWidth = "BufferWidth";
	private const string TagBufferHeight = "BufferHeight";
	private const string TagViewPortWidth = "ViewPortWidth";
	private const string TagViewPortHeight = "ViewPortHeight";
	private const string TagAllowUserResizing = "AllowUserResizing";
	private const string TagOrientation = "Orientation";
	private const string TagProfile = "Profile";
	private const string TagDebug = "Debug";
	private const string TagDebugging = "Debugging";
	private const string TagColor = "Color";
	private const string TagOverlayColor = "OverlayColor";
	private const string TagImageColor = "ImageColor";
	private const string TagAudio = "Audio";
	private const string TagMasterVolume = "MasterVolume";
	private const string TagMusicVolume = "MusicVolume";
	private const string TagSoundVolume = "SoundVolume";
	private const string TagVoiceVolume = "VoiceVolume";
	private const string TagAmbienceVolume = "AmbienceVolume";
	// Variables privadas
	private RepositoryXmlHelper _helper = new();

	/// <summary>
	///		Carga la configuración de un texto XML
	/// </summary>
	internal void Load(string xml, EngineSettings settings)
	{
		MLFile fileML = new Libraries.LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
					{
						// Asigna el título
						settings.WindowTitle = GetDefaultString(rootML.Attributes[TagWindowTitle].Value.TrimIgnoreNull(), settings.WindowTitle);
						// Carga los nodos
						foreach (MLNode nodeML in rootML.Nodes)
							switch (nodeML.Name)
							{
								case TagEngine:
										LoadEngineSettings(settings, nodeML);
									break;
								case TagScreen:
										LoadScreenSettings(settings, nodeML);
									break;
								case TagDebug:
										LoadDebugSettings(settings, nodeML);
									break;
								case TagAudio:
										LoadAudioSettings(settings, nodeML);
									break;
							}
					}
	}

	/// <summary>
	///		Carga la configuración general del motor
	/// </summary>
	private void LoadEngineSettings(EngineSettings settings, MLNode rootML)
	{
		settings.DefaultFont = GetDefaultString(rootML.Attributes[TagFont].Value.TrimIgnoreNull(), settings.DefaultFont);
	}

	/// <summary>
	///		Carga la configuración de pantalla
	/// </summary>
	private void LoadScreenSettings(EngineSettings settings, MLNode rootML)
	{
		settings.ScreenSettings.IsMouseVisible = rootML.Attributes[TagMouseVisible].Value.GetBool(settings.ScreenSettings.IsMouseVisible);
		settings.ScreenSettings.FullScreen = rootML.Attributes[TagFullScreen].Value.GetBool(settings.ScreenSettings.FullScreen);
		settings.ScreenSettings.Borderless = rootML.Attributes[TagBorderless].Value.GetBool(settings.ScreenSettings.Borderless);
		settings.ScreenSettings.PreferMultiSampling = rootML.Attributes[TagMultiSampling].Value.GetBool(settings.ScreenSettings.PreferMultiSampling);
		settings.ScreenSettings.SynchronizeWithVerticalRetrace = rootML.Attributes[TagSynchronizeWithVerticalRetrace].Value.GetBool(settings.ScreenSettings.SynchronizeWithVerticalRetrace);
		settings.ScreenSettings.ScreenBufferWidth = rootML.Attributes[TagBufferWidth].Value.GetInt(settings.ScreenSettings.ScreenBufferWidth);
		settings.ScreenSettings.ScreenBufferHeight = rootML.Attributes[TagBufferHeight].Value.GetInt(settings.ScreenSettings.ScreenBufferHeight);
		settings.ScreenSettings.ViewPortWidth = rootML.Attributes[TagViewPortWidth].Value.GetInt(settings.ScreenSettings.ViewPortWidth);
		settings.ScreenSettings.ViewPortHeight = rootML.Attributes[TagViewPortHeight].Value.GetInt(settings.ScreenSettings.ViewPortHeight);
		settings.ScreenSettings.WindowAllowUserResizing = rootML.Attributes[TagAllowUserResizing].Value.GetBool(settings.ScreenSettings.WindowAllowUserResizing);
		settings.ScreenSettings.DisplayOrientation = rootML.Attributes[TagOrientation].Value.GetEnum(ScreenSettings.DeviceOrientation.Default);
		settings.ScreenSettings.Profile = rootML.Attributes[TagProfile].Value.GetEnum(ScreenSettings.GraphicsProfile.LowDefinition);
	}

	/// <summary>
	///		Carga la configuración de depuración
	/// </summary>
	private void LoadDebugSettings(EngineSettings settings, MLNode rootML)
	{
		settings.DebugSettings.IsDebugging = rootML.Attributes[TagDebugging].Value.GetBool(settings.DebugSettings.IsDebugging);
		settings.DebugSettings.Font = GetDefaultString(rootML.Attributes[TagFont].Value.TrimIgnoreNull(), settings.DebugSettings.Font ?? "fonts/hud");
		settings.DebugSettings.Color = _helper.GetColor(rootML.Attributes[TagColor].Value, settings.DebugSettings.Color);
		settings.DebugSettings.OverlayColor = _helper.GetColor(rootML.Attributes[TagOverlayColor].Value, settings.DebugSettings.OverlayColor);
		settings.DebugSettings.ImageColor = _helper.GetColor(rootML.Attributes[TagImageColor].Value, settings.DebugSettings.ImageColor);
	}

	/// <summary>
	///		Carga la configuración de audio
	/// </summary>
	private void LoadAudioSettings(EngineSettings settings, MLNode rootML)
	{
		settings.AudioSettings.MasterVolume = (float) rootML.Attributes[TagMasterVolume].Value.GetDouble(settings.AudioSettings.MasterVolume);
		settings.AudioSettings.MusicVolume = (float) rootML.Attributes[TagMusicVolume].Value.GetDouble(settings.AudioSettings.MusicVolume);
		settings.AudioSettings.SoundVolume = (float) rootML.Attributes[TagSoundVolume].Value.GetDouble(settings.AudioSettings.SoundVolume);
		settings.AudioSettings.VoiceVolume = (float) rootML.Attributes[TagVoiceVolume].Value.GetDouble(settings.AudioSettings.VoiceVolume);
		settings.AudioSettings.AmbienceVolume = (float) rootML.Attributes[TagAmbienceVolume].Value.GetDouble(settings.AudioSettings.AmbienceVolume);
	}

	/// <summary>
	///		Obtiene una cadena predeterminada si está vacía
	/// </summary>
	private string GetDefaultString(string value, string defaultValue)
	{
		if (!string.IsNullOrWhiteSpace(value))
			return value;
		else
			return defaultValue;
	}
}