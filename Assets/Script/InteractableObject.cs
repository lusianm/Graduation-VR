using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool isGrabbable = false;
    [SerializeField] private bool isTriggerable = false;
    [SerializeField] private UnityEvent triggerEvent;
    [SerializeField] private Material selectedMaterial;
    private Material defaultMaterial;
    [SerializeField] private GameObject highlightObject;
    private MeshRenderer objectMeshRenderer;
    private Transform parentTranform;
    public bool IsGrabbable() => isGrabbable;
    public bool IsTriggerable() => isTriggerable;

    private void Start()
    {
        objectMeshRenderer = highlightObject.GetComponent<MeshRenderer>();
        defaultMaterial = objectMeshRenderer.material;
    }


    // Start is called before the first frame update

    public void OnSelected()
    {
        objectMeshRenderer.material = selectedMaterial;
    }
    
    public void OffSelected()
    {
        objectMeshRenderer.material = defaultMaterial;
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
