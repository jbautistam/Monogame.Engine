using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.AudioSystems;

public class SfxSystem(AudioManager audioManager) : AbstractAudioSystem(audioManager)
{
    private readonly List<SFXInstance> _activeSFX = new();
    private readonly Queue<SoundEffectInstance> _instancePool = new();
    private readonly List<ScheduledSFX> _scheduled = new();
    
    public void Play(string soundName, float volume = 1f, float pitch = 0f, 
        float pan = 0f, float delay = 0f)
    {
        float finalVolume = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Sfx, volume);
        
        // Silencio absoluto
        if (finalVolume <= 0f) return;
        
        if (delay > 0f)
        {
            _scheduled.Add(new ScheduledSFX
            {
                TriggerTime = AudioEngine.TotalGameTime + delay,
                SoundName = soundName,
                Volume = volume,
                Pitch = pitch,
                Pan = pan
            });
            return;
        }
        
        PlayImmediate(soundName, finalVolume, pitch, pan);
    }
    
    private void PlayImmediate(string soundName, float volume, float pitch, float pan)
    {
        var sfx = AudioEngine.LoadSFX(soundName);
        
        // Reutilizar instancias del pool si es posible
        SoundEffectInstance instance;
        if (_instancePool.Count > 0)
        {
            instance = _instancePool.Dequeue();
            // Resetear estado si es necesario
        }
        else
        {
            instance = sfx.CreateInstance();
        }
        
        instance.Volume = volume;
        instance.Pitch = pitch;
        instance.Pan = pan;
        
        instance.Play();
        
        _activeSFX.Add(new SFXInstance
        {
            Instance = instance,
            StartTime = AudioEngine.TotalGameTime,
            Duration = (float)sfx.Duration.TotalSeconds
        });
    }
    
    public void Update(float deltaTime)
    {
        double currentTime = AudioEngine.TotalGameTime;
        
        // Procesar scheduled
        for (int i = _scheduled.Count - 1; i >= 0; i--)
        {
            if (_scheduled[i].TriggerTime <= currentTime)
            {
                var s = _scheduled[i];
                float finalVol = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Sfx, s.Volume);
                if (finalVol > 0f)
                {
                    PlayImmediate(s.SoundName, finalVol, s.Pitch, s.Pan);
                }
                _scheduled.RemoveAt(i);
            }
        }
        
        // Limpiar terminados
        for (int i = _activeSFX.Count - 1; i >= 0; i--)
        {
            var sfx = _activeSFX[i];
            
            if (sfx.Instance.State == SoundState.Stopped ||
                (currentTime - sfx.StartTime) >= sfx.Duration)
            {
                sfx.Instance.Stop();
                _instancePool.Enqueue(sfx.Instance);
                _activeSFX.RemoveAt(i);
            }
        }
    }
    
    public void StopAll()
    {
        foreach (var sfx in _activeSFX)
        {
            sfx.Instance.Stop();
            _instancePool.Enqueue(sfx.Instance);
        }
        _activeSFX.Clear();
        _scheduled.Clear();
    }
    
    public void OnVolumeChanged()
    {
        // No podemos cambiar volumen de SFX ya reproducidos fácilmente
        // Solo afecta a futuros sonidos
    }
    
    public void Dispose()
    {
        StopAll();
        while (_instancePool.Count > 0)
        {
            _instancePool.Dequeue()?.Dispose();
        }
    }
    
    private class SFXInstance
    {
        public SoundEffectInstance Instance;
        public double StartTime;
        public float Duration;
    }
    
    private class ScheduledSFX
    {
        public double TriggerTime;
        public string SoundName;
        public float Volume;
        public float Pitch;
        public float Pan;
    }
    
    /// <summary>
    ///     Motor de audio
    /// </summary>
    public AudioEngine AudioEngine { get; } = audioEngine;
}