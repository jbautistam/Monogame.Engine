using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.AudioSystems;

public class VoiceSystem(AudioEngine audioEngine)
{
    private SoundEffectInstance _currentVoice;
    private string _currentCharacter;
    private Action _onCompleteCallback;
    private double _startTime;
    private float _duration;
    private bool _isPlaying;
    
    // Lip sync (simulado - solo timing)
    private List<LipSyncKey> _lipSyncData;
    private int _currentLipKey;
    
    public bool IsPlaying => _isPlaying;
    public string CurrentCharacter => _currentCharacter;
    
    public void Play(string characterId, string lineId, string emotion, Action onComplete)
    {
        Stop(); // Detener voz anterior
        
        float finalVolume = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Voice, 1f);
        
        // Si silencio absoluto, ejecutar callback inmediatamente y retornar
        if (finalVolume <= 0f)
        {
            onComplete?.Invoke();
            return;
        }
        
        // Cargar audio de voz
        string path = $"voice/{characterId}/{lineId}_{emotion}";
        var sfx = AudioEngine.LoadSFX(path);
        
        _currentVoice = sfx.CreateInstance();
        _currentVoice.Volume = finalVolume;
        
        // Cargar lip sync data si existe
        _lipSyncData = LoadLipSyncData(path);
        _currentLipKey = 0;
        
        _currentCharacter = characterId;
        _onCompleteCallback = onComplete;
        _duration = (float)sfx.Duration.TotalSeconds;
        _startTime = AudioEngine.TotalGameTime;
        _isPlaying = true;
        
        _currentVoice.Play();
    }
    
    public void Stop()
    {
        if (_currentVoice != null)
        {
            _currentVoice.Stop();
            _currentVoice.Dispose();
            _currentVoice = null;
        }
        
        _isPlaying = false;
        _currentCharacter = null;
        _lipSyncData = null;
        // No ejecutar callback al forzar stop
        _onCompleteCallback = null;
    }
    
    public void Update(float deltaTime)
    {
        if (!_isPlaying || _currentVoice == null) return;
        
        // Verificar si terminó
        if (_currentVoice.State == SoundState.Stopped ||
            (AudioEngine.TotalGameTime - _startTime) >= _duration)
        {
            var callback = _onCompleteCallback;
            _isPlaying = false;
            _currentVoice = null;
            _currentCharacter = null;
            _lipSyncData = null;
            _onCompleteCallback = null;
            
            callback?.Invoke();
        }
        else
        {
            // Actualizar lip sync
            UpdateLipSync();
        }
    }
    
    private void UpdateLipSync()
    {
        if (_lipSyncData == null || _lipSyncData.Count == 0) return;
        
        double currentTime = AudioEngine.TotalGameTime - _startTime;
        
        // Avanzar keys
        while (_currentLipKey < _lipSyncData.Count && 
               _lipSyncData[_currentLipKey].Time <= currentTime)
        {
            var key = _lipSyncData[_currentLipKey];
            // Notificar al sistema de sprites para cambiar expresión de boca
            //VNGame.Scene.SetMouthShape(_currentCharacter, key.MouthShape);
            _currentLipKey++;
        }
    }
    
    public void OnVolumeChanged()
    {
        if (_currentVoice != null)
        {
            float final = AudioEngine.Configuration.CalculateVolume(AudioEngine.AudioDefinitionType.Voice, 1f);
            _currentVoice.Volume = final;
            
            // Si pasa a 0, detener
            if (final <= 0f)
            {
                Stop();
            }
        }
    }
    
    public void Dispose()
    {
        _currentVoice?.Dispose();
    }
    
    private List<LipSyncKey> LoadLipSyncData(string audioPath)
    {
        // Cargar archivo .lip o .json con timing de fonemas
        // Formato simple: tiempo + forma de boca (A, E, I, O, U, closed)
        try
        {
            return AudioEngine.Load<List<LipSyncKey>>(audioPath + "_lip");
        }
        catch
        {
            return null;
        }
    }
    
    private class LipSyncKey
    {
        public float Time;
        public string MouthShape;
    }

    /// <summary>
    ///     Motor de audio
    /// </summary>
    public AudioEngine AudioEngine { get; } = audioEngine;
}
