using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroupHolder<T> where T: InstanceOwner<T>
{
    Transform Holder { get; }

    List<T> GetAll(T type);
}

public interface IManageList<T>
{
    T GetRandom(T excluded);
}



public class InstanceOwner<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; }
}
