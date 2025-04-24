//using Hung.Core;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : Singleton<InputManager>
//{
//    private Bolt boltSelected;

//    public Bolt BoltSelected => boltSelected;

//    public bool IsCanSelectBolt { get; private set; } = true;

//    private bool isRemovingBolt = false;

//    //private void OnMouseDown()
//    //{

//    //    if (!IsCanSelectBolt) return;
     

//    //    if (IsCanSelectBolt)
//    //    {
//    //        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
//    //        {
//    //            if (hit.collider.TryGetComponent<Bolt>(out Bolt bolt))
//    //            {
//    //                Debug.Log("Bolt selected" + bolt.name);
//    //                boltSelected = bolt;
//    //                boltSelected.TryPinBolt();
//    //            }
//    //        }
//    //    }
//    //}
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (!IsCanSelectBolt) return;

//            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

//            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

//            if (hit.collider != null)
//            {
//                if (hit.collider.TryGetComponent<Bolt>(out Bolt bolt))
//                {
//                    boltSelected = bolt;
//                    boltSelected.TryPinBolt();
//                }
//            }

//        }
//    }

//}

