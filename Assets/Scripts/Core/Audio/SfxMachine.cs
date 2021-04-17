using System;
using UnityEngine;

namespace Core.Audio
{
    [Serializable]
    public class SfxMachine
    {
        public SfxList label;
        public AudioClip clip;
        public bool oneShot;
        public Vector2 range;
        [Range(0,1)] public float volume = 0.6f;
        [Range(0,1)] public float pitch = 1f;
        public bool loop;
       
        private AudioSource _audioSource;
        private AudioSystem _system;
        private float _lastPitchMultiplier = 1f;
        
        public void Initialize(AudioSource inSrc, AudioSystem inSystem)
        {
            ClampValues();
            _system = inSystem;
            _audioSource = inSrc;
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
            if(oneShot) _audioSource.loop = loop;
        }

        public void ClampValues()
        {
            //- Clamp range
            if (clip == null)
            {
                range = Vector2.zero; 
                return;
            }
            range = new Vector2(Mathf.Clamp(range.x, 0, clip.length),
                Mathf.Clamp(range.y, 0, clip.length));
            if (range.x > range.y) range = new Vector2(range.y, range.y);
        }
        
        public void Tick()
        {
            if(!_audioSource.isPlaying || oneShot) return;
            if (_audioSource.time >= range.y)
            {
                _audioSource.Stop();
                if (loop) Play(_lastPitchMultiplier);
            }
        }
        
        public void Play(float pitchMultiplier)
        {
            _lastPitchMultiplier = pitchMultiplier;
            _audioSource.volume = volume * _system.masterVolume;
            _audioSource.pitch = pitch * _lastPitchMultiplier;
            
            if (oneShot)
            {
                _audioSource.time = 0f;
                _audioSource.Play();
                return;
            }
            _audioSource.time = range.x;
            _audioSource.Play();
        }

        public void Stop() => _audioSource.Stop();

        public void Pause() => _audioSource.Pause();

        public void Resume() => _audioSource.Play();
    }
}
