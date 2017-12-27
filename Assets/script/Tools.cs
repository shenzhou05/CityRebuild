using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RebuildUI;
using UnityEditor;

namespace RebuildUI
{
    public class Tools : MonoBehaviour
    {
        void point()
        {
            this.transform.GetChild(0).GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            this.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1);
            MouseRay.canTrans = false;
            MouseRay.canRotate = false;
            MouseRay.canScale = false;
        }

        void trans()
        {
            this.transform.GetChild(1).GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            this.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1);
            MouseRay.canTrans = true;
            MouseRay.canRotate = false;
            MouseRay.canScale = false;
        }

        void rotate()
        {
            this.transform.GetChild(2).GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            this.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1);
            MouseRay.canRotate = true;
            MouseRay.canTrans = false;
            MouseRay.canScale = false;
        }

        void scale()
        {
            this.transform.GetChild(3).GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            this.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1);
            this.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            MouseRay.canScale = true;
            MouseRay.canTrans = false;
            MouseRay.canRotate = false;
        }

        void delet()
        {
            if (MouseRay.building)
            {
                MouseRay.reset();
                point();
            }
        }

        void outputOBJ()
        {
            //活动的tag为building的对象
            List<MeshFilter> meshFilters = new List<MeshFilter>();
            foreach (var building in GameObject.FindGameObjectsWithTag("building"))
            {
                if (building.activeSelf == true)
                {
                    meshFilters.Add(building.GetComponent<MeshFilter>());
                }
            }
            //print(meshFilters.Count);
            CombineInstance[] combine = new CombineInstance[meshFilters.Count];

            for (int i = 0; i < meshFilters.Count; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            string ExportOBJ_targetPath = "";
            MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
            mf.mesh.CombineMeshes(combine, false);
            ExportOBJ_targetPath = EditorUtility.SaveFilePanel("Save File", ExportOBJ_targetPath, "Object", "obj");
            ObjExporter.MeshToFile(mf, ExportOBJ_targetPath);
            Destroy(this.gameObject.GetComponent<MeshFilter>());
            meshFilters.Clear();
        }

        void Start()
        {
            this.transform.GetChild(0).GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            this.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(point);
            this.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(trans);
            this.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(rotate);
            this.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(scale);
            this.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delet);
            this.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(outputOBJ);
        }
    }
}