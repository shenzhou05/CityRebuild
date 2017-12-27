using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RebuildUI;

namespace RebuildUI
{
    public class ButtonBuilder : MonoBehaviour
    {
        public GameObject buildPerb;

        void onclick()
        {
            Instantiate(buildPerb, GameObject.Find("builds").transform);
        }
        void Start()
        {
            this.transform.GetComponent<Button>().onClick.AddListener(onclick);
        }

        void Update()
        {

        }
    }
}