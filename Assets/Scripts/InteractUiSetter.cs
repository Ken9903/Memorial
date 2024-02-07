using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class InteractUiSetter : MonoBehaviour
{
    public float centerLayDistance = 3f;
    public float layDistance = 4f;
    public float optimization_centerUiSet = 0.1f;

    public GameObject interactUi;
    public Camera cam;
    public Canvas mainCanvas;

    public GameObject interactUiShowBox;

    private RaycastHit hit;
    private RaycastHit hit_object;

    int layermaskPlayer = 3;

    private void Start()
    {
        StartCoroutine(centerUiSet());
    }

    IEnumerator centerUiSet() //초점을 통해 열리는 Ui -> 
    {
        bool showing = false;
        while (true)
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * centerLayDistance, Color.blue);
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit_object, centerLayDistance, layermaskPlayer))
            {
                if (hit_object.collider.gameObject.CompareTag("InteractUi") && showing == false)
                {
                    showing = true;
                    makeInteractUi(hit_object.collider.gameObject);
                }
                else if(!hit_object.collider.gameObject.CompareTag("InteractUi"))
                {
                    showing = false;
                }
            }
            yield return new WaitForSeconds(optimization_centerUiSet); //최적화
        }
    }

    public void makeInteractUi(GameObject target)
    {
        //Ui객체에게 넘기기
        GameObject instantUi = Instantiate(interactUi, mainCanvas.transform);
        StartCoroutine(instantUi.GetComponent<InteractUiOn>().changeUiPos(target, layDistance));
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("InteractUi"))
        {
            Debug.DrawRay(other.gameObject.transform.position, cam.transform.position - other.gameObject.transform.position , Color.green);
            if (Physics.Raycast(other.gameObject.transform.position, cam.transform.position - other.gameObject.transform.position, out hit,layDistance))
            {
                if (hit.collider.gameObject.layer == 3) 
                {
                    makeInteractUi(other.gameObject);
                }
            }
        }

        #region DebugcheckPoint
        /*
         * 레이캐스트 차단시 트리거가 엔터 된 상태로 레이캐스트 Hit 조건 성립시 Enter의 특성인 한번 시행으로 인해 플레이가 매끄럽지 않을 수 있음
         * 문제 발생시 Stay로 변경하거나 로직 변경 요망
         */

        #endregion
    }
}
