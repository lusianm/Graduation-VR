using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    private bool isGrabbable = false;
    private bool isTriggerable = false;
    [SerializeField] private UnityEvent triggerEvent;
    private Transform parentTranform;
    public bool IsGrabbable() => isGrabbable;
    public bool IsTriggerable() => isTriggerable;
    
    
    // Start is called before the first frame update

    public void OnSelected()
    {
        
    }
    
    public void OffSelected()
    {
        
    }

    public void OnGrap()
    {
        if (!isGrabbable)
            return;

    }

    public void OnGrap(Transform newParent)
    {
        if (!isGrabbable)
            return;
        parentTranform = transform.parent;
        transform.SetParent(newParent);

    }

    public void OffGrap()
    {
        transform.SetParent(parentTranform);
    }
    

    public void OnTrigger()
    {
        if (!isTriggerable)
            return;
        triggerEvent.Invoke();
    }
}
