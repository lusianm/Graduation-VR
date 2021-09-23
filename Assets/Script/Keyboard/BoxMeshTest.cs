using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BoxMeshTest : MonoBehaviour
{
    [SerializeField] private Transform startPosition, endPosition, basePosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Bounds boxBound = new Bounds();
        boxBound.min = startPosition.position;
        boxBound.max = endPosition.position; 
        //transform.localScale = new Vector3((boxBound.extents * 2f).x, 1f, (boxBound.extents * 2f).z);
        Vector3 normalA = startPosition.position - basePosition.position;
        Vector3 normalB = endPosition.position - basePosition.position;
        transform.up = Vector3.Cross(normalA, normalB);

        transform.position = boxBound.center;
        //transform.localScale = boxBound.extents*2;

        //transform.localScale = new Vector3((boxBound.extents * 2f).x, 1f, (boxBound.extents * 2f).z);
        transform.localScale = new Vector3(Mathf.Sqrt(boxBound.size.x * boxBound.size.x + boxBound.size.z * boxBound.size.z)/2.0f , 1f,
            Mathf.Sqrt(boxBound.size.y * boxBound.size.y + boxBound.size.z * boxBound.size.z)/0.7f);


    }
}
