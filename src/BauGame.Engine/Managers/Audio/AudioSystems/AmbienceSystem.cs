using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.AudioSystems;

/// <summary>
///     Sistema para efectos ambientales
/// </summary>
public class AmbienceSystem(AudioEngine audioEngine)
{
    // Variables privadas
    private readonly List<AmbientLayer> _layers = [];
    private SoundEffectInstance _currentAmbience;
    private string _currentType;
    private float _targetVolume = 1f;
    private float _currentVolume = 1f;
    private bool _isFading;
    
    public string CurrentType => _currentType;
    
    public void Play(string ambienceType, float fadeDuration = 2f)
    {
        if (_currentType == ambienceType) return;
        
        float finalVolume = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Ambience, 1f);
        
        if (finalVolume <= 0f)
        {
            _currentType = ambienceType;
            return; // Silencio absoluto, solo registramos el tipo
        }
        
        var sfx = AudioEngine.LoadSFX($"ambience/{ambienceType}");
        
        if (_currentAmbience != null && fadeDuration > 0f)
        {
            // Crossfade manual
            FadeToNew(sfx, ambienceType, fadeDuration);
        }
        else
        {
            _currentAmbience?.Stop();
            _currentAmbience?.Dispose();
            
            _currentAmbience = sfx.CreateInstance();
            _currentAmbience.IsLooped = true;
            _currentAmbience.Volume = finalVolume;
            _currentAmbience.Play();
            
            _currentType = ambienceType;
            _currentVolume = 1f;
        }
    }
    
    public void Stop(float fadeDuration = 2f)
    {
        if (_currentAmbience == null) return;
        
        if (fadeDuration > 0f)
        {
            _targetVolume = 0f;
            _isFading = true;
            // El fade se procesa en Update
        }
        else
        {
            _currentAmbience.Stop();
            _currentAmbience.Dispose();
            _currentAmbience = null;
            _currentType = null;
        }
    }
    
    private void FadeToNew(SoundEffect newSfx, string type, float duration)
    {
        // Simplificación: fade out, luego fade in
        // Para true crossfade necesitarías dos instancias simultáneas
        
        _targetVolume = 0f;
        _isFading = true;
        
        // Programar cambio después del fade out
        AudioEngine.ScheduleAction(duration * 0.5f, () =>
        {
            _currentAmbience?.Stop();
            _currentAmbience?.Dispose();
            
            float finalVol = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Ambience, 1f);
            
            _currentAmbience = newSfx.CreateInstance();
            _currentAmbience.IsLooped = true;
            _currentAmbience.Volume = 0f;
            _currentAmbience.Play();
            
            _currentType = type;
            _currentVolume = 0f;
            _targetVolume = 1f;
        });
    }
    
    public void Update(float deltaTime, double totalTime)
    {
        // Procesar fades
        if (_isFading || Math.Abs(_currentVolume - _targetVolume) > 0.01f)
        {
            _currentVolume = MathHelper.Lerp(_currentVolume, _targetVolume, 2f * deltaTime);
            
            float final = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Ambience, _currentVolume);
            
            if (_currentAmbience != null)
            {
                _currentAmbience.Volume = final;
                
                // Detener si fade out completo
                if (_targetVolume <= 0f && _currentVolume < 0.01f)
                {
                    _currentAmbience.Stop();
                    _currentAmbience.Dispose();
                    _currentAmbience = null;
                    _currentType = null;
                    _isFading = false;
                }
            }
            
            if (Math.Abs(_currentVolume - _targetVolume) < 0.01f)
            {
                _currentVolume = _targetVolume;
                _isFading = false;
            }
        }
        
        // Actualizar capas de ambiente procedural
        foreach (AmbientLayer layer in _layers)
            layer.Update(deltaTime, totalTime);
    }
    
    public void OnVolumeChanged()
    {
        if (_currentAmbience != null)
        {
            float final = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Ambience, _currentVolume);
            _currentAmbience.Volume = final;
            
            if (final <= 0f)
            {
                _currentAmbience.Stop();
            }
        }
    }
    
    public void Dispose()
    {
        _currentAmbience?.Dispose();
        foreach (var layer in _layers) 
            layer.Dispose();
    }

    /// <summary>
    ///     Motor de audio
    /// </summary>
    public AudioEngine AudioEngine { get; } = audioEngine;
}
public class AmbienceSystem
{
    // Capas de ambiente activas
    private readonly List<AmbientLayer> _layers = new();
    
    // Ambiente base (loop continuo)
    private SoundEffectInstance _baseLoop;
    private string _baseLoopName;
    private float _baseVolume = 1f;
    
    // Control de fade
    private float _currentVolume = 1f;
    private float _targetVolume = 1f;
    private bool _isFading;
    
    public string CurrentType => _baseLoopName;
    
    // Reproducir ambiente base (loop principal)
    public void Play(string ambienceType, float fadeDuration = 2f)
    {
        if (_baseLoopName == ambienceType) return;
        
        float finalVolume = AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, 1f);
        
        // Silencio absoluto: solo guardar tipo, no cargar nada
        if (finalVolume <= 0f)
        {
            _baseLoopName = ambienceType;
            return;
        }
        
        // Cargar nuevo loop base
        var newLoop = LoadAmbienceLoop(ambienceType);
        
        if (_baseLoop != null && fadeDuration > 0f)
        {
            // Crossfade: iniciar fade out del actual, luego fade in del nuevo
            FadeToNewBase(newLoop, ambienceType, fadeDuration);
        }
        else
        {
            // Cambio inmediato
            StopBase();
            StartBase(newLoop, ambienceType, finalVolume);
        }
        
        // Cargar capas procedurales asociadas a este ambiente
        LoadLayersForAmbience(ambienceType);
    }
    
    public void Stop(float fadeDuration = 2f)
    {
        if (fadeDuration > 0f && _baseLoop != null)
        {
            _targetVolume = 0f;
            _isFading = true;
        }
        else
        {
            StopAll();
        }
    }
    
    private void LoadLayersForAmbience(string ambienceType)
    {
        // Limpiar capas anteriores
        foreach (var layer in _layers) layer.Dispose();
        _layers.Clear();
        
        // Cargar configuración según tipo
        switch (ambienceType)
        {
            case "cafe_morning":
                AddLayer(
                    variations: new[] { "ambience/cafe/voice_01", "ambience/cafe/voice_02", "ambience/cafe/voice_03" },
                    minInterval: 3f,
                    maxInterval: 8f,
                    baseVolume: 0.6f
                );
                AddLayer(
                    variations: new[] { "ambience/cafe/clink_01", "ambience/cafe/clink_02" },
                    minInterval: 10f,
                    maxInterval: 25f,
                    baseVolume: 0.4f
                );
                break;
                
            case "street_busy":
                AddLayer(
                    variations: new[] { "ambience/street/horn_01", "ambience/street/horn_02", "ambience/street/horn_03" },
                    minInterval: 5f,
                    maxInterval: 15f,
                    baseVolume: 0.5f
                );
                AddLayer(
                    variations: new[] { "ambience/street/brake_01", "ambience/street/brake_02" },
                    minInterval: 8f,
                    maxInterval: 20f,
                    baseVolume: 0.3f
                );
                break;
                
            case "rain_heavy":
                // Solo loop base, sin capas adicionales
                break;
                
            case "forest_day":
                AddLayer(
                    variations: new[] { "ambience/forest/bird_01", "ambience/forest/bird_02", "ambience/forest/bird_03", "ambience/forest/bird_04" },
                    minInterval: 2f,
                    maxInterval: 6f,
                    baseVolume: 0.7f
                );
                AddLayer(
                    variations: new[] { "ambience/forest/wind_gust_01", "ambience/forest/wind_gust_02" },
                    minInterval: 12f,
                    maxInterval: 30f,
                    baseVolume: 0.5f
                );
                break;
        }
    }
    
    private void AddLayer(string[] variations, float minInterval, float maxInterval, float baseVolume)
    {
        var layer = new AmbientLayer(variations, minInterval, maxInterval, baseVolume);
        layer.Load(VNGame.Content);
        _layers.Add(layer);
    }
    
    public void Update(float deltaTime, double totalTime)
    {
        // Actualizar fade del loop base
        if (_isFading || Math.Abs(_currentVolume - _targetVolume) > 0.01f)
        {
            _currentVolume = MathHelper.Lerp(_currentVolume, _targetVolume, 3f * deltaTime);
            
            float final = AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, _currentVolume);
            
            if (_baseLoop != null)
            {
                _baseLoop.Volume = final;
                
                if (_targetVolume <= 0f && _currentVolume < 0.01f)
                {
                    StopBase();
                    _isFading = false;
                }
            }
            
            // También aplicar a capas
            foreach (var layer in _layers)
            {
                layer.SetMasterVolume(_currentVolume);
            }
        }
        
        // Actualizar capas procedurales
        foreach (var layer in _layers)
        {
            layer.Update(deltaTime, totalTime);
        }
    }
    
    public void OnVolumeChanged()
    {
        float final = AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, _currentVolume);
        
        if (_baseLoop != null)
        {
            _baseLoop.Volume = final;
            
            if (final <= 0f)
            {
                _baseLoop.Stop();
            }
            else if (_baseLoop.State == SoundState.Stopped)
            {
                _baseLoop.Play();
            }
        }
        
        foreach (var layer in _layers)
        {
            layer.OnMasterVolumeChanged();
        }
    }
    
    // Métodos auxiliares
    private SoundEffectInstance LoadAmbienceLoop(string type)
    {
        var sfx = VNGame.Audio.LoadSFX($"ambience/{type}_loop");
        return sfx.CreateInstance();
    }
    
    private void StartBase(SoundEffectInstance loop, string name, float volume)
    {
        _baseLoop = loop;
        _baseLoopName = name;
        _baseLoop.IsLooped = true;
        _baseLoop.Volume = volume;
        _baseLoop.Play();
        _currentVolume = 1f;
        _targetVolume = 1f;
    }
    
    private void StopBase()
    {
        _baseLoop?.Stop();
        _baseLoop?.Dispose();
        _baseLoop = null;
        _baseLoopName = null;
    }
    
    private void StopAll()
    {
        StopBase();
        foreach (var layer in _layers) layer.Dispose();
        _layers.Clear();
        _isFading = false;
    }
    
    private void FadeToNewBase(SoundEffectInstance newLoop, string name, float duration)
    {
        _targetVolume = 0f;
        _isFading = true;
        
        // Programar inicio del nuevo loop a mitad de fade
        VNGame.Audio.ScheduleAction(duration * 0.5f, () =>
        {
            StopBase();
            float finalVol = AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, 1f);
            StartBase(newLoop, name, 0f);
            _currentVolume = 0f;
            _targetVolume = 1f;
            _isFading = true;
        });
    }
    
    public void Dispose()
    {
        StopAll();
    }
}

public class AmbienceSystem
{
    private AudioTransition _transition = new();
    
    // Loop base
    private IAudioSource _baseLoop;
    private string _baseName;
    
    // Capas procedurales
    private readonly List<AmbientLayer> _layers = new();
    private float _masterMultiplier = 1f;
    
    public string CurrentType => _baseName;
    
    public void Play(string ambienceType, TransitionType type = TransitionType.Fade, float duration = 2f)
    {
        if (_baseName == ambienceType) return;
        
        float finalVol = AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, 1f);
        if (finalVol <= 0f)
        {
            _baseName = ambienceType;
            LoadLayers(ambienceType); // Cargar aunque no suenen
            return;
        }
        
        // Crear nuevo loop
        var newLoop = CreateLoopSource(ambienceType);
        
        switch (type)
        {
            case TransitionType.Cut:
                ExecuteCut(newLoop, finalVol, ambienceType);
                break;
                
            case TransitionType.Fade:
                ExecuteFade(newLoop, duration, finalVol, ambienceType);
                break;
                
            case TransitionType.Crossfade:
                ExecuteCrossfade(newLoop, duration, ambienceType);
                break;
        }
    }
    
    private void ExecuteCut(IAudioSource newLoop, float volume, string name)
    {
        _transition.Stop();
        _baseLoop?.Stop();
        
        ClearLayers();
        
        newLoop.SetVolume(volume);
        newLoop.Play();
        
        _baseLoop = newLoop;
        _baseName = name;
        
        LoadLayers(name);
    }
    
    private void ExecuteFade(IAudioSource newLoop, float duration, float targetVol, string name)
    {
        if (_baseLoop == null)
        {
            ExecuteCut(newLoop, targetVol, name);
            return;
        }
        
        _transition.OnFadeOutComplete = () =>
        {
            _baseLoop.Stop();
            ClearLayers();
            
            newLoop.SetVolume(0f);
            newLoop.Play();
            
            _transition.StartFade(0f, targetVol, duration * 0.5f);
            _transition.OnFadeInComplete = () =>
            {
                _baseLoop = newLoop;
                _baseName = name;
                LoadLayers(name);
            };
        };
        
        _transition.StartFade(_masterMultiplier, 0f, duration * 0.5f);
    }
    
    private void ExecuteCrossfade(IAudioSource newLoop, float duration, string name)
    {
        if (_baseLoop == null)
        {
            ExecuteCut(newLoop, AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, 1f), name);
            return;
        }
        
        // Preparar nuevo loop
        newLoop.SetVolume(0f);
        newLoop.Play();
        
        _transition.StartCrossfade(_baseLoop, newLoop, duration);
        
        _transition.OnCrossfadeMidpoint = () =>
        {
            // A mitad de camino, cambiar capas
            ClearLayers();
            LoadLayers(name);
        };
        
        _transition.OnFadeInComplete = () =>
        {
            _baseLoop = newLoop;
            _baseName = name;
        };
    }
    
    public void Stop(TransitionType type = TransitionType.Fade, float duration = 2f)
    {
        if (_baseLoop == null) return;
        
        switch (type)
        {
            case TransitionType.Cut:
                _baseLoop.Stop();
                _transition.Stop();
                ClearLayers();
                _baseName = null;
                break;
                
            case TransitionType.Fade:
                _transition.StartFade(_masterMultiplier, 0f, duration);
                _transition.OnFadeInComplete = () =>
                {
                    _baseLoop.Stop();
                    _baseLoop = null;
                    ClearLayers();
                    _baseName = null;
                };
                break;
        }
    }
    
    public void Update(float deltaTime, double totalTime)
    {
        // Procesar transición
        if (_transition.IsActive)
        {
            var vol = _transition.Update(deltaTime, out _);
            _masterMultiplier = vol;
            
            // Aplicar a capas
            foreach (var layer in _layers)
                layer.SetMasterVolume(vol);
        }
        
        // Actualizar capas
        foreach (var layer in _layers)
            layer.Update(deltaTime, totalTime);
    }
    
    private IAudioSource CreateLoopSource(string name)
    {
        var sfx = VNGame.Audio.LoadSFX($"ambience/{name}_loop");
        var instance = sfx.CreateInstance();
        instance.IsLooped = true;
        return new SFXSource(instance);
    }
    
    private void LoadLayers(string ambienceType)
    {
        // Configuración de capas por tipo
        var configs = GetLayerConfigs(ambienceType);
        
        foreach (var config in configs)
        {
            var layer = new AmbientLayer(config.Variations, config.MinInterval, 
                config.MaxInterval, config.BaseVolume);
            layer.Load(VNGame.Content);
            layer.SetMasterVolume(_masterMultiplier);
            _layers.Add(layer);
        }
    }
    
    private List<LayerConfig> GetLayerConfigs(string type)
    {
        var configs = new List<LayerConfig>();
        
        switch (type)
        {
            case "cafe_morning":
                configs.Add(new LayerConfig
                {
                    Variations = new[] { "ambience/cafe/voice_01", "ambience/cafe/voice_02", "ambience/cafe/voice_03" },
                    MinInterval = 3f,
                    MaxInterval = 8f,
                    BaseVolume = 0.6f
                });
                configs.Add(new LayerConfig
                {
                    Variations = new[] { "ambience/cafe/clink_01", "ambience/cafe/clink_02" },
                    MinInterval = 10f,
                    MaxInterval = 25f,
                    BaseVolume = 0.4f
                });
                break;
                
            case "street_busy":
                configs.Add(new LayerConfig
                {
                    Variations = new[] { "ambience/street/horn_01", "ambience/street/horn_02", "ambience/street/horn_03" },
                    MinInterval = 5f,
                    MaxInterval = 15f,
                    BaseVolume = 0.5f
                });
                break;
                
            case "forest_day":
                configs.Add(new LayerConfig
                {
                    Variations = new[] { "ambience/forest/bird_01", "ambience/forest/bird_02", "ambience/forest/bird_03", "ambience/forest/bird_04" },
                    MinInterval = 2f,
                    MaxInterval = 6f,
                    BaseVolume = 0.7f
                });
                configs.Add(new LayerConfig
                {
                    Variations = new[] { "ambience/forest/wind_gust_01", "ambience/forest/wind_gust_02" },
                    MinInterval = 12f,
                    MaxInterval = 30f,
                    BaseVolume = 0.5f
                });
                break;
        }
        
        return configs;
    }
    
    private void ClearLayers()
    {
        foreach (var layer in _layers) layer.Dispose();
        _layers.Clear();
    }
    
    public void OnVolumeChanged()
    {
        float final = AudioConfig.CalculateFinalVolume(AudioCategory.Ambience, _masterMultiplier);
        
        if (_baseLoop != null && !_transition.IsActive)
        {
            _baseLoop.SetVolume(final > 0f ? 1f : 0f);
        }
        
        foreach (var layer in _layers)
            layer.OnMasterVolumeChanged();
    }
    
    private struct LayerConfig
    {
        public string[] Variations;
        public float MinInterval;
        public float MaxInterval;
        public float BaseVolume;
    }
}