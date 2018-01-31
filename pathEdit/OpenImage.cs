using UnityEngine;

using System.Collections;

using System.Text;

using System.Runtime.InteropServices;

using System;

public class OpenImage : MonoBehaviour
{

    void Start()
    {
      
        OnGUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Open Image"))
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = "图片(*.jpg)\0*.jpg\0图片(*.jpng)\0*.jpng\0图片(*.png)\0*.png";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            openFileName.title = "窗口标题";
            openFileName.flags =
                0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

            if (LocalDialog.GetSaveFileName(openFileName))
            {
                Debug.Log("获取路径成功："+openFileName.file);
                Debug.Log("文件名：" + openFileName.fileTitle);
                LocalDialog.WaitLoad(openFileName.file);
            }
        }
    }


}