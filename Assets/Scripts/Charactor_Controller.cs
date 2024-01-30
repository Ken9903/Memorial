using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor_Controller : MonoBehaviour
{
    public Camera playerCam;

    #region Camera Zoom Variables

    public bool isZoomed = false;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;
    public float fov = 60f;

    #endregion

    public bool cameraMode = false;
    public bool playerCanInteractive = true;


    public Camera_Setter camera_Setter;



    void Update()
    {
        #region CameraSetting

        if (playerCanInteractive && Input.GetKeyDown(KeyCode.E))
        {
            camera_Setter.changeCameraUi(cameraMode);
            cameraMode = !cameraMode;
        }
        #endregion

        #region CameraShot

        if (playerCanInteractive && cameraMode &&Input.GetKeyDown(KeyCode.Mouse0))
        {
            camera_Setter.cameraShot();
        }
        #endregion

    }

    public IEnumerator zoom()
    {
        isZoomed = !isZoomed;
        for (int i = 0; i < 50; i++)
        {
            if (isZoomed)
            {
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            
            else if (!isZoomed)
            {
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
            

            yield return null;
        }

    }


}
