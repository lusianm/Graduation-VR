using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidDisplayer : MonoBehaviour
{
    [SerializeField] private RectTransform outlineRectTransform, screenRectTransfrom;
    [SerializeField] private RawImage screenRawImage;

    public void SetDisplayTexture(Texture newTexture)
    {
        screenRawImage.material.mainTexture = newTexture;
        screenRawImage.texture = newTexture;
    }

    public Texture GetDisplayTexture() => screenRawImage.texture;

    public void ResetDispalyerSize(float width, float height, float paddingWidth, float paddingHeight)
    {
        outlineRectTransform.sizeDelta = new Vector2(width, height);
        screenRectTransfrom.sizeDelta = new Vector2(height - paddingHeight, width - paddingWidth);
    }

    public void ResetDisplayerPosition(float x, float y)
    {
        outlineRectTransform.position = new Vector2(x, y);
    }







}
