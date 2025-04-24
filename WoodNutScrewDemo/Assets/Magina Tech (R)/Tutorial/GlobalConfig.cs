using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConfig
{
    void Config();

    void AssignConfig(ref Action configuration);
}
