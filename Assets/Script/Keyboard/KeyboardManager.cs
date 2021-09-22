using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private KeyboardCanvas keyboardCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupKeyboard()
    {
        keyboardCanvas.gameObject.SetActive(true);
        DataStructs.vrKeyboardBuffor.Clear();
    }
    
    
    public void CloseKeyboard()
    {
        keyboardCanvas.gameObject.SetActive(false);
    }
    
}
