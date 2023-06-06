using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CamerCapture
{
    [MenuItem("tools/CaptureCameraTexture %&#C")]
    public static void CaptureTexture()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.Log("捕获图片失败,MainCamera相机为空");
            return;
        }

        Vector2 viewSize = Handles.GetMainGameViewSize();
        Vector2Int textureSize = new Vector2Int((int)viewSize.x, (int)viewSize.y);
        RenderTexture renderTexture = new RenderTexture(textureSize.x, textureSize.y, 0);

        camera.targetTexture = renderTexture;
        camera.Render();

        RenderTexture.active = renderTexture;
        Texture2D mtexture = new Texture2D(textureSize.x, textureSize.y);
        Rect rect = new Rect();
        rect.width = textureSize.x;
        rect.height = textureSize.y;

        mtexture.ReadPixels(rect, 0, 0);
        mtexture.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null;
        string path = EditorUtility.SaveFilePanel("保存相机图片", "Asset", "_CameraTexture", ".png");

        ExportTexture(path, mtexture);


    }

    public static void ExportTexture(string path, Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        FileStream file;
        if (!Directory.Exists(path))
        {
            file = File.Open(path, FileMode.Create);
        }
        else
        {
            file = File.Open(path, FileMode.Open);
        }
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        file.Dispose();
        Debug.Log("保存成功:" + path);
    }
}
