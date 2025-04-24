using Hung;
using Hung.AbilitySystem;
using Hung.Pooling;
using System.Collections;
using System.Collections.Generic;
using Hung.AbilitySystem.Core;
using Hung.StatSystem;
using Unity.Mathematics;
using UnityEngine;
using Hung.Unit;
using Sirenix.OdinInspector;
using System;

public class AreaOfEffect : MonoBehaviour, ISkillshot, ICommonPoolable  
{
    public Vector3 positionValue => transform.position;

    [field: ReadOnly][field: SerializeField] public VisualEffect3D vfx { get; set; }
    [field: ReadOnly][field:SerializeField] public PoolPartition pooling { get; set; }
    public bool isNewPooling { get; set; }
    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    private Area FIXED_RAD = new();
    [SerializeField] private Area _rad;
    private IAbility _caster;
    private AOEShape _shape;
    private float _angle;
    private int _layerMask;

    public void Setup(IAbility caster, RPG_Effect effect, VisualEffect3D vfx, AOEShape shape, float degreeAngle, Area radius)
    {
        _rad = radius;
        if (_rad == null) _rad = FIXED_RAD;
        _caster = caster;
        this.vfx = vfx;
        vfx.size = (float)_rad.current;
        //if (caster.unit.teamID == 10) _layerMask = Enemy.LAYER_MASK;
        //else _layerMask = Player.LAYER_MASK;

        Setup_Core(effect, shape, degreeAngle);
    }


    private void Setup_Core(RPG_Effect effect, AOEShape shape, float degreeAngle)
    {
        _shape = shape;
        _angle = degreeAngle * Mathf.Deg2Rad;
        switch (shape)
        {
            case AOEShape.Circle:
                StartCoroutine(DealingEffectOvertime(CircleDetect, effect, StatSystem.TICK_TIME));
                break;

            case AOEShape.Cone:
                StartCoroutine(DealingEffectOvertime(ConeDetect, effect, StatSystem.TICK_TIME));
                break;
        }       
    }

    delegate int ColliderDetector(Vector3 position, float radius, Collider[] res);

    private Collider[] _res = new Collider[100];
    private Collider[] _temp = new Collider[100];

    private int CircleDetect(Vector3 position, float radius, Collider[] res)
    {
        return Physics.OverlapSphereNonAlloc(position, radius, res, _layerMask);
    }

    private int ConeDetect(Vector3 position, float radius, Collider[] res)
    {
        var all = Physics.OverlapSphereNonAlloc(position, radius, _temp, _layerMask);
        int hit_count = 0;
        for (int i = 0; i < all; i++)
        {
            var targetPosition = _temp[i].transform.position;
            var dir = Vector3.Angle(transform.forward, targetPosition - position);
            //Debug.Log(dir);
            //Debug.Log(_caster.unit.faceDirection); 
            //if (GeometryUtils.IsSmallerThan(dir, GeometryUtils.Plus(face, _angle/2)) && GeometryUtils.IsSmallerThan(GeometryUtils.Plus(face, -_angle / 2), dir))
            //{
            //    res[hit_count++] = _temp[i];
            //}
            if (dir <= _angle * 0.5f * Mathf.Rad2Deg)
            {
                res[hit_count++] = _temp[i];
            }
        }
        return hit_count;
    }

    private IUnit _unit;
    IEnumerator DealingEffectOvertime(ColliderDetector detector, RPG_Effect effect, float tick)
    {
        WaitForSeconds tickDelay = new WaitForSeconds(tick);
        while (true)
        {
            EffectOnTick(detector, effect);

            yield return tickDelay;
        }
    }

    void EffectOnTick(ColliderDetector detector, RPG_Effect effect)
    {
        var hit_count = detector(transform.position, _rad.current, _res);
        if (hit_count > 0)
        {
            for (int i = 0; i < hit_count; i++)
            {
                if (_res[i].gameObject.TryGetComponent(out _unit))
                {
                    effect(_unit);
                }
            }
        }
    }

    void EffectOnTick_(ColliderDetector detector, RPG_Effect effect)
    {
        var hit_count = detector(transform.position, _rad.current, _res);
        if (hit_count > 0)
        {
            for (int i = 0; i < hit_count; i++)
            {
                if (_res[i].gameObject.TryGetComponent(out _unit))
                {
                    //Debug.Log("AOE Detect: " + _unit.gameObject.name); 
                    effect(_unit);
                }
            }
        }
    }

    public void InstantAffect(RPG_Effect effect)
    {
        switch (_shape)
        {
            case AOEShape.Circle:
                EffectOnTick_(CircleDetect, effect);
                break;

            case AOEShape.Cone:
                EffectOnTick(ConeDetect, effect);
                break;
        }
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

    public ISkillshot Clone()
    {
        throw new System.NotImplementedException();
    }

    public ISkillshot DOAttack(IPosition caster, ISkillTargetType targetType = null)
    {
        throw new System.NotImplementedException();
    }

    public ISkillshot DOFollow(IUnit target)
    {
        throw new System.NotImplementedException();
    }

    public ISkillshot DOMoveDirection(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public ISkillshot DOMoveLocation(Vector3 location)
    {
        throw new System.NotImplementedException();
    }

    public ISkillshot Redirect(float angle)
    {
        throw new System.NotImplementedException();
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

    public ISkillshot OnComplete(Action completeAction)
    {
        throw new System.NotImplementedException();
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
        dispose_assign = null;
        if (vfx != null) Pool.BackToPool(vfx);
    }

    public void DisappearAfter(float seconds)
    {
        Invoke("Dispose", seconds);
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
}
