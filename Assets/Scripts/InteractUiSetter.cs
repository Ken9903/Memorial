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

    IEnumerator centerUiSet() //������ ���� ������ Ui -> 
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
            yield return new WaitForSeconds(optimization_centerUiSet); //����ȭ
        }
    }

    public void makeInteractUi(GameObject target)
    {
        //Ui��ü���� �ѱ��
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
         * ����ĳ��Ʈ ���ܽ� Ʈ���Ű� ���� �� ���·� ����ĳ��Ʈ Hit ���� ������ Enter�� Ư���� �ѹ� �������� ���� �÷��̰� �Ų����� ���� �� ����
         * ���� �߻��� Stay�� �����ϰų� ���� ���� ���
         */

        #endregion
    }
}
