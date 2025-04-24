using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using Hung.GameData;
using Hung.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

public class ResourceClaim : MonoBehaviour, ICommonPoolable
{
    [field: SerializeField] public ParticleSystem particle { get; private set; }

    [SerializeField] private int maxDrop;

    public PoolPartition pooling { get; set; }
    public bool isNewPooling { get; set; }

    [Button]
    public void Drop(Vector3 worldPos, IResource resource)
    {
        Archetype.MasterSound.PlayUISound(UISound.ResourceClaim);
        var resourceData = DataCollection.GetResourceData(resource);

        particle.textureSheetAnimation.SetSprite(0, resourceData.Icon);

        var part = Mathf.RoundToInt((resource.value / resourceData.flexibleValue * maxDrop).ToRealValue());
        part = Mathf.Clamp(part, 1, maxDrop);
        resourceData.VisualClaim(this, part, part == 1 ? resource.value : (resource.value / part));
        particle.Emit(part);
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
