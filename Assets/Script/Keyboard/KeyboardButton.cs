using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardButton : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial, selectMaterial;
    [SerializeField] private KeyboardButtonType buttonType = KeyboardButtonType.a;
    [SerializeField] private KeyboardCanvas keyboardCanvas;
    private MeshRenderer meshRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ButtonPress()
    {
        meshRenderer.material = selectMaterial;
        Vector3 selectPosition = transform.localPosition;
        selectPosition.y = -0.02f;
        transform.localPosition = selectPosition;
        keyboardCanvas.ButtonPressed(buttonType);
    }

    public void ButtonExit()
    {
        meshRenderer.material = defaultMaterial;
        Vector3 defaultPosition = transform.localPosition;
        defaultPosition.y = 0f;
        transform.localPosition = defaultPosition;
        keyboardCanvas.ButtonExit(buttonType);
        
    }

    public void ButtonUp()
    {
        ButtonExit();
        keyboardCanvas.ButtonUp(buttonType);
    }
    
    
    
}
