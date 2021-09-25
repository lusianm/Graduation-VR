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

    }

    public void SetMirroringMode(Texture setTexture)
    {
        isMirroringActivated = true;
        currentSetTexture = setTexture;
        uiAndroidDisplayer.gameObject.SetActive(true);
        uiAndroidDisplayer.SetDisplayTexture(setTexture);
        uiAndroidDisplayer.ResetDispalyerSize(DataStructs.mirroringData.screenWidth * 0.6f,
            DataStructs.mirroringData.screenHeight * 0.6f);
    }
    public void closeMirroringMode()
    {
        isMirroringActivated = false;
        uiAndroidDisplayer.gameObject.SetActive(false);
    }
}
