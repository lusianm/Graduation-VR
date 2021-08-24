using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ByteToRawTexture : MonoBehaviour
{
    Texture2D tex;
    [SerializeField] List<RawImage> renders;
    public void Start()
    {
        //tex = new Texture2D(720, 480, TextureFormat.RGBA4444, false);
        tex = new Texture2D(720, 480, TextureFormat.RGBA32, false);
    }

    public void setTexture(byte[] imageByte)
    {
        if (imageByte.Length < 1)
            return;

        Debug.Log("Image Received");
        byte[] decompressedByte = imageByte;
        tex.LoadRawTextureData(decompressedByte);
        tex.Apply();

        foreach (RawImage render in renders)
        {
            render.material.mainTexture = tex;
            render.texture = tex;
        }
    }
    public void LoadPNG(byte[] pngByte)
    {
        if (pngByte.Length < 1)
            return;

        Debug.Log("Png Received");
        tex.LoadImage(pngByte);
        tex.Apply();
        foreach (RawImage render in renders)
        {
            render.material.mainTexture = tex;
            render.texture = tex;
        }
    }
}
