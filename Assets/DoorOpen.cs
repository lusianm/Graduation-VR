using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoorTrigger()
    {
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        float movedDistance = 0f;
        while (movedDistance < 1.5f)
        {
            transform.Translate(1f * Time.deltaTime, 0f,0f, Space.Self);
            movedDistance += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(10f);

        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        float movedDistance = 0f;
        while (movedDistance < 1.5f)
        {
            transform.Translate(-1f * Time.deltaTime, 0f, 0f, Space.Self);
            movedDistance += Time.deltaTime;
            yield return null;
        }
    }
    
    
    
}
