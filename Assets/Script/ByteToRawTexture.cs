using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ByteToRawTexture : MonoBehaviour
{
    Texture2D tex;

    public Texture2D GetMobileImageTexture() => tex;
    public void Start()
    {
        //tex = new Texture2D(720, 480, TextureFormat.RGBA4444, false);
        tex = new Texture2D(720, 480, TextureFormat.RGBA32, false);
    }

    public void TextureResize(int width, int height, TextureFormat textureFormat = TextureFormat.RGBA32,
        bool mipChain = false)
    {
        tex = new Texture2D(width, height, textureFormat, mipChain);
    }
    
    
    public void LoadPNG(byte[] pngByte)
    {
        if (pngByte.Length < 1)
            return;

        Debug.Log("Png Received");
        tex.LoadImage(pngByte);
        tex.Apply();
    }
}
