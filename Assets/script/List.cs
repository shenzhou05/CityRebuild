using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RebuildUI;

namespace RebuildUI
{
    public class List : MonoBehaviour
    {
        public GameObject buttonPerb;
        public GameObject[] builds;
        private int count;

        void Start()
        {
            count = builds.Length;
            this.transform.parent.parent.GetComponentInChildren<Scrollbar>().interactable = (count <= 9) ? false : true;
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(buttonPerb, this.transform);
                obj.GetComponentInChildren<Text>().text = builds[i].gameObject.name;
                obj.AddComponent<ButtonBuilder>().buildPerb = builds[i];
            }
            if (count > 9)
            {
                this.GetComponent<RectTransform>().sizeDelta = new Vector2(359.5f, 450 + (count - 9) * 105);
            }
        }

        void Update()
        {

        }
    }
}