using Hung.Pooling;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.Unit
{
    [RequireComponent(typeof(UIProcessBar))]
    public class UIHealthBar : ObjectFollower<IUnit>, IResizable, ICommonPoolable
    {
        [SerializeField] protected UIProcessBar bar;
        [field: SerializeField] public float appearTime { get; set; }
        [ReadOnly][SerializeField] private float _appearTime = 0;

        public float size
        {
            get
            {
                return Model.transform.localScale.magnitude;
            }
            set
            {
                Model.transform.localScale = Vector3.one * value;
            }
        }

        private void Start()
        {
            VisibleCheck += TimeCheck;
        }

        private bool TimeCheck()
        {
            _appearTime -= Time.deltaTime;
            return _appearTime >= 0;
        }

        public float current
        {
            set
            {
                bar.InstantSet(value);
            }
        }

        protected override void OnFollow(IUnit unit)
        {
            _appearTime = appearTime;

            size = unit.visualSize;

            bar.InstantSet(unit.HP.current / unit.HP.max);
            IHP hpHolder = unit;
            hpHolder.OnStatEffected += OnHealthChanged;
        }

        public void SetFollow(Transform target, IHP unitHP, float size = 1)
        {
            _appearTime = appearTime;

            gameObject.SetActive(true);
            this.target = target;

            this.size = size;

            bar.InstantSet(unitHP.HP.current / unitHP.HP.max);
            unitHP.OnStatEffected += OnHealthChanged;
        }


        internal override void Unfollow(Action onComplete = null)
        {
            target = null;
            Pool.BackToPool(this);
        }

        internal void OnHealthChanged(float current, float max)
        {
            //Debug.Log(changedPercent); 
            bar.VisualChange(bar.percent - current/max);
            _appearTime = appearTime;
        }
    }

}