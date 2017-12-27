using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RebuildUI;

namespace RebuildUI
{
    public class AixRotate : MonoBehaviour
    {
        void Update()
        {
            //算法不对
            var temp = GameObject.Find("Main Camera").transform.localEulerAngles;
            this.transform.localEulerAngles = new Vector3(temp.x, temp.y + 180, temp.z);
        }
    }
}