using Hung.Core;
using Hung.Pooling;
using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenNavigator : ObjectFollower<IOffscreenFollowable>
{
    [SerializeField] private bool isRotated = true;

    private Vector2 _midPort;
    private Vector2 _arrowPos;
    private float _angle;

    private readonly Vector2 OFFSET = new Vector2(0.5f, 0.5f);

    Collider _targetCollider;
    protected override void OnFollow(IOffscreenFollowable follow)
    {
        _targetCollider = follow.Collider;

        VisibleCheck += HasNavigate;
    }

    //Camera MainCam;
    //RayReference AimRay;
    //Canvas WorldCanvas;

    //private void Start()
    //{
    //    MainCam = SingletonRole.Cameraman.targetCamera;
    //    AimRay = CameraicFollower.Instance.AimRay;
    //    WorldCanvas = Ref.Instance.WorldCanvas;
    //}

    Plane[] planes = new Plane[5];

    bool HasNavigate()
    {
        //Debug.Log(Vector3.Distance(_targetCollider.transform.position, MainCam.transform.position));
        if (_targetCollider == null) return false;
        planes = GeometryUtility.CalculateFrustumPlanes(Archetype.Cameraman.targetCamera);
        //planes = planes[0..4];
        if (GeometryUtility.TestPlanesAABB(planes, _targetCollider.bounds))
        {
            return false;
        }
        return Vector3.Distance(_targetCollider.transform.position, Archetype.Cameraman.targetCamera.transform.position) <= Archetype.Cameraman.targetCamera.farClipPlane;
    }

    internal override void Unfollow(Action onComplete = null)
    {
        target = null;
        Pool.BackToPool(this);
    }

    protected override void StartFollow()
    {
        
        if (target == null) return;
        _backToCamVt = Archetype.Cameraman.transform.position - target.position;
        _backwardNormalVt = -Archetype.Cameraman.direction;
        _navDir = _backwardNormalVt.normalized * Mathf.Cos(Vector3.Angle(_backToCamVt, _backwardNormalVt) * Mathf.Deg2Rad) - _backToCamVt.normalized;
        _midPoint = Archetype.WorldCanvas.targetCanvas.transform.position;

        _midPort = (Vector2)Archetype.Cameraman.targetCamera.WorldToViewportPoint(_midPoint + _navDir) - OFFSET;

        _angle = ((_midPort.x < 0 ? 180 : 0) + Mathf.Atan((_midPort.y) / (_midPort.x)) * Mathf.Rad2Deg);

        _midPort /= (Mathf.Max(Mathf.Abs(_midPort.x), Mathf.Abs(_midPort.y)) * 2);

        m_rect.anchorMax = _midPort + OFFSET;
        m_rect.anchorMin = _midPort + OFFSET;
        m_rect.anchoredPosition = Vector2.zero;

        _arrowPos = m_rect.localPosition;

        _angle = ((_arrowPos.x < 0 ? 180 : 0) + Mathf.Atan((_arrowPos.y) / (_arrowPos.x)) * Mathf.Rad2Deg);

        if (isRotated)
            m_rect.localEulerAngles = Vector3.forward * _angle;

        _prePort = _midPort;
        base.StartFollow();
    }

    Vector2 _prePort;
    Vector2 _v1;
    [Range(0, 5f)][SerializeField] private float maxSpeed;
    protected override void OnFollowing()
    {
        _backToCamVt = Archetype.Cameraman.transform.position - target.position;
        _backwardNormalVt = -Archetype.Cameraman.direction;
        _navDir = _backwardNormalVt.normalized * Mathf.Cos(Vector3.Angle(_backToCamVt, _backwardNormalVt) * Mathf.Deg2Rad) - _backToCamVt.normalized;
        _midPoint = Archetype.WorldCanvas.targetCanvas.transform.position;

        _midPort = (Vector2)Archetype.Cameraman.targetCamera.WorldToViewportPoint(_midPoint + _navDir) - OFFSET;

        _angle = ((_midPort.x < 0 ? 180 : 0) + Mathf.Atan((_midPort.y) / (_midPort.x)) * Mathf.Rad2Deg);

        _midPort /= ( Mathf.Max(Mathf.Abs(_midPort.x), Mathf.Abs(_midPort.y)) * 2);

        _prePort = Vector2.SmoothDamp(_prePort, _midPort, ref _v1, smoothTime, maxSpeed);

        m_rect.anchorMax = _prePort + OFFSET;
        m_rect.anchorMin = _prePort + OFFSET;
        m_rect.anchoredPosition = Vector2.zero;

        _arrowPos = m_rect.localPosition;

        _angle = ((_arrowPos.x < 0 ? 180 : 0) + Mathf.Atan((_arrowPos.y) / (_arrowPos.x)) * Mathf.Rad2Deg);

        if (isRotated)
            m_rect.localEulerAngles = Vector3.forward * _angle;
    }


    Vector3 _backToCamVt;
    Vector3 _backwardNormalVt;
    Vector3 _navDir;
    Vector3 _midPoint;

    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.red;

        Gizmos.DrawRay(target.position, _backToCamVt);

        Gizmos.DrawRay(_midPoint, _backwardNormalVt);

        Gizmos.DrawRay(_midPoint, _navDir);

        //Gizmos.DrawRay(midPoint, _screenPos);
    }
}
