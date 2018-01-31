using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class LocalDialog : MonoBehaviour
{
    //链接指定系统函数       打开文件对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOFN([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //链接指定系统函数        另存为对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool GetSFN([In,Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn); 
    }

      public static void WaitLoad(string fileName)
    {
        try            {                 WWW wwwTexture = new WWW("file://" + fileName);
                 Texture2D t2d = new Texture2D(200, 200);
                 wwwTexture.LoadImageIntoTexture(wwwTexture.texture);
                 GameObject.Find("game").GetComponent<Renderer>().material.mainTexture = wwwTexture.texture;//测试贴图是否生成成功（你测试完之后可以删了）
                 Debug.Log("创建贴图成功： "+ fileName);                //createACuboid所在类.createACuboid(wwwTexture.texture);//调用你的函数                      }            catch (Exception e)            {               throw e;            }       } 
    }
}

