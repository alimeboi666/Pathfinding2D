using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Bolt : MonoBehaviour, IBolt, IInteractable, IPiece
{
    [Header("References")]
    private Collider2D modelCollider;

    private List<HingeJoint2D> joints = new();

    [Header("Parameters")]
    [SerializeField] private float moveTime;




    private bool isPick = false;

    private void Awake()
    {
        
    }



    public void PickToHole()
    {
        isPick = true;

    }

    public void PinToHole()
    {
        isPick = false;
    }

    public void TryPinBolt()
    {
        if (CanPin())
        {
            PinToHole();
            AnimatePinBolt(isPick);
        }
        else
        {
            PickToHole();
            AnimatePickBolt(isPick);
        }
    }

    private bool CanPin()
    {
        return isPick;
    }



    private void MoveToNewHole()
    {

    }



    private void ReplacePlateHoles()
    {

    }


    private void SaveState()
    {

    }

    public void AddPlateHole()
    {

    }

    public void SetColliderTrigger(bool trigger)
    {
        if (this.modelCollider != null)
            this.modelCollider.isTrigger = trigger;
    }

    public void SetJoint(Rigidbody2D plateBody)
    {
        HingeJoint2D newJoint = gameObject.AddComponent<HingeJoint2D>();
        newJoint.connectedBody = plateBody;
        newJoint.enableCollision = false;
        this.joints.Add(newJoint);
    }

    public void RemoveJoint(Rigidbody2D plateBody)
    {
        foreach (HingeJoint2D joint in this.joints)
        {
            if (joint.connectedBody == plateBody)
            {
                this.joints.Remove(joint);
                Destroy(joint);
                break;
            }
        }
    }

    public void RemoveSelf()
    {
        gameObject.SetActive(false);
    }

    #region Animation

    [SerializeField] private WorldModel model;

    public void AnimatePickBolt(bool isPick)
    {
        model.AnimatePick(isPick);
    }

    public void AnimatePinBolt(bool isPick)
    {
        model.AnimatePick(isPick);
    }

    #endregion
}

