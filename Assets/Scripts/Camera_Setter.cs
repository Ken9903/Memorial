using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Camera_Setter : MonoBehaviour
{
    public GameObject cameraUi;
    public GameObject defaultUi;


    public GameObject camera;

    public Volume volume;
    public ColorAdjustments colorAdjustments;

    public Charactor_Controller charactor_Controller;

    public bool shotCoolDown = false;
    public float shotCoolTime = 1.5f;

    private void Start()
    {
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
    }
    //

    public void changeCameraUi(bool cameraMode)
    {
        if(cameraMode) //카메라 모드 -> 디폴트 모드
        {
            cameraUi.SetActive(false);
            defaultUi.SetActive(true);
            StartCoroutine(charactor_Controller.zoom());
        }
        else //디폴트 모드 -> 카메라 모드
        {
            cameraUi.SetActive(true);
            defaultUi.SetActive(false);
            StartCoroutine(charactor_Controller.zoom());
        }
    }

    public void cameraShot()
    {
        if(!shotCoolDown)
        {
            shotCoolDown = true;
            StartCoroutine(shotCoolTimeProcess());
            StartCoroutine(flash());
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
