using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FingerTipUIButton : MonoBehaviour
{
    public FingerTipUIPanel childFingerTipUIPanel = null;
    public FingerTipUIPanel currentFingerTipUIPanel;
    public bool hasChildPanel() => (childFingerTipUIPanel != null);
    
    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        button = transform.GetComponent<Button>();
        currentFingerTipUIPanel = transform.parent.GetComponent<FingerTipUIPanel>();
        if (childFingerTipUIPanel != null)
        {
            childFingerTipUIPanel.SetParentPanel(currentFingerTipUIPanel);
            childFingerTipUIPanel.gameObject.SetActive(false);
        }

    }

    public FingerTipUIPanel ButtonAction()
    {
        button.onClick.Invoke();
        if(childFingerTipUIPanel != null)
            childFingerTipUIPanel.gameObject.SetActive(true);
        return childFingerTipUIPanel;
    }
    
    
    
}
