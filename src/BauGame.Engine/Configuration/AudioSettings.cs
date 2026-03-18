namespace Bau.BauEngine.Configuration;

/// <summary>
///     Configuración de audio
/// </summary>
public class AudioSettings
{
    /// <summary>
    ///     Obtiene el volumen de una categoría
    /// </summary>
    public float CalculateVolume(Managers.Audio.AudioManager.AudioDefinitionType category, float baseVolume)
    {
        float volume = category switch
                                    {
                                        Managers.Audio.AudioManager.AudioDefinitionType.Music => MusicVolume,
                                        Managers.Audio.AudioManager.AudioDefinitionType.Sfx => SoundVolume,
                                        Managers.Audio.AudioManager.AudioDefinitionType.Voice => VoiceVolume,
                                        Managers.Audio.AudioManager.AudioDefinitionType.Ambience => AmbienceVolume,
                                        _ => 1.0f
                                    };
        
            // Obtiene el volumen de la categoría
            if (MasterVolume <= 0f || volume <= 0f || baseVolume <= 0f)
                return 0f;
            else
                return MasterVolume * volume * baseVolume;
    }

    /// <summary>
    ///     Volumen principal (0 a 1)
    /// </summary>
    public float MasterVolume { get; set; } = 1.0f;

    /// <summary>
    ///     Volumen de la música (0 a 1)
    /// </summary>
    public float MusicVolume { get; set; } = 0.8f;

    /// <summary>
    ///     Volumen de los efectos (0 a 1)
    /// </summary>
    public float SoundVolume { get; set; } = 1.0f;

    /// <summary>
    ///     Volumen de las voces (0 a 1)
    /// </summary>
    public float VoiceVolume { get; set; } = 1.0f;

    /// <summary>
    ///     Volumen de la música ambiente (0 a 1)
    /// </summary>
    public float AmbienceVolume { get; set; } = 0.6f;
}