using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RebuildUI;
namespace RebuildUI
{
    public class MouseRay : MonoBehaviour
    {
        // private bool isaix = false;
        public GameObject Information = null;
        private static GameObject taix = null;
        private static GameObject saix = null;
        private static GameObject raix = null;
        public static GameObject building = null;
        public static bool canTrans = false;
        public static bool canRotate = false;
        public static bool canScale = false;
        private float tsize = 257.6485f;
        private float ssize = 2.589466f;
        private float rsize = 2.589466f;

        public static void reset()
        {
            if (building)
                building.GetComponent<Binfo>().delet();
            canTrans = false;
            canRotate = false;
            canScale = false;
            building = null;
            taix = null;
            saix = null;
            raix = null;
        }

        void Start()
        {
            canTrans = false;
            canRotate = false;
            canScale = false;
            building = null;
            taix = null;
            saix = null;
            raix = null;
        }
        private float size = 0.0f;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point);
                    GameObject gameObj = hitInfo.collider.gameObject;
                    if (gameObj.tag == "building")
                    {
                        if (building)
                        {
                            building.GetComponent<MeshCollider>().enabled = true;
                            building.transform.GetChild(0).gameObject.SetActive(false);
                            building.transform.GetChild(1).gameObject.SetActive(false);
                            building.transform.GetChild(2).gameObject.SetActive(false);
                            building.GetComponent<Renderer>().materials[0].SetColor("_RimColor", new Color(0, 0, 0));
                        }
                        gameObj.GetComponent<MeshCollider>().enabled = (canTrans || canScale || canRotate) ? false : true;

                        gameObj.transform.GetChild(0).localScale = new Vector3(
                            tsize / gameObj.transform.localScale.x,
                            tsize / gameObj.transform.localScale.y,
                            tsize / gameObj.transform.localScale.z);
                        gameObj.transform.GetChild(1).localScale = new Vector3(
                            ssize / gameObj.transform.localScale.x,
                            ssize / gameObj.transform.localScale.y,
                            ssize / gameObj.transform.localScale.z);
                        gameObj.transform.GetChild(2).localScale = new Vector3(
                            rsize / gameObj.transform.localScale.x,
                            rsize / gameObj.transform.localScale.y,
                            rsize / gameObj.transform.localScale.z);

                        gameObj.transform.GetChild(0).gameObject.SetActive(canTrans ? true : false);
                        gameObj.transform.GetChild(1).gameObject.SetActive(canScale ? true : false);
                        gameObj.transform.GetChild(2).gameObject.SetActive(canRotate ? true : false);
                        gameObj.GetComponent<Renderer>().materials[0].SetColor("_RimColor", new Color(1, 0, 0));
                        building = gameObj;
                        taix = null;
                        saix = null;
                        raix = null;
                    }
                    else if (canTrans && gameObj.tag == "taix")
                    {
                        taix = gameObj;
                    }
                    else if (canScale && gameObj.tag == "saix")
                    {
                        saix = gameObj;
                    }
                    else if (canRotate && gameObj.tag == "raix")
                    {
                        raix = gameObj;
                    }
                }
                else
                {
                    taix = null;
                    saix = null;
                    raix = null;
                    if (building)
                    {
                        building.GetComponent<MeshCollider>().enabled = true;
                        building.transform.GetChild(0).gameObject.SetActive(false);
                        building.transform.GetChild(1).gameObject.SetActive(false);
                        building.transform.GetChild(2).gameObject.SetActive(false);
                        building.GetComponent<Renderer>().materials[0].SetColor("_RimColor", new Color(0, 0, 0));
                    }
                    building = null;
                }
            }

            if (taix)
            {
                if (Input.GetMouseButton(0))
                {
                    var vect = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    if (taix.name == "x")
                    {
                        Vector2 temp = Camera.main.WorldToScreenPoint(taix.transform.position + taix.transform.right) - Camera.main.WorldToScreenPoint(taix.transform.position);
                        building.transform.Translate(new Vector3(Vector2.Dot(vect, temp.normalized) * 10, 0, 0), Space.Self);
                    }
                    if (taix.name == "y")
                    {
                        Vector2 temp = Camera.main.WorldToScreenPoint(taix.transform.position + taix.transform.forward) - Camera.main.WorldToScreenPoint(taix.transform.position);
                        building.transform.Translate(new Vector3(0, Vector2.Dot(vect, temp.normalized) * 10, 0), Space.Self);
                    }
                    if (taix.name == "z")
                    {
                        Vector2 temp = Camera.main.WorldToScreenPoint(taix.transform.position + taix.transform.up) - Camera.main.WorldToScreenPoint(taix.transform.position);
                        building.transform.Translate(new Vector3(0, 0, -Vector2.Dot(vect, temp.normalized) * 10), Space.Self);
                    }
                }
            }

            if (saix)
            {
                if (Input.GetMouseButton(0))
                {
                    var vect = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    if (saix.name == "x")
                    {
                        if (building.transform.localScale.x > 0.0f)
                        {
                            Vector2 temp = Camera.main.WorldToScreenPoint(saix.transform.position + saix.transform.right) - Camera.main.WorldToScreenPoint(saix.transform.position);
                            building.transform.localScale += new Vector3(Vector2.Dot(vect, temp.normalized) / 10, 0, 0);
                        }
                        else
                            building.transform.localScale = new Vector3(0.01f, building.transform.localScale.y, building.transform.localScale.z);
                    }
                    if (saix.name == "y")
                    {
                        if (building.transform.localScale.y > 0.0f)
                        {
                            Vector2 temp = Camera.main.WorldToScreenPoint(saix.transform.position + saix.transform.up) - Camera.main.WorldToScreenPoint(saix.transform.position);
                            building.transform.localScale += new Vector3(0, Vector2.Dot(vect, temp.normalized) / 10, 0);
                        }
                        else
                            building.transform.localScale = new Vector3(building.transform.localScale.x, 0.01f, building.transform.localScale.z);
                    }
                    if (saix.name == "z")
                    {
                        if (building.transform.localScale.z > 0.0f)
                        {
                            Vector2 temp = Camera.main.WorldToScreenPoint(saix.transform.position + saix.transform.forward) - Camera.main.WorldToScreenPoint(saix.transform.position);
                            building.transform.localScale += new Vector3(0, 0, Vector2.Dot(vect, temp.normalized) / 10);
                        }
                        else
                            building.transform.localScale = new Vector3(building.transform.localScale.x, building.transform.localScale.y, 0.01f);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    //print(building.transform.localScale);
                    building.transform.GetChild(0).gameObject.transform.localScale = new Vector3(
                         tsize / building.transform.localScale.x,
                         tsize / building.transform.localScale.y,
                         tsize / building.transform.localScale.z);
                    building.transform.GetChild(1).gameObject.transform.localScale = new Vector3(
                         ssize / building.transform.localScale.x,
                         ssize / building.transform.localScale.y,
                         ssize / building.transform.localScale.z);
                    building.transform.GetChild(2).gameObject.transform.localScale = new Vector3(
                         rsize / building.transform.localScale.x,
                         rsize / building.transform.localScale.y,
                         rsize / building.transform.localScale.z);
                }
            }

            if (raix)
            {
                if (Input.GetMouseButton(0))
                {
                    var vect = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    if (raix.name == "x")
                    {
                        Vector2 temp = Camera.main.WorldToScreenPoint(raix.transform.position + raix.transform.forward) - Camera.main.WorldToScreenPoint(raix.transform.position + raix.transform.up);
                        building.transform.Rotate(new Vector3(Vector2.Dot(vect, temp.normalized) * 10, 0, 0), Space.Self);
                    }
                    if (raix.name == "y")
                    {
                        Vector2 temp = Camera.main.WorldToScreenPoint(raix.transform.position + raix.transform.right) - Camera.main.WorldToScreenPoint(raix.transform.position + raix.transform.forward);
                        building.transform.Rotate(new Vector3(0, Vector2.Dot(vect, temp.normalized) * 10, 0), Space.Self);
                    }
                    if (raix.name == "z")
                    {
                        Vector2 temp = Camera.main.WorldToScreenPoint(raix.transform.position + raix.transform.up) - Camera.main.WorldToScreenPoint(raix.transform.position + raix.transform.right);
                        building.transform.Rotate(new Vector3(0, 0, Vector2.Dot(vect, temp.normalized) * 10), Space.Self);
                    }
                }
            }

            if (building)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    tsize *= 1 + Time.deltaTime * 10;
                    ssize *= 1 + Time.deltaTime * 10;
                    rsize *= 1 + Time.deltaTime * 10;
                    building.transform.GetChild(0).gameObject.transform.localScale *= 1 + Time.deltaTime * 10;
                    building.transform.GetChild(1).gameObject.transform.localScale *= 1 + Time.deltaTime * 10;
                    building.transform.GetChild(2).gameObject.transform.localScale *= 1 + Time.deltaTime * 10;
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (tsize > 257.6485f / 5 && ssize > 2.589466f / 5)
                    {
                        tsize *= 1 - Time.deltaTime * 10;
                        ssize *= 1 - Time.deltaTime * 10;
                        rsize *= 1 - Time.deltaTime * 10;
                        building.transform.GetChild(0).gameObject.transform.localScale *= 1 - Time.deltaTime * 10;
                        building.transform.GetChild(1).gameObject.transform.localScale *= 1 - Time.deltaTime * 10;
                        building.transform.GetChild(2).gameObject.transform.localScale *= 1 - Time.deltaTime * 10;
                    }
                    else
                    {
                        tsize = 257.6485f / 5;
                        ssize = 2.589466f / 5;
                        rsize = 2.589466f / 5;
                    }
                }
                Information.transform.GetChild(1).GetComponent<Text>().text =
                    "Postion: X: " + System.Math.Round(building.transform.position.x, 2) +
                    "m Y: " + System.Math.Round(building.transform.position.y, 2) +
                    "m Z: " + System.Math.Round(building.transform.position.z, 2) + "m";

                Information.transform.GetChild(2).GetComponent<Text>().text =
                    "Rotation: X: " + System.Math.Round(building.transform.eulerAngles.x, 2) +
                    "° Y: " + System.Math.Round(building.transform.eulerAngles.y, 2) +
                    "° Z: " + System.Math.Round(building.transform.eulerAngles.z, 2) + "°";

                Information.transform.GetChild(3).GetComponent<Text>().text =
                    "Scale: X: " + System.Math.Round(building.transform.localScale.x, 2) +
                    "s Y: " + System.Math.Round(building.transform.localScale.y, 2) +
                    "s Z: " + System.Math.Round(building.transform.localScale.z, 2) + "s";

                Information.transform.GetChild(4).GetComponent<Text>().text = "Describe:\n" + "this is " + building.name.Replace("(Clone)", "") + ".";
            }
            else
            {
                Information.transform.GetChild(1).GetComponent<Text>().text = "Postion: X:none";
                Information.transform.GetChild(2).GetComponent<Text>().text = "Rotation: none";
                Information.transform.GetChild(3).GetComponent<Text>().text = "Scale: X:none";
                Information.transform.GetChild(4).GetComponent<Text>().text = "Describe:\nnone";
            }
        }
    }
}