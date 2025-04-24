using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkin<T> where T: System.Enum
{
    void ChooseSkin(T skin);
}