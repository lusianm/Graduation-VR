using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirroringManager : MonoBehaviour
{
    private Texture currentSetTexture;
    private bool isMirroringActivated;
    [SerializeField]
    private AndroidDisplayer uiAndroidDisplayer;
    // Start is called before the first frame update
    void Start()
    {
        currentSetTexture = null;
        isMirroringActivated = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMirroringActivated)
            return;

    }

    public void SetMirroringMode(Texture setTexture)
    {
        isMirroringActivated = true;
        currentSetTexture = setTexture;
        uiAndroidDisplayer.gameObject.SetActive(true);
        uiAndroidDisplayer.ResetDispalyerSize(DataStructs.partialTrackingData.cameraResolution.x / 10f,
            DataStructs.partialTrackingData.cameraResolution.y / 10f, 3, 3);
        uiAndroidDisplayer.SetDisplayTexture(setTexture);
    }
    public void closeMirroringMode()
    {
        isMirroringActivated = false;
        uiAndroidDisplayer.gameObject.SetActive(false);
    }
}
