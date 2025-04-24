using DamageNumbersPro;
using UnityEngine;

namespace Hung.Core.TextSystem
{
    public enum WorldTextType : byte
    {
        Damage,
        Heal,
        CriticalDamage,
        StackDamage
    }

    public class TextSpawner : Singleton<TextSpawner>
    {
        [SerializeField] private bool enable = true;
        [SerializeField] private DamageNumber[] origins;

        public bool Enable
        {
            get => enable;
            set => enable = value;
        }

        public static void ToggleSystem()
        {
            Instance.Enable = !Instance.Enable;
        }

        private void Start()
        {
            foreach (var item in origins)
            {
                item.PrewarmPool();
            }
        }

        // Spawn text at position with value
        public void Spawn(Vector3 position, float value, WorldTextType textType = WorldTextType.Damage)
        {
            if (!enable || value < 0.5f) return;

            int roundedValue = Mathf.RoundToInt(Mathf.Abs(value));
            origins[(byte)textType].Spawn(position, roundedValue);
        }

        // Spawn text at target with stackable value
        public void Stack(float stackValue, Transform target)
        {
            if (!enable) return;

            origins[(byte)WorldTextType.StackDamage].Spawn(target.position + new Vector3(1, 1), stackValue, target);
        }
    }
}
