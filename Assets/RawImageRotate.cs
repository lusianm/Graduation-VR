using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawImageRotate : MonoBehaviour
{
    [SerializeField] RectTransform baseRectTransform;
    [SerializeField] RectTransform rectTransform;

    // Update is called once per frame
    void Update()
    {
        rectTransform.sizeDelta = new Vector2(baseRectTransform.sizeDelta.y, baseRectTransform.sizeDelta.x);
    }
}
