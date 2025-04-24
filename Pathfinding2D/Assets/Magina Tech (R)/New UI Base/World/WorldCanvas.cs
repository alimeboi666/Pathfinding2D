using System;
using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public record LayerElement
{
    public Transform head;
    public List<Transform> elements;

    public void AddElement(Transform element)
    {
        if (elements == null) elements = new List<Transform>();
        if (!elements.Contains(element)) elements.Add(element);
    }

    public void Remove(Transform element)
    {
        if (element == head)
        {
            AssignNewHead();
        }
        else if (elements != null)
        {
            elements.Remove(element);
        }
    }

    public void AssignNewHead()
    {     
        if (elements == null || elements.Count == 0)
        {
            head = null;
            return;
        }
        head = elements[0];
        elements.Remove(head);
    }
}

public class WorldCanvas : MonoBehaviour, IWorldCanvas, ILayer
{
    [field:SerializeField] public Canvas targetCanvas { get; private set; }

    [SerializeField] private GraphicRaycaster raycaster;
    public bool isInteractable
    {
        get => raycaster.enabled;

        set => raycaster.enabled = value;
    }

    //[ReadOnly, ListDrawerSettings(ShowIndexLabels = true), SerializeField] protected LayerElement[] layers = new LayerElement[ILayer.MAX_LAYER];

    //public void AssignLayer(Transform ui, int layer)
    //{
    //    //Debug.Log("Assign " + ui.name + " to layer " + layer);
    //    if (layers[layer].head == null)
    //    {
    //        layers[layer].head = ui;
    //        int i;
    //        for (i = layer + 1; i < layers.Length; i++)
    //        {
    //            if (layers[i].head != null)
    //            {
    //                //Debug.Log("Align with layer " + i);
    //                var arrangeIndex = layers[i].head.GetSiblingIndex();
    //                Debug.Log($"Arrange {ui.name} at {arrangeIndex} with alignment of layer " + i);
    //                ui.SetSiblingIndex(arrangeIndex);

    //                break;
    //            }
    //        }
    //        if (i == layers.Length)
    //        {
    //            ui.SetAsLastSibling();
    //            Debug.Log($"Arrange {ui.name} to the first class of layer {layer}");
    //            //ui.gameObject.name += " => Lead";
    //        }

    //    }
    //    else
    //    {
    //        if (ui == layers[layer].head) return;
    //        var arrangeIndex = layers[layer].head.GetSiblingIndex() + 1;
    //        Debug.Log($"Arrange {ui.name} at {arrangeIndex} with alignment of the leader");
    //        ui.SetSiblingIndex(arrangeIndex);
    //        layers[layer].AddElement(ui);
    //        //ui.gameObject.SetActive(false);
    //    }
    //    //if (layer == 1) 
    //    //{
    //    //    Debug.Log("Kekw"); 
    //    //}
    //}

    //public void RemoveLayer(Transform ui, int layer)
    //{
    //    layers[layer].Remove(ui);
    //}

    [SerializeField] Transform[] layerHolder = new Transform[ILayer.MAX_LAYER];

    public void AssignLayer(Transform ui, int layer)
    {
        //if (layerHolder[layer] == null)
        //{
        //    var layerTransform = new GameObject("Layer " + layer).transform as RectTransform;
        //    layerTransform.SetParent(transform);
        //    layerTransform.localScale = Vector3.one;
        //    layerTransform.localPosition = Vector3.zero;
        //    layerTransform.localRotation = Quaternion.identity;
        //    layerTransform.anchorMax = Vector2.zero;
        //    layerTransform.anchorMin = Vector2.zero;
        //    layerTransform.anchoredPosition = Vector2.zero;
        //    int i;
        //    for (i = layer + 1; i < layerHolder.Length; i++)
        //    {
        //        if (layerHolder[i] != null)
        //        {
        //            layerTransform.SetSiblingIndex(layerHolder[i].GetSiblingIndex() - 1);
        //        }
        //    }
        //    if (i == layerHolder.Length)
        //    {
        //        layerTransform.SetAsLastSibling();
        //    }
        //    layerHolder[layer] = layerTransform;
        //}
        ui.SetParent(layerHolder[layer]);
    }

    public void RemoveLayer(Transform ui, int layer)
    {
        
    }
}
