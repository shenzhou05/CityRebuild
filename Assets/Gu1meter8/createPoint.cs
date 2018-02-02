using UnityEngine;
using System.Collections;

public class createPoint : MonoBehaviour {
    static string CUBENAME = "game";
	// Use this for initialization
	void Start () {
	
	}
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("鼠标点击");
                if (hit.collider.gameObject.name == CUBENAME)
                {
                    Debug.Log("碰到立方体");
                    GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
                    //(GameObject)Instantiate(Resources.Load("Sphere"));
                    point.transform.position = new Vector3(hit.point.x, 5, hit.point.z);
                    point.GetComponent<Renderer>().material.color = Color.red;
                }
                    
                //create a ball
            }

                
        }
    }

}
