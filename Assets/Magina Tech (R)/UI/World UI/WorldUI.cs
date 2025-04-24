using Hung.Core;
using Hung.Pooling;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class WorldUI : MonoBehaviour, ICanvas, IPoolable
{
    [field: SerializeField] public Vector3 Offset { get; set; }

    [field: Range(0, ILayer.MAX_LAYER - 1), SerializeField] public int layer { get; private set; }

    [field:SerializeField] public bool cameraZoomListen { get; private set; }
    [ShowIf("cameraZoomListen"), Range(1, 3), SerializeField] private float maxZoomer = 3;
    [field: SerializeField] public bool lockHidden { get; protected set; }

    [field:SerializeField] public bool onScreenAdapt { get; private set; }

    [ShowIf("onScreenAdapt"), SerializeField] private float adjustOffset;

    [ReadOnly, SerializeField] private Canvas _canvas;
    public Canvas targetCanvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = GetComponentInParent<Canvas>();
            }
            return _canvas;
        }
    }

    public PoolPartition pooling { get; set; }

    public bool isNewPooling { get; set; }

    protected RectTransform m_rect;

    [Button]
    bool IsRectTransformPartiallyOutsideCanvas(out Vector2 adjustment)
    {
        RectTransform canvasRectTransform = targetCanvas.transform as RectTransform;
        transform.localScale = Vector2.one;

        Vector3[] objectCorners = new Vector3[4];
        m_rect.GetWorldCorners(objectCorners);
        //foreach(var vt in objectCorners)
        //{
        //    Debug.Log(vt);
        //}
        adjustment = Vector2.zero;

        foreach (Vector3 corner in objectCorners)
        {
            Vector3 canvasLocalCorner = canvasRectTransform.InverseTransformPoint(corner);

            if (canvasLocalCorner.x < canvasRectTransform.rect.xMin + adjustOffset)
            {
                adjustment.x += (canvasRectTransform.rect.xMin + adjustOffset) - canvasLocalCorner.x;
            }
            else if (canvasLocalCorner.x > canvasRectTransform.rect.xMax - adjustOffset)
            {
                adjustment.x -= canvasLocalCorner.x - (canvasRectTransform.rect.xMax - adjustOffset);
            }

            if (canvasLocalCorner.y < canvasRectTransform.rect.yMin + adjustOffset)
            {
                adjustment.y += (canvasRectTransform.rect.yMin + adjustOffset) - canvasLocalCorner.y;
            }
            else if (canvasLocalCorner.y > canvasRectTransform.rect.yMax - adjustOffset)
            {
                adjustment.y -= canvasLocalCorner.y - (canvasRectTransform.rect.yMax - adjustOffset);
            }
        }

        return adjustment != Vector2.zero;
    }

    ILayer m_layerHolder;
    protected void OnEnable()
    {
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;

        var rect = transform as RectTransform;
        rect.anchorMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;

        if (transform.parent.TryGetComponent(out m_layerHolder))
        {
            m_layerHolder.AssignLayer(transform, layer);
        }
        if (cameraZoomListen) Archetype.Cameraman.OnCameraSizeChanged += OnCameraSizeChanged;
    }

    protected void OnDisable()
    {
        if (m_layerHolder != null)
        {
            m_layerHolder.RemoveLayer(transform, layer);
            m_layerHolder = null;
        }
        if (cameraZoomListen && Archetype.Cameraman != null) Archetype.Cameraman.OnCameraSizeChanged -= OnCameraSizeChanged;
    }

    void OnCameraSizeChanged(float cameraSize)
    {
        transform.localScale = Mathf.Min(cameraSize, maxZoomer) * Vector3.one;
    }

    public void SetLayer(int layer)
    {
        if (transform.parent.TryGetComponent(out m_layerHolder))
        {
            m_layerHolder.RemoveLayer(transform, this.layer);
            m_layerHolder.AssignLayer(transform, layer);
        }
        this.layer = layer;
    }


    protected void Awake()
    {
        m_rect = (RectTransform)transform;
    }

    Vector2 _cur;
    [Range(0.001f, 0.1f)][SerializeField] protected float smoothTime = 0.001f;
    protected void LocateAt(Vector3 worldPos)
    {
        m_rect.anchoredPosition = Vector2.SmoothDamp(m_rect.anchoredPosition, Archetype.Cameraman.targetCamera.WorldToScreenPoint(worldPos) / targetCanvas.scaleFactor + Offset, ref _cur, smoothTime);
    }


    public void FollowWorldPos(Vector3 worldPos, float time, Action followAction = null)
    {
        StartCoroutine(FollowingPos(worldPos, time, followAction));
    }

    IEnumerator FollowingPos(Vector3 worldPos, float time, Action followAction)
    {
        while (time > 0)
        {
            InstantLocateAt(worldPos);
            yield return null;
            time-= Time.deltaTime;
            followAction?.Invoke();
        }
    }

    public void InstantLocateAt(Vector3 worldPos)
    {
        m_rect = transform as RectTransform;
        m_rect.anchoredPosition = Archetype.Cameraman.targetCamera.WorldToScreenPoint(worldPos) / targetCanvas.scaleFactor + Offset;
        if (onScreenAdapt)
        {
            if (IsRectTransformPartiallyOutsideCanvas(out Vector2 adjustment))
            {
                Debug.Log(name + " is outsided");
                m_rect.anchoredPosition += adjustment;
            }
        }      
    }

    public void InstantAnchorAt(Vector3 screenPoint)
    {
        m_rect.anchoredPosition = screenPoint / targetCanvas.scaleFactor + Offset;
    }

    public static void AnchorAt(RectTransform rect, Vector3 worldPos, Canvas canvas)
    {
        rect.SetParent(canvas.transform, true);
        rect.localEulerAngles = Vector3.zero;

        rect.anchorMax = Vector2.zero;
        rect.anchorMin = Vector2.zero;

        rect.anchoredPosition = Archetype.Cameraman.targetCamera.WorldToScreenPoint(worldPos) / canvas.scaleFactor;
    }

    public abstract void ToggleOn();
    public abstract void ToggleOff();
}