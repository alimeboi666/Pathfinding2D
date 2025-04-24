using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI.Button.Clickable
{
    public abstract class ClickableConfig : ScriptableObject
    {
        public abstract void SetClickable(Image clickPane, Image targetPane, bool set);
    }

}
