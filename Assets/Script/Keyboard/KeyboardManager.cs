using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private KeyboardCanvas keyboardCanvas;

    private Vector3[] keyboardPosition;
    private int currentKeybaordMode;
    [SerializeField] private Transform player;
    private Vector2[] keyboardTouchBound;

    // Start is called before the first frame update
    void Start()
    {
        currentKeybaordMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentKeybaordMode)
        {
            case 0:
                
                break;
            case 1:
                break;
        }
    }

    public void SetupKeyboard(int mode = 0)
    {
        keyboardCanvas.gameObject.SetActive(true);
        DataStructs.vrKeyboardBuffor.Clear();

        currentKeybaordMode = mode;
        switch (currentKeybaordMode)
        {
            case 0:
                keyboardCanvas.transform.SetParent(player);
                keyboardCanvas.transform.localEulerAngles = new Vector3(-50f, 0f, 0f);
                keyboardCanvas.transform.localPosition = new Vector3(0f, -0.2f, 0.5f);
                keyboardCanvas.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                break;
            case 1:
                break;
        }
    }
    
    
    public void CloseKeyboard()
    {
        keyboardCanvas.transform.SetParent(null);
        keyboardCanvas.gameObject.SetActive(false);
    }
    
}
