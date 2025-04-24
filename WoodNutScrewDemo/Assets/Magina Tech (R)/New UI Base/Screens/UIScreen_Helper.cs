using System;
using System.Collections;
using System.Collections.Generic;
using Hung.UI;
using Hung.UI.Button;
using TMPro;
using UnityEngine;

namespace Hung.UI.Extensions
{
    public class UIScreen_Helper : UIScreen
    {
        [SerializeField] private TMP_Text txtContent;
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private SimpleButton btnGoto;

        public void Set(string content, string title, Action gotoAction = null, Action dismissAction = null)
        {
            txtContent.text = content;
            txtTitle.text = title;
            btnGoto.CommitAction = gotoAction;
        }
    }
}
