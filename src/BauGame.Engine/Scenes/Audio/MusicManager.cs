using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Bau.Libraries.BauGame.Engine.Scenes.Audio;

/// <summary>
///     Manager para música ambiental
/// </summary>
internal class MusicManager(AudioManager audioManager) : AbstractAudioManager(audioManager)
{
    // Variables privadas
    private Song? _currentSong;
    private Song? _nextSong;
    private float _targetVolume = 1.0f;
    private float _currentVolume = 0.0f;
    private float fadeSpeed = 2.0f; // unidades por segundo
    private bool _isFading = false;

    /// <summary>
    ///     Inicializa el manager de música
    /// </summary>
    internal void Initialize()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0f;
    }

    /// <summary>
    ///     Reproduce una canción
    /// </summary>
    internal void PlaySong(string name)
    {
        Song? song = GameEngine.Instance.ResourcesManager.GlobalContentManager.LoadAsset<Song>(name);

            // Comienza la reproducción de música
            if (song is not null && _currentSong != song)
            {
                // Si hay una canción activa, inicia un fade-out, si no, inicia la reproducción
                if (_currentSong is not null)
                {
                    _nextSong = song;
                    _isFading = true;
                }
                else
                {
                    // Guarda la canción actual
                    _currentSong = song;
                    _currentVolume = 0f;
                    _targetVolume = ActualVolume;
                    _isFading = true;
                    // Comienza la reproducción de la música
                    MediaPlayer.Volume = 0f;
                    MediaPlayer.Play(song);
                }
            }
    }

    /// <summary>
    ///     Detiene momentáneamente la reproducción de música
    /// </summary>
    internal void Pause()
    {
        if (IsPlaying)
            MediaPlayer.Pause();
    }

    /// <summary>
    ///     Continúa la reproducción de música
    /// </summary>
    internal void Resume()
    {
        if (IsPaused)
            MediaPlayer.Resume();
    }

    /// <summary>
    ///     Detiene la reproducción de música
    /// </summary>
    internal void Stop()
    {
        // Detiene la reproducción
        MediaPlayer.Stop();
        // Inicializa las variables
        _currentSong = null;
        _nextSong = null;
        _currentVolume = 0f;
        _isFading = false;
    }

    /// <summary>
    ///     Actualiza la música
    /// </summary>
    internal override void UpdateAudio(GameTime gameTime)
    {
        if (_isFading)
        {
            float delta = (float) gameTime.ElapsedGameTime.TotalSeconds * fadeSpeed;

                if (_currentVolume < _targetVolume)
                    _currentVolume = MathHelper.Min(_currentVolume + delta, _targetVolume);
                else if (_currentVolume > _targetVolume)
                    _currentVolume = MathHelper.Max(_currentVolume - delta, _targetVolume);

                MediaPlayer.Volume = _currentVolume;

                // Si estamos bajando el volumen para cambiar de canción
                if (_nextSong != null && _currentVolume <= 0.01f)
                {
                    _currentSong = _nextSong;
                    _nextSong = null;
                    MediaPlayer.Play(_currentSong);
                    _targetVolume = ActualVolume;
                    _currentVolume = 0f;
                    // Continuar fade-in
                }

                // Si el fade ha terminado
                if (MathHelper.Distance(_currentVolume, _targetVolume) < 0.01f)
                {
                    _currentVolume = _targetVolume;
                    MediaPlayer.Volume = _currentVolume;
                    _isFading = false;
                }
        }
    }

    /// <summary>
    ///     Volumen actual de la música
    /// </summary>
    internal float ActualVolume
    {
        get => _targetVolume;
        set
        {
            _targetVolume = MathHelper.Clamp(value, 0f, 1f);
            if (!_isFading)
                _currentVolume = _targetVolume;
        }
    }

    /// <summary>
    ///     Indica si se está reproduciendo alguna canción
    /// </summary>
    internal bool IsPlaying => _currentSong is not null && MediaPlayer.State == MediaState.Playing;

    /// <summary>
    ///     Indica si la reproducción de música está detenida
    /// </summary>
    internal bool IsPaused => _currentSong is not null && MediaPlayer.State == MediaState.Paused;
}