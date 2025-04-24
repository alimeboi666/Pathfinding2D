using Hung.Core;
using Hung.Pooling;
using UnityEngine;

namespace Hung.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, ICommonPoolable
    {
        public enum SourceSpawnType
        {
            World,
            System
        }

        [field: SerializeField] public AudioSource source { get; private set; }

        public PoolPartition pooling { get; set; }
        public bool isNewPooling { get; set; }


        bool hasRegister;
        public void RegisterToSystem()
        {
            hasRegister = true;
            ToggleSound(Archetype.MasterSound.isSoundOn);
            Archetype.MasterSound.onSoundChanged += ToggleSound;
        }

        private void OnEnable()
        {
            if (hasRegister)
            {
                ToggleSound(Archetype.MasterSound.isSoundOn);
                Archetype.MasterSound.onSoundChanged += ToggleSound;
            }
        }

        private void OnDisable()
        {
            if (hasRegister)
            {
                Archetype.MasterSound.onSoundChanged -= ToggleSound;
            }
        }

        public void ToggleSound(bool isOn)
        {
            source.mute = !isOn;
        }

        public void ToggleOff()
        {
            gameObject.SetActive(false);
        }

        public void ToggleOn()
        {
            gameObject.SetActive(true);
        }

    }

}