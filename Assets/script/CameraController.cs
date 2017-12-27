using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RebuildUI;
namespace RebuildUI
{
    public enum State
    {
        normal,
        orth
    }

    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public float Speed = 100.0f;
        public float RotateSpeed = 10.0f;
        public BoxCollider boxColl = null;
        public Camera Second = null;

        private Transform cameraTrans;
        private int wDown = 0;
        private int sDown = 0;
        private int aDown = 0;
        private int dDown = 0;
        private int qDown = 0;
        private int eDown = 0;
        private bool rightMouseDown = false;
        private float spw;
        private State state = State.normal;
        private Vector3 originPos;
        private Vector3 originRot;

        private void CameraMove(float speed, State state = State.normal)
        {
            if (Input.GetMouseButton(2))
            {
                cameraTrans.Translate(-Input.GetAxis("Mouse X") * Time.deltaTime * 10 * speed, -Input.GetAxis("Mouse Y") * Time.deltaTime * 10 * speed, 0, Space.Self);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                wDown = 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                sDown = 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                aDown = 1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                dDown = 1;
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (state == State.normal)
                    cameraTrans.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 50 * speed), Space.Self);
                else
                {
                    cameraTrans.GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 500;
                    if (cameraTrans.GetComponent<Camera>().orthographicSize > 2000)
                        cameraTrans.GetComponent<Camera>().orthographicSize = 2000;
                    if (cameraTrans.GetComponent<Camera>().orthographicSize < 0)
                        cameraTrans.GetComponent<Camera>().orthographicSize = 10;
                    this.transform.GetChild(1).transform.localScale = new Vector3(size * cameraTrans.GetComponent<Camera>().orthographicSize / 100f, size * cameraTrans.GetComponent<Camera>().orthographicSize / 100f, size * cameraTrans.GetComponent<Camera>().orthographicSize / 100f);
                    Second.orthographicSize = 10f * cameraTrans.GetComponent<Camera>().orthographicSize / 100f;
                }
            }
            if (state == State.normal)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    qDown = 1;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    eDown = 1;
                }
            }

            if (wDown == 1 || sDown == 1 || aDown == 1 || dDown == 1 || qDown == 1 || eDown == 1)
            {
                cameraTrans.Translate(state == State.normal ?
                    new Vector3(Time.deltaTime * speed * (dDown - aDown), Time.deltaTime * speed * (qDown - eDown), Time.deltaTime * speed * (wDown - sDown)) :
                    new Vector3(Time.deltaTime * speed * (dDown - aDown), Time.deltaTime * speed * (wDown - sDown), Time.deltaTime * speed * (qDown - eDown))
                    , Space.Self);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                wDown = 0;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                sDown = 0;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                aDown = 0;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                dDown = 0;
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                qDown = 0;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                eDown = 0;
            }

            if (Mathf.Abs(cameraTrans.position.x) > boxColl.size.x / 2)
            {
                cameraTrans.position = new Vector3(cameraTrans.position.x > 0 ? (boxColl.size.x / 2) - 1 : -(boxColl.size.x / 2) + 1, cameraTrans.position.y, cameraTrans.position.z);
            }
            else if (Mathf.Abs(cameraTrans.position.z) > boxColl.size.z / 2)
            {
                cameraTrans.position = new Vector3(cameraTrans.position.x, cameraTrans.position.y, cameraTrans.position.z > 0 ? (boxColl.size.z / 2) - 1 : -(boxColl.size.z / 2) + 1);
            }
            else if (cameraTrans.position.y > (boxColl.size.y / 2 + boxColl.center.y) || cameraTrans.position.y < (boxColl.center.y - boxColl.size.y / 2))
            {
                cameraTrans.position = new Vector3(cameraTrans.position.x, cameraTrans.position.y > boxColl.center.y ? (boxColl.size.y / 2 + boxColl.center.y - 1) : (boxColl.center.y - (boxColl.size.y / 2) + 1), cameraTrans.position.z);
            }
        }
        private float size = 0.0006489074f * 343f;
        private void CameraRotate(float speed)
        {
            if (Input.GetMouseButton(1))
            {
                cameraTrans.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * speed, Input.GetAxis("Mouse X") * Time.deltaTime * speed, 0, Space.Self);
                cameraTrans.eulerAngles = new Vector3(cameraTrans.eulerAngles.x, cameraTrans.eulerAngles.y, 0);
            }
        }

        void Start()
        {
            cameraTrans = this.transform;
            boxColl.isTrigger = true;
            spw = this.Speed;
            this.state = State.normal;
            originPos = cameraTrans.position;
            originRot = cameraTrans.eulerAngles;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                spw = this.Speed * 3;
            if (Input.GetKeyUp(KeyCode.LeftShift))
                spw = this.Speed;
            switch (this.state)
            {
                case State.normal:
                    {
                        CameraMove(spw, State.normal);
                        CameraRotate(this.RotateSpeed);
                        if (Input.GetKeyDown(KeyCode.O))
                        {
                            print("切换为正交模式");
                            this.state = State.orth;
                            cameraTrans.GetComponent<Camera>().orthographic = true;
                            cameraTrans.GetComponent<Camera>().orthographicSize = 100;
                            cameraTrans.eulerAngles = new Vector3(90, 0, 0);
                            cameraTrans.position = new Vector3(0, boxColl.size.y / 2 + boxColl.center.y, 0);
                            this.transform.GetChild(1).transform.localScale *= 343f;
                            Second.orthographicSize = 10f;
                        }
                        break;
                    }
                case State.orth:
                    {
                        CameraMove(spw, State.orth);
                        if (Input.GetKeyDown(KeyCode.N))
                        {
                            print("切换为正常模式");
                            this.state = State.normal;
                            cameraTrans.GetComponent<Camera>().orthographic = false;
                            cameraTrans.eulerAngles = originRot;
                            cameraTrans.position = originPos;
                            this.transform.GetChild(1).transform.localScale = new Vector3(0.0006489074f, 0.0006489074f, 0.0006489074f);
                            Second.orthographicSize = 0.03f;
                        }
                        break;
                    }
            }
        }
    }
}