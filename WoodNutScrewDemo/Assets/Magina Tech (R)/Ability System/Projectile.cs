using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Hung.Unit;
using Hung.Pooling;
using Hung.StatSystem;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Hung.AbilitySystem.Core
{
    public class Projectile : MonoBehaviour, ISkillshot, ICommonPoolable
    {
        public Vector3 positionValue => transform.position;
        [SerializeField] private Detector<IUnit> detector;
        [field: ReadOnly][field:SerializeField] public VisualEffect3D vfx { get; set; }
        [field: ReadOnly][field:SerializeField] public PoolPartition pooling { get; set; }
        public bool isNewPooling { get; set; }

        [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

        private Speed FIXED_SPD = new();
        private Area FIXED_AREA = new();
        private Speed _speed;
        private Area _area;
        private RPG_Effect _effect;
        private ProjectileProperty _property;

        void UpdateColliderSize(float size)
        {
            detector.SetCollider(size);
        }

        public void Setup(ProjectileProperty property, RPG_Effect effect, VisualEffect3D vfx, int colliderLayer, Area area, Speed speed)
        {     
            _speed = speed;
            if (_speed == null) _speed = FIXED_SPD;
            _area = area;
            if (_area == null) _area = FIXED_AREA;
            _area.OnValueChange += UpdateColliderSize;
            this.vfx = vfx;
            vfx.size = _area.current;
            Setup_Core(property, effect, _area.current, colliderLayer);
        }


        void Setup_Core(ProjectileProperty property, RPG_Effect effect, float size, int colliderLayer)
        {
            _effect = effect;
            _property = property;

            if (property.blockByUnit)
            {
                detector.ToggleOn();
                detector.OnTrigger = (IUnit unit) =>
                {
                    effect(unit);
                    //Debug.Log(unit.gameObject.name); 
                };
                detector.SetCollider(size, colliderLayer);
                
            }
            else
            {
                detector.ToggleOff();
            }
        }

        IEnumerator FollowingTarget(IUnit target)
        {
            //if (target == null)
            //{
            //    Dispose();
            //    yield break;
            //}
            
            var targetTransform = target.transform;
            while (true)
            {
                var delta = Time.deltaTime * _speed.current;
                if (Vector3.Distance(transform.position, targetTransform.position) <= delta) break;
                var dir = (targetTransform.position - transform.position).normalized;
                transform.position += delta * dir;
                transform.LookAtDirection3D(dir);
                yield return null;
                if (targetTransform == null)
                {
                    OnCompleteTween();
                    yield break;
                }
            }
            if (!_property.blockByUnit)
            {
                _effect(target);
            }
            OnCompleteTween();
        }

        public void InstantAffect(RPG_Effect effect)
        {
            throw new NotImplementedException();
        }

        public void Attach(IAbility caster, SpawnOffset offset)
        {
            transform.parent = caster.transform;
            transform.localPosition = offset.Get3DPostionOffset();
            transform.localEulerAngles = Vector3.up * offset.angle;

            if (offset.followRotation)
            {
                var follow = gameObject.AddComponent<RotationFollower>();
                var target = caster.unit.Model.transform;
                follow.Offset = Vector3.up * offset.angle;
                follow.target = target;
            }
        }


        public ISkillshot DOAttack(IPosition caster, ISkillTargetType targetType = null)
        {
            throw new System.NotImplementedException();
        }

        public ISkillshot DOFollow(IUnit target)
        {
            StartCoroutine(FollowingTarget(target));
            return this;
        }

        public ISkillshot DOMoveDirection(Vector3 direction)
        {
            transform.DOKill();
            _dir = direction * 50;
            transform.LookAtDirection3D(_dir);
            transform.DOMove(transform.position + _dir, 50 / _speed.current).SetEase(Ease.Linear).OnComplete(OnCompleteTween);
            return this;
        }

        Vector3 _dir;
        public ISkillshot DOMoveLocation(Vector3 destination)
        {
            transform.DOKill();
            _dir = destination - transform.position;
            transform.LookAtDirection3D(_dir);
            transform.DOMove(destination, _dir.magnitude / _speed.current).SetEase(Ease.Linear).OnComplete(OnCompleteTween);
            return this;
        }

        public ISkillshot Redirect(float degreeAngle)
        {
            transform.DOKill();
            _dir = Quaternion.AngleAxis(degreeAngle, Vector3.up) * _dir;
            transform.LookAtDirection3D(_dir);
            transform.DOMove(transform.position + _dir, _dir.magnitude / _speed.current).SetEase(Ease.Linear).OnComplete(OnCompleteTween);
            return this;
        }

        void Update()
        {
            update_assign?.Invoke();
        }

        Action update_assign;
        public ISkillshot OnUpdate(Action updateAction)
        {
            update_assign += updateAction;
            return this;
        }

        void OnCompleteTween()
        {           
            if (complete_assign == null) Dispose();
            else
            {
                complete_assign.Invoke();
                complete_assign = null;
            }
        }

        Action complete_assign;
        public ISkillshot OnComplete(Action completeAction)
        {
            complete_assign += completeAction;
            return this;
        }

        Action dispose_assign;

        public event Action<bool> OnVisualChanged;

        public ISkillshot OnDispose(Action disposeAction)
        {
            dispose_assign += disposeAction;
            return this;
        }


        public void Dispose()
        {
            dispose_assign?.Invoke();
            Pool.BackToPool(this);
            update_assign = null;
            complete_assign = null;
            dispose_assign = null;

            _speed.OnValueChange -= UpdateColliderSize;

            if (vfx != null) Pool.BackToPool(vfx);
        }

        public void ToggleOff()
        {
            isVisible = false;
            gameObject.SetActive(false);
        }

        public void ToggleOn()
        {
            isVisible = true;
            gameObject.SetActive(true);
        }

        public void DisappearAfter(float seconds)
        {
            throw new NotImplementedException();
        }
    }

}