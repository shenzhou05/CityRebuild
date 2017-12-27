using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RebuildUI;

namespace RebuildUI
{
    public class Binfo : MonoBehaviour
    {
        public void delet()
        {
            //Destroy(this);//删不了？？？
            this.gameObject.SetActive(false);
            //print("ok");
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}