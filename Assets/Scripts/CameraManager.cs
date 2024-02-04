using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public Camera playerCam;

    public bool zooming = false;
    public bool cameraMode = false;
    public bool playerCanInteractive = true;

    public GameObject cameraUi;
    public GameObject defaultUi;

    public Volume volume;
    public ColorAdjustments colorAdjustments;

    public Charactor_Controller charactor_Controller;


    #region shoot

    public GameObject cameraCheckCollider;
    public bool shotCoolDown = false;
    public float shotCoolTime = 1.5f;

    #endregion

    #region Camera Zoom Variables

    public bool isZoomed = false;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;
    public float fov = 60f;

    #endregion

    #region filmEffect

    public FilmGrain filmGrain;
    public float filmIntensity = 0.6f;
    public float filmResponse = 0.2f;

    #endregion

    private void Start()
    {
        //SceneChange할 때 다시 호출.
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out filmGrain);
    }

    void Update()
    {
        #region CameraSetting

        if (playerCanInteractive && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(changeCamera(cameraMode));
            cameraMode = !cameraMode;
        }
        #endregion

        #region CameraShot

        if (playerCanInteractive && cameraMode && Input.GetKeyDown(KeyCode.Mouse0))
        {
            cameraShot();
        }
        #endregion

    }

    IEnumerator changeCamera(bool cameraMode)
    {
        if (cameraMode) //카메라 모드 -> 디폴트 모드
        {
            zooming = true;

            deleteNoise();
            cameraUi.SetActive(false);
            defaultUi.SetActive(true);
            StartCoroutine(zoom());
        }
        else //디폴트 모드 -> 카메라 모드
        {
            zooming = true;
            defaultUi.SetActive(false);
            StartCoroutine(zoom());
            cameraUi.SetActive(true);

            while (zooming)
            {
                yield return null;
            }

            makeNoise();

        }
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

        zooming = false;

    }
    
    public void makeNoise()
    {
        filmGrain.active = true;
        filmGrain.intensity.value = filmIntensity;
        filmGrain.response.value = filmResponse;
    }
    public void deleteNoise()
    {
        filmGrain.active = false;
    }
    public void cameraShot()
    {
        if(!shotCoolDown)
        {
            shotCoolDown = true;
            StartCoroutine(shotCoolTimeProcess());
            StartCoroutine(flash());

            checkShotObject();

        }
    }
    public void checkShotObject()
    {
        Collider[] colliders =
                   Physics.OverlapBox(cameraCheckCollider.transform.position, cameraCheckCollider.transform.localScale,cameraCheckCollider.transform.rotation);

        foreach (Collider col in colliders)
        {
            if(col.tag == "Enemy")
            {
                Debug.Log("Enemy");
            }
            else if(col.tag == "Puzzle")
            {
                Debug.Log("Puzzle");
            }
            else if(col.tag == "ScenarioTrigger")
            {
                Debug.Log("ScenarioTrigger");
            }
            else
            {

            }
        }
    }

    IEnumerator shotCoolTimeProcess()
    {
        //*개발 Ui 추가
        yield return new WaitForSeconds(shotCoolTime);
        shotCoolDown = false;
    }

    IEnumerator flash()
    {
        colorAdjustments.postExposure.value = 2.5f;

        for(int i = 0; i < 100; i++)
        {
            colorAdjustments.postExposure.value -= 0.015f;
            yield return new WaitForSeconds(0.002f);
        }

        yield return null;
    }
   
}
