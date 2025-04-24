using System;
using System.Collections.Generic;
using Hung.Pooling;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Hung.Core.Extensions
{
    public class SoundMaster : SerializedMonoBehaviour, IMasterSound
    {

        [SerializeField] private AudioListener audioListener;

        [ReadOnly, SerializeField] bool _isMusicOn;
        [ReadOnly, SerializeField] bool _isSoundOn;
        [ReadOnly, SerializeField] bool _isVibration;
        public bool isMusicOn 
        {
            get => _isMusicOn;
            set
            {
                if (_isMusicOn != value)
                {
                    _isMusicOn = value;
                    Archetype.MasterSound.ChangeAllBackgrounds(!value);
                }             
            }
        }


        public bool isSoundOn 
        {
            get => _isSoundOn;
            set
            {
                if (value != _isSoundOn)
                {
                    _isSoundOn = value;
                    onSoundChanged?.Invoke(value);
                }              
            } 
        }

        public bool isVibrationOn
        {
            get
            {
                return _isVibration;
            }
            set
            {
                if (value != _isVibration)
                {
                    _isVibration = value;
                }
            }
        }

        [Button]
        public void ToggleSound(bool isOn)
        {
            isSoundOn = isOn;
            isMusicOn = isOn;
        }


        [SerializeField] private Dictionary<UISound, AudioClip> clips;
        [SerializeField] private Dictionary<int, AudioClip> levelClips = new();

        public void PlayUISound(AudioClip sound, bool isLoop)
        {
            if (!isSoundOn || sound == null) return;
            AudioSource.PlayClipAtPoint(sound, audioListener.transform.position);
        }

        public void PlayUISound(AudioClip sound)
        {
            if (!isSoundOn || sound == null) return;
            AudioSource.PlayClipAtPoint(sound, audioListener.transform.position);
        }

        public void PlayLevelSound(int level)
        {
            if (!isSoundOn) return;
            
            if (levelClips.TryGetValue(level, out var clip))
            {
                AudioSource.PlayClipAtPoint(clip, audioListener.transform.position);
            }

        }

        public void PlayUISound(UISound sound)
        {
            if (!isSoundOn || !clips.ContainsKey(sound)) return;
            AudioSource.PlayClipAtPoint(clips[sound], audioListener.transform.position);
        }

        public void Register(IPlaySound player)
        {

        }

        public void Remove(IPlaySound player)
        {

        }

        Dictionary<string, AudioPlayer> sourceDict = new();

        public void ChangeAllBackgrounds(bool isMute)
        {
            foreach(var source in sourceDict.Values)
            {
                source.source.mute = isMute;
            }
        }

        public void PlayBackground(string source, AudioClip sound)
        {
            if (sound == null) return;
            if (!sourceDict.ContainsKey(source))
            {
                CommonPool.Spawn(out AudioPlayer audioPlayer);
                audioPlayer.transform.SetParent(audioListener.transform, false);
                sourceDict[source] = audioPlayer;
            }
            var audioSource = sourceDict[source].source;
            audioSource.clip = sound;
            audioSource.loop = true;
            audioSource.mute = !isMusicOn;
            audioSource.Play();
        }

        public void StopBackground(string source)
        {
            if (sourceDict.ContainsKey(source)) 
            {
                sourceDict[source].BackToPool();
                sourceDict.Remove(source);
            }
        }

        public void ChangeBackground(string source, AudioClip sound)
        {
            if (sound == null) return;
            if (sourceDict.ContainsKey(source))
            {
                var audioSource = sourceDict[source].source;
                audioSource.clip = sound;
                audioSource.Play();
            }
            else PlayBackground(source, sound);
        }

        [Range(0.001f, 0.02f), SerializeField] private float distanceScale;

        public event Action<bool> onSoundChanged;
        public event Action<bool> onVibrationChanged;

        public void PlayWorldSound(AudioClip sound, Vector3 position)
        {
            if (!isSoundOn || sound == null) return;
            
            AudioSource.PlayClipAtPoint(sound, audioListener.transform.position + distanceScale * (position - audioListener.transform.position));
        }

        public void PlayVibration()
        {
            if (isVibrationOn)
            {
                try
                {
                    //Handheld.Vibrate();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Handheld.Vibrate error: {e}");
                }

                try
                {
                    //HapticFeedback.HeavyFeedback();
                }
                catch (Exception e)
                {
                    Debug.LogError($"HapticFeedback error: {e}");
                }

                try
                {
                    Vibration.Vibrate(100);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Vibration.Vibrate error: {e}");
                }
            }

        }
    }

}

